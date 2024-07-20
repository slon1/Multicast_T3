using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class GameHub : Hub {
	private readonly IGrainFactory _grainFactory;
	private static readonly ConcurrentDictionary<string, string> PlayerConnections = new ConcurrentDictionary<string, string>();

	public GameHub(IGrainFactory grainFactory) {
		_grainFactory = grainFactory;
	}

	public override Task OnDisconnectedAsync(Exception? exception) {
		var playerIdToRemove = PlayerConnections.FirstOrDefault(kvp => kvp.Value == Context.ConnectionId).Key;
		if (!string.IsNullOrEmpty(playerIdToRemove)) {
			PlayerConnections.TryRemove(playerIdToRemove, out _);
		}
		return base.OnDisconnectedAsync(exception);
	}

	public async Task SendBet(string playerId, int bet) {
		PlayerConnections[playerId] = Context.ConnectionId;
		var playerGrain = _grainFactory.GetGrain<IPlayerGrain>(playerId);
		await playerGrain.SetBet(bet);

		var gameQueueGrain = _grainFactory.GetGrain<IGameQueueGrain>(0);
		await gameQueueGrain.AddPlayerToQueue(playerId);

		var players = await gameQueueGrain.GetPlayersInQueue();
		if (players.Count >= 2) {
			await CalculateGame(gameQueueGrain, players);
			if (PlayerConnections.ContainsKey(players[0])) {
				await Clients.Client(PlayerConnections[players[0]]).SendAsync("ReceiveBet", playerId, bet);
			}
			
		}
		
	}

	private async Task CalculateGame(IGameQueueGrain gameQueueGrain, List<string> players) {
		var player1 = players[0];
		var player2 = players[1];

		var bet1 = await _grainFactory.GetGrain<IPlayerGrain>(player1).GetBet();
		var bet2 = await _grainFactory.GetGrain<IPlayerGrain>(player2).GetBet();

		var gameRoom = _grainFactory.GetGrain<IGameRoomGrain>($"{player1}vs{player2}");
		await gameRoom.StartGame(player1, bet1, player2, bet2);

		var state = await gameRoom.GetState();
		if (PlayerConnections.TryGetValue(player1, out var connectionId1)) {
			await Clients.Client(connectionId1).SendAsync("ReceiveResult", state);
		}
		if (PlayerConnections.TryGetValue(player2, out var connectionId2)) {
			await Clients.Client(connectionId2).SendAsync("ReceiveResult", state);
		}

		await gameQueueGrain.RemovePlayerFromQueue(player1);
		await gameQueueGrain.RemovePlayerFromQueue(player2);
	}

	public async Task<int> GetPoints(string playerId) {
		var playerGrain = _grainFactory.GetGrain<IPlayerGrain>(playerId);
		return await playerGrain.GetPoints();
	}
}
