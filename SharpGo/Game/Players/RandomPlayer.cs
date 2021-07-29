using System.Collections.Generic;
using System.Linq;
using MathlibNET;
using MathlibNET.Random;
using Source.Game;

namespace SharpGo.Game.Players
{
	public class RandomPlayer : Player
	{
		public RandomPlayer(Color color) : base(color) { }

		protected override (int, int) PickPosition(Board board, HashSet<Intersection> validIntersections)
		{
			int index = Rng.Rand(validIntersections.Count);
			Intersection intersection = validIntersections.ToArray()[index];
			return (intersection.I, intersection.J);
		}

		protected override bool Pass(Board board, HashSet<Intersection> validIntersections) =>
			Rng.Rand() < SpecialFunctions.Lerp(NbTurnsPlayed, 0, 1000, 0, 0.5);
	}
}
