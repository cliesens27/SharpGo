using SharpGo.Game;

namespace SharpGo.Game
{
	public struct Intersection
	{
		public State State { get; }
		public int I { get; }
		public int J { get; }

		public Intersection(State state, int i, int j) => (State, I, J) = (state, i, j);
	}
}
