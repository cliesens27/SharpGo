using System;
using Source.Game;

namespace SharpGo.Game.Players
{
	public abstract class Player
	{
		public Color Color { get; }

		protected Player(Color color) => Color = color;

		protected void PlayStone(Board board, int i, int j) => board.AddStone(Color, i, j);

		protected abstract bool Pass();

		protected abstract (int, int) PickPosition(Board board, (State, int, int)[] validCells);

		public void Play(Board board)
		{
			if (Pass())
			{
				return;
			}

			(State, int, int)[] validCells = board.GetUnoccupiedCellsArray(Color);

			if (validCells.Length == 0)
			{
				throw new ArgumentException("Cannot pick a position, there are no valid cells.");
			}

			(int i, int j) = PickPosition(board, validCells);

			PlayStone(board, i, j);
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
