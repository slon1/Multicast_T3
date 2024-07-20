public interface IPlayerGrain : IGrainWithStringKey {
	Task SetBet(int bet);
	Task <int>GetBet();
	Task<int> GetPoints();
	Task SetPoints(int points);
}
