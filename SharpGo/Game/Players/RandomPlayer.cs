using System.Collections.Generic;
using System.Linq;
using MathlibNET;
using MathlibNET.Random;
using Source.Game;

namespace SharpGo.Game.Players
{
	internal class RandomPlayer : Player
	{
		internal RandomPlayer(PlayerColor color) : base(color) { }

		protected override (int, int) PickPosition(Board board, HashSet<Intersection> legalIntersections)
		{
			int index = Rng.Rand(legalIntersections.Count);
			Intersection intersection = legalIntersections.ToArray()[index];
			return (intersection.I, intersection.J);
		}

		protected override bool Pass(Board board, HashSet<Intersection> legalIntersections) =>
			Rng.Rand() < SpecialFunctions.Lerp(NbTurnsPlayed, 0, 1000, 0, 0.5);
	}
}
