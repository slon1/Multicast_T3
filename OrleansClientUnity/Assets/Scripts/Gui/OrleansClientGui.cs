using System;
using System.Text;
using UnityEngine;


public class OrleansClientGui : MonoBehaviour {
	[SerializeField]
	private LoginGui LoginGui;
	[SerializeField]
	private GameGui GameGui;		
	private OrleansClient client;
	[SerializeField]
	private string url;
	[SerializeField]
	private string player;
	[SerializeField]
	private string hub;
	[SerializeField]
	private int port;
	private void Awake() {
		UnityMainThreadDispatcher.Initialize();
	}
	
	void Start() {
		player = "user" + UnityEngine.Random.Range(0, 100);
		LoginGui.playerName.text = player;
		LoginGui.AddListner(OnLoginClick);
		GameGui.AddListner(OnSetBetClick);
		client = new OrleansClient($"{url}:{port}/{hub}");
		client.onReceiveBet += Client_onReceiveBet;
		client.onReceiveResult += Client_onReceiveResult;
	}

	private void Client_onReceiveResult(GameRoomState state) {
		UnityMainThreadDispatcher.EnqueueAction(async () => {
			if (state.Player1 == player) {
				GameGui.player2Name.SetText(state.Player2);
				GameGui.player2Bet.SetText("Player bet " + state.Bet2);
			}
			else {
				GameGui.player2Name.SetText(state.Player1);
				GameGui.player2Bet.SetText("Player bet " + state.Bet1);
			}
			GameGui.playerScore.SetText("Player score " + await client.GetPoints(player));
			GameGui.AIBet.SetText("Ai said " + state.SecretNumber);
			print($"{state.Player1} : {state.Bet1}, {state.Player2} : {state.Bet2}, Ai : {state.SecretNumber}. {state.Winner} Win!!!");
		});

	}
	public void Exit() {
		Application.Quit();
	}
	private void Client_onReceiveBet(string player, int bet) {
		print($"{player} said {bet}, start game...");
	}

	private async void OnSetBetClick() {
		int.TryParse(GameGui.inputField.text, out var result);
		await client.SendBet(player, Mathf.Clamp(result, 0, 100));
	}

	public async void OnLoginClick() {
		if (await client.ConnectToHub()) {
			StartGame();
		}
	}

	public void SetPlayerName(string payerName) {
		player = payerName;
	}	

	public async void StartGame() {
		LoginGui.Show(false);
		GameGui.Show(true);
		GameGui.playerName.text = player;
		GameGui.playerScore.text = "Player score: " + await client.GetPoints(player);
	}	

	private void OnDestroy() {
		client.onReceiveBet -= Client_onReceiveBet;
		client.onReceiveResult -= Client_onReceiveResult;
		LoginGui.Dispose();
		GameGui.Dispose();
		client.Dispose();
	}

}
