using System;
using System.Collections.Generic;
using Source.Game;

namespace SharpGo.Game.Players
{
	public abstract class Player
	{
		public Color Color { get; }

		protected Player(Color color) => Color = color;

		protected void PlaceStone(Board board, int i, int j) => board.PlaceStone(Color, i, j);

		protected abstract bool Pass();

		protected abstract (int, int) PickPosition(Board board, HashSet<(State, int, int)> validIntersections);

		public void Play(Board board)
		{
			if (Pass())
			{
				return;
			}

			HashSet<(State, int, int)> validIntersections = board.GetUnoccupiedValidIntersections(Color);

			if (validIntersections.Count == 0)
			{
				throw new ArgumentException("Cannot pick a position, there are no valid intersections.");
			}

			(int i, int j) = PickPosition(board, validIntersections);

			PlaceStone(board, i, j);
			// Capture
			// Self-capture
		}

		public static State PlayerColorToState(Color color)
		{
			if (color == Color.White)
			{
				return State.White;
			}
			else if (color == Color.Black)
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
