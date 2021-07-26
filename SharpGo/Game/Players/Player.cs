using System;
using Source.Game;

namespace SharpGo.Game.Players
{
	public abstract class Player
	{
		public Color Color { get; }

		public abstract void MakeMove(Board board);

		protected Player(Color color)
		{
			Color = color;
		}

		protected void AddStone(Board board, int i, int j) => board.AddStone(Color, i, j);

		protected abstract bool Pass();

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
