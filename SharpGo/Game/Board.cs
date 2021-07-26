using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharpGo.Game.Players;

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

		public (State, int, int)[] GetUnoccupiedCellsArray(Color color) =>
			GetUnoccupiedAndValidCells(color).Cast<(State, int, int)>().ToArray();

		public void AddStone(Color color, int i, int j)
		{
			if (IsOccupied(i, j))
			{
				throw new InvalidOperationException($"Cannot add stone to cell ({i},{j}), it is already occupied.");
			}

			if (i < 0 || i >= NbRows || j < 0 || j >= NbCols)
			{
				throw new IndexOutOfRangeException($"Cannot add stone to cell ({i},{j}), it is outside the board.");
			}

			states[i][j] = Player.PlayerColorToState(color);
		}

		public IEnumerable<(State, int, int)> GetUnoccupiedAndValidCells(Color color)
		{
			foreach ((State s, int i, int j) in GetValidCells(color))
			{
				if (!IsOccupied(i, j))
				{
					yield return (s, i, j);
				}
			}
		}

		private IEnumerable<(State, int, int)> GetValidCells(Color color)
		{
			for (int i = 0; i < NbRows; i++)
			{
				for (int j = 0; j < NbCols; j++)
				{
					if (color == Color.White && !(this[i, j] == State.EmptyWhite || this[i, j] == State.EmptyBoth))
					{
						yield return (this[i, j], i, j);
					}
					else if (color == Color.Black && !(this[i, j] == State.EmptyBlack || this[i, j] == State.EmptyBoth))
					{
						yield return (this[i, j], i, j);
					}
					else
					{
						throw new ArgumentException("Bad player color.");
					}
				}
			}
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
