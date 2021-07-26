using System.Collections.Generic;
using System.Linq;
using MathlibNET.Random;
using Source.Game;

namespace SharpGo.Game.Players
{
	public class RandomPlayer : Player
	{
		public RandomPlayer(Color color) : base(color) { }

		protected override (int, int) PickPosition(Board board, HashSet<(State, int, int)> intersections)
		{
			int index = Rng.Rand(intersections.Count);
			(_, int i, int j) = intersections.ToArray()[index];
			return (i, j);
		}

		protected override bool Pass() => Rng.Rand() < 0.25;
	}
}
