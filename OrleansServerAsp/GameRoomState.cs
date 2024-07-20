[GenerateSerializer]
public class GameRoomState {
	[Id(0)]
	public string Player1 { get; set; }

	[Id(1)]
	public int Bet1 { get; set; }

	[Id(2)]
	public string Player2 { get; set; }

	[Id(3)]
	public int Bet2 { get; set; }

	[Id(4)]
	public int SecretNumber { get; set; }

	[Id(5)]
	public string Winner { get; set; }
}
