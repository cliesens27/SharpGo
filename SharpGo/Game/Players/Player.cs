using System;
using System.Collections.Generic;
using Source.Game;

namespace SharpGo.Game.Players
{
	public abstract class Player
	{
		public PlayerColor Color { get; }
		public bool Passed { get; private set; }
		public int NbTurnsPlayed { get; private set; }

		protected Player(PlayerColor color) => Color = color;

		protected void PlaceStone(Board board, int i, int j) => board.PlaceStone(Color, i, j);

		protected void Capture(HashSet<Intersection> intersections, Board board)
		{
			foreach(var intersection in intersections)
			{
				board.Capture(intersection);
			}
		}

		protected abstract bool Pass(Board board, HashSet<Intersection> legalIntersections);

		protected abstract (int, int) PickPosition(Board board, HashSet<Intersection> legalIntersections);

		public void Play(Board board)
		{
			HashSet<Intersection> legalIntersections = board.GetLegalIntersections(Color);
			HashSet<Intersection> capturableIntersections = board.GetLegalIntersectionsWithNoLiberties(Color);

			if (Pass(board, legalIntersections) || legalIntersections.Count == 0)
			{
				Passed = true;
				return;
			}

			(int i, int j) = PickPosition(board, legalIntersections);

			PlaceStone(board, i, j);
			Capture(capturableIntersections, board);

			Passed = false;
			NbTurnsPlayed++;
		}

		public static State PlayerColorToState(PlayerColor color)
		{
			if (color == PlayerColor.White)
			{
				return State.White;
			}
			else if (color == PlayerColor.Black)
			{
				return State.Black;
			}
			else
			{
				throw new ArgumentException("Bad player color.");
			}
		}
	}
}
