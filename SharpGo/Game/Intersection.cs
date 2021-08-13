using Source.Game;

namespace SharpGo.Game
{
	internal struct Intersection
	{
		internal State State { get; }
		internal int I { get; }
		internal int J { get; }

		internal Intersection(State state, int i, int j) => (State, I, J) = (state, i, j);
	}
}
