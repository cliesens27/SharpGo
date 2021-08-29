using System;
using System.Collections.Generic;
using SharpGo.Game.Players;

namespace SharpGo.Game
{
	public class Board
	{
		public State this[int i, int j]
		{
			get
			{
				if (!Utils.IsInsideBoard(i, j))
				{
					throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
				}

				return states[i][j];
			}
			private set
			{
				if (!Utils.IsInsideBoard(i, j))
				{
					throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
				}

				states[i][j] = value;
			}
		}

		public int Size { get; }
		public int NbIntersections { get; }
		public BoardUtils Utils { get; }

		public int NbEmptyIntersections { get; private set; }
		public int NbWhiteStones { get; private set; } = 0;
		public int NbBlackStones { get; private set; } = 0;

		public bool IsEmpty => (NbWhiteStones == 0) && (NbBlackStones == 0) && (NbEmptyIntersections == Size * Size);

		private readonly State[][] states;

		public Board(int size = 19)
		{
			Utils = new BoardUtils(this);

			Size = size;
			NbIntersections = Size * Size;
			NbEmptyIntersections = NbIntersections;
			states = new State[Size][];

			for (int i = 0; i < Size; i++)
			{
				states[i] = new State[Size];
			}

			Reset();
		}

		public void PlaceStone(PlayerColor color, int i, int j)
		{
			if (Utils.IsOccupied(i, j))
			{
				throw new InvalidOperationException($"Cannot place stone on intersection ({i},{j}), it is already occupied.");
			}

			if (!Utils.IsInsideBoard(i, j))
			{
				throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
			}

			this[i, j] = Player.PlayerColorToState(color);

			if (color == PlayerColor.White)
			{
				NbWhiteStones++;
			}
			else if (color == PlayerColor.Black)
			{
				NbBlackStones++;
			}

			NbEmptyIntersections--;
		}

		public void Capture(int i, int j)
		{
			if (this[i, j] == State.White)
			{
				this[i, j] = State.EmptyWhite;

				NbWhiteStones--;
				NbEmptyIntersections++;
			}
			else if (this[i, j] == State.Black)
			{
				this[i, j] = State.EmptyBlack;

				NbBlackStones--;
				NbEmptyIntersections++;
			}
		}

		public void UpdateEmptyIntersections()
		{
			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					if (Utils.IsEmpty(i, j))
					{
						this[i, j] = State.Empty;
					}
				}
			}
		}

		public void PlaceTempStone(PlayerColor color, int i, int j)
		{
			if (Utils.IsOccupied(i, j))
			{
				throw new InvalidOperationException($"Cannot place stone on intersection ({i},{j}), it is already occupied.");
			}

			if (!Utils.IsInsideBoard(i, j))
			{
				throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
			}

			this[i, j] = Player.PlayerColorToState(color);
		}

		public void RemoveTempStone(State state, int i, int j)
		{
			if (this[i, j] == State.White || this[i, j] == State.Black)
			{
				this[i, j] = state;
			}
		}

		private void Reset()
		{
			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					this[i, j] = State.Empty;
				}
			}
		}
	}
}
