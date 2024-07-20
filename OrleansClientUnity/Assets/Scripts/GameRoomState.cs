using System;

[Serializable]
public class GameRoomState {
	public string Player1 { get; set; }
	public int Bet1 { get; set; }
	public string Player2 { get; set; }
	public int Bet2 { get; set; }
	public int SecretNumber { get; set; }
	public string Winner { get; set; }
}
