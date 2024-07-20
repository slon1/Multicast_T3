namespace OrleansServer {
	[GenerateSerializer]
	public class PlayerGrainState {
		[Id(0)]
		public int _points { get; set; }
		
		
		[Id(1)]
		public int _bet { get; set; }
	}
}



