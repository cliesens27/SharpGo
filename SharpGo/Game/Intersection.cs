using System;

namespace SharpGo.Game
{
	public class Intersection
	{
		public State State { get; }
		public int I { get; }
		public int J { get; }

		public Intersection(State state, int i, int j) => (State, I, J) = (state, i, j);

		public override bool Equals(object obj)
		{
			if (!(obj is Intersection o))
			{
				return false;
			}

			return State == o.State && I == o.I && J == o.J;
		}

		public override int GetHashCode() => Tuple.Create(State, I, J).GetHashCode();

		public static bool operator ==(Intersection x, Intersection y) => x.Equals(y);

		public static bool operator !=(Intersection x, Intersection y) => !x.Equals(y);

		public override string ToString() => $"({State}, {I}, {J})";
	}
}
