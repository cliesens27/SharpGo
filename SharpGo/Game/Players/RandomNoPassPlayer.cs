using System.Collections.Generic;
using System.Linq;
using MathlibNET.Random;

namespace SharpGo.Game.Players
{
	public class RandomNoPassPlayer : Player
	{
		public RandomNoPassPlayer(PlayerColor color) : base(color) { }

		protected override (int, int) PickPosition(Board board, HashSet<Intersection> legalIntersections)
		{
			int index = Rng.Rand(legalIntersections.Count);
			Intersection intersection = legalIntersections.ToArray()[index];
			return (intersection.I, intersection.J);
		}

		protected override bool Pass(Board board, HashSet<Intersection> legalIntersections) => true;
	}
}
