using Orleans.Runtime;
using OrleansServer;

public class PlayerGrain : Grain, IPlayerGrain{

	private readonly IPersistentState<PlayerGrainState> state;

	public PlayerGrain([PersistentState("playerState", "File")] IPersistentState<PlayerGrainState> state) {
		this.state = state;
	}

	public async Task SetBet(int bet) {
		state.State._bet = bet;
		
	}

	public override async Task OnActivateAsync(CancellationToken token) {
		await state.ReadStateAsync();
		await base.OnActivateAsync(token);
	}

	public Task<int> GetPoints() {
		return Task.FromResult(state.State._points);
	}

	public async Task SetPoints(int points) {
		state.State._points += points;
		await state.WriteStateAsync();
	}

	public Task<int> GetBet() {
		return Task.FromResult(state.State._bet);
	}

}
