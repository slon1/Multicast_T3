using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class GameGui : IDisposable {
	[SerializeField]
	private GameObject gameRoot;
	public TextMeshProUGUI playerName;
	public TextMeshProUGUI playerScore;
	public TextMeshProUGUI playerBet;
	public TMP_InputField inputField;

	public TextMeshProUGUI player2Name;	
	public TextMeshProUGUI player2Bet;
	
	public TextMeshProUGUI AIBet;
		

	[SerializeField]
	private Button betBtn;

		
	public void AddListner(UnityAction action) {
		betBtn.onClick.AddListener(action);
	}

	public void Dispose() {
		
	}
	public void Show(bool show) {
		gameRoot.SetActive(show);
	}
}
