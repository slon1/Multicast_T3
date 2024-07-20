public class GameQueueGrain : Grain, IGameQueueGrain {
	private readonly List<string> _players = new List<string>();

	public Task AddPlayerToQueue(string playerId) {
		if (!_players.Contains(playerId)) {
			_players.Add(playerId);
		}
		return Task.CompletedTask;
	}

	public Task<List<string>> GetPlayersInQueue() {
		return Task.FromResult(_players);
	}

	public Task RemovePlayerFromQueue(string playerId) {
		_players.Remove(playerId);
		return Task.CompletedTask;
	}
}
