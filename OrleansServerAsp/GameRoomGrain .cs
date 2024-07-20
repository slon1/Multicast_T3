public class GameRoomGrain : Grain, IGameRoomGrain {
	private GameRoomState state;
	private const int max = 101;

	public override async Task OnActivateAsync(CancellationToken token) {
		state = new GameRoomState {
			SecretNumber = new Random().Next(1, max)
		};
		await base.OnActivateAsync(token);

	}

	public async Task StartGame(string player1Id, int bet1, string player2Id, int bet2) {
		state.Player1 = player1Id;
		state.Bet1 = bet1;
		state.Player2 = player2Id;
		state.Bet2 = bet2;

		await EvaluateGame();
	}

	private async Task EvaluateGame() {
		var distance1 = Math.Abs(state.SecretNumber - state.Bet1);
		var distance2 = Math.Abs(state.SecretNumber - state.Bet2);

		state.Winner = distance1 < distance2 ? state.Player1 : state.Player2;
		var winnerGrain = GrainFactory.GetGrain<IPlayerGrain>(state.Winner);
		await winnerGrain.SetPoints(1);
		state.SecretNumber = new Random().Next(1, max);
		await Task.CompletedTask;
	}

	public Task<GameRoomState> GetState() {
		return Task.FromResult(state);
	}
}
