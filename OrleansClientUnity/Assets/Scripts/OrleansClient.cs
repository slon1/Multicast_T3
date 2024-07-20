using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class OrleansClient :IDisposable {
	private HubConnection connection;
	public event Action<string, int> onReceiveBet;
	public event Action<GameRoomState> onReceiveResult;

	public OrleansClient(string url)
    {
		connection = new HubConnectionBuilder()
			.WithUrl(url)
			.Build();

		connection.On<GameRoomState>("ReceiveResult", OnReceiveResult);
		connection.On<string, int>("ReceiveBet", OnReceiveBet);
	}

	private void OnReceiveBet(string playerName, int bet) {		
		onReceiveBet?.Invoke(playerName, bet);
	}

	private void OnReceiveResult(GameRoomState state) {		
		onReceiveResult?.Invoke(state);		
	}	

	public async Task<bool> ConnectToHub() {
		try {
			await connection.StartAsync();
			Debug.Log("Connected to SignalR Hub");
			return true;
		}
		catch (System.Exception ex) {
			Debug.LogError($"Failed to connect to SignalR Hub: {ex.Message}");
			return false;
		}
	}	

	public async Task SendBet(string playerId, int bet) {
		if (connection.State == HubConnectionState.Connected) {
			try {
				await connection.InvokeAsync("SendBet", playerId, bet);
				Debug.Log("Bet sent: " + bet);
			}
			catch (System.Exception ex) {
				Debug.LogError("Failed to send bet: " + ex.Message);
			}
		}
		else {
			Debug.LogError("Failed to send bet: " + connection.State);
		}
	}
	public async Task<int> GetPoints(string playerId) {
		try {
			return await connection.InvokeAsync<int>("GetPoints", playerId);
		}
		catch (Exception ex) {
			Debug.LogError($"Error getting points: {ex.Message}");
			return -1;
		}
	}	

	public async void Dispose() {
		await connection.StopAsync();
		await connection.DisposeAsync();
	}
}
