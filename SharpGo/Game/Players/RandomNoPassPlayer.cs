using System.Collections.Generic;
using System.Linq;
using DrawingBoardNET.Drawing;

namespace SharpGo.Game.Players
{
	public class RandomNoPassPlayer : Player
	{
		public RandomNoPassPlayer(PlayerColor color) : base(color) { }

		protected override (int, int) PickPosition(Board board, HashSet<Intersection> legalIntersections)
		{
			int index = MathUtils.Rand(legalIntersections.Count);
			Intersection intersection = legalIntersections.ToArray()[index];
			return (intersection.I, intersection.J);
		}

		protected override bool Pass(Board board, HashSet<Intersection> legalIntersections) => false;
	}
}
