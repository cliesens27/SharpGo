using System;

namespace Source.Game
{
	public class Board
	{
		public State this[int i, int j] => states[i][j];
		public int NbRows { get; }
		public int NbCols { get; }

		private readonly State[][] states;

		public Board(int size)
		{
			NbRows = NbCols = size;

			states = new State[NbRows][];

			for (int i = 0; i < NbRows; i++)
			{
				states[i] = new State[NbCols];
			}

			Reset();
		}

		public void AddWhiteStone(int i, int j) => AddStone(State.White, i, j);

		public void AddBlackStone(int i, int j) => AddStone(State.Black, i, j);

		private void AddStone(State state, int i, int j)
		{
			if (IsOccupied(i, j))
			{
				throw new InvalidOperationException($"Cannot add stone to cell ({i},{j}), it is already occupied.");
			}

			if (i < 0 || i >= NbRows || j < 0 || j >= NbCols)
			{
				throw new IndexOutOfRangeException($"Cannot add stone to cell ({i},{j}), it is outside the board.");
			}

			states[i][j] = state;
		}

		private bool IsOccupied(int i, int j) => this[i, j] != State.Empty;

		private void Reset()
		{
			for (int i = 0; i < NbRows; i++)
			{
				for (int j = 0; j < NbCols; j++)
				{
					states[i][j] = State.Empty;
				}
			}
		}
	}
}
