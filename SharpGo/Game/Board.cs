using System;
using SharpGo.Game.Players;

namespace SharpGo.Game
{
	public class Board
	{
		public Intersection this[int i, int j]
		{
			get
			{
				if (!Utils.IsInsideBoard(i, j))
				{
					throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
				}

				return intersections[i][j];
			}
			private set
			{
				if (!Utils.IsInsideBoard(i, j))
				{
					throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
				}

				intersections[i][j] = value;
			}
		}

		public int Size { get; }
		public int NbIntersections { get; }
		public BoardUtils Utils { get; }

		public int NbEmptyIntersections { get; private set; }
		public int NbWhiteStones { get; private set; } = 0;
		public int NbBlackStones { get; private set; } = 0;

		public bool IsEmpty => (NbWhiteStones == 0) && (NbBlackStones == 0) && (NbEmptyIntersections == Size * Size);

		private readonly Intersection[][] intersections;

		public Board(int size = 19)
		{
			Utils = new BoardUtils(this);

			Size = size;
			NbIntersections = Size * Size;
			NbEmptyIntersections = NbIntersections;
			NbWhiteStones = NbBlackStones = 0;
			intersections = new Intersection[Size][];

			for (int i = 0; i < Size; i++)
			{
				intersections[i] = new Intersection[Size];

				for (int j = 0; j < Size; j++)
				{
					intersections[i][j] = new Intersection(State.Empty, i, j);
				}
			}
		}

		public Board(Board board)
		{
			Utils = new BoardUtils(this);

			Size = board.Size;
			NbIntersections = Size * Size;
			NbEmptyIntersections = board.NbIntersections;
			NbWhiteStones = board.NbWhiteStones;
			NbBlackStones = board.NbBlackStones;

			intersections = new Intersection[Size][];

			for (int i = 0; i < Size; i++)
			{
				intersections[i] = new Intersection[Size];

				for (int j = 0; j < Size; j++)
				{
					intersections[i][j] = new Intersection(board[i, j].State, board[i, j].I, board[i, j].J);
				}
			}
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

			this[i, j].State = Player.PlayerColorToState(color);

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
			if (this[i, j].State == State.White)
			{
				this[i, j].State = State.EmptyWhite;

				NbWhiteStones--;
				NbEmptyIntersections++;
			}
			else if (this[i, j].State == State.Black)
			{
				this[i, j].State = State.EmptyBlack;

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
						this[i, j].State = State.Empty;
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

			this[i, j].State = Player.PlayerColorToState(color);
		}

		public void RemoveTempStone(State state, int i, int j)
		{
			if (this[i, j].State == State.White || this[i, j].State == State.Black)
			{
				this[i, j].State = state;
			}
		}
	}
}
