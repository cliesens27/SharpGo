using System;
using System.Collections.Generic;
using System.Text;

namespace Source.Game
{
	public class Board
	{
		public int NbRows { get; }
		public int NbCols { get; }

		private State[][] board;

		protected Board(int rows, int cols)
		{
			NbRows = rows;
			NbCols = cols;

			board = new State[rows][];

			for (int i = 0; i < rows; i++)
			{
				board[i] = new State[cols];

				Reset();
			}
		}

		private void Reset()
		{
			for (int i = 0; i < NbRows; i++)
			{
				for (int j = 0; j < NbCols; j++)
				{
					board[i][j] = State.Empty;
				}
			}
		}
	}
}
