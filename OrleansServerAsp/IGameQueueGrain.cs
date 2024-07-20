public interface IGameQueueGrain : IGrainWithIntegerKey {
	Task AddPlayerToQueue(string playerId);
	Task<List<string>> GetPlayersInQueue();
	Task RemovePlayerFromQueue(string playerId);
}
