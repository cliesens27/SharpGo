using System;
using System.Collections.Generic;

namespace SharpGo.Game.Players
{
	public abstract class Player
	{
		public PlayerColor Color { get; }
		public bool HasPassed { get; private set; }
		public int NbTurnsPlayed { get; private set; }
		public Intersection LastMove { get; private set; } = null;

		protected Player(PlayerColor color) => Color = color;

		protected void PlaceStone(Board board, int i, int j) => board.PlaceStone(Color, i, j);

		protected void Capture(HashSet<Intersection> intersections, Board board)
		{
			foreach (var intersection in intersections)
			{
				board.Capture(intersection.I, intersection.J);
			}
		}

		protected abstract bool Pass(Board board, HashSet<Intersection> legalIntersections);

		protected abstract (int, int) PickPosition(Board board, HashSet<Intersection> legalIntersections);

		public void Play(Board board)
		{
			HashSet<Intersection> legalIntersections = board.Utils.GetLegalIntersections(Color);

			if (Pass(board, legalIntersections) || legalIntersections.Count == 0)
			{
				LastMove = null;
				HasPassed = true;
				NbTurnsPlayed++;
				return;
			}

			(int i, int j) = PickPosition(board, legalIntersections);

			PlaceStone(board, i, j);

			HashSet<Intersection> capturableIntersections = board.Utils.GetCapturableIntersections(Color);
			Capture(capturableIntersections, board);

			LastMove = new Intersection(PlayerColorToState(Color), i, j);
			HasPassed = false;
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
