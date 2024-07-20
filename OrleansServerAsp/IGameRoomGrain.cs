public interface IGameRoomGrain : IGrainWithStringKey
{
    Task StartGame(string player1Id, int bet1, string player2Id, int bet2);
    Task<GameRoomState> GetState();
}
