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
				if (!utils.IsInsideBoard(i, j))
				{
					throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
				}

				return states[i][j];
			}
			private set
			{
				if (!utils.IsInsideBoard(i, j))
				{
					throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
				}

				states[i][j] = value;
			}
		}

		public int Size { get; }
		public int NbIntersections { get; }

		public int NbEmptyIntersections { get; private set; }
		public int NbWhiteStones { get; private set; } = 0;
		public int NbBlackStones { get; private set; } = 0;

		public bool IsEmpty => (NbWhiteStones == 0) && (NbBlackStones == 0) && (NbEmptyIntersections == Size * Size);

		private readonly State[][] states;
		private readonly BoardUtils utils;

		public Board(int size = 19)
		{
			utils = new BoardUtils(this);

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
			if (utils.IsOccupied(i, j))
			{
				throw new InvalidOperationException($"Cannot place stone on intersection ({i},{j}), it is already occupied.");
			}

			if (!utils.IsInsideBoard(i, j))
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

		public void Capture(Intersection intersection)
		{
			if (this[intersection.I, intersection.J] == State.White)
			{
				this[intersection.I, intersection.J] = State.EmptyWhite;
				NbWhiteStones--;
			}
			else if (this[intersection.I, intersection.J] == State.Black)
			{
				this[intersection.I, intersection.J] = State.EmptyBlack;
				NbBlackStones--;
			}

			NbEmptyIntersections++;
		}

		public void UpdateEmptyIntersections()
		{
			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					if (utils.IsEmpty(i, j))
					{
						this[i, j] = State.Empty;
					}
				}
			}
		}

		public HashSet<Intersection> GetLegalIntersectionsWithNoLiberties(PlayerColor color) =>
			utils.GetLegalIntersectionsWithNoLiberties(color);

		public HashSet<Intersection> GetAdjacentIntersections(int i, int j) => utils.GetAdjacentIntersections(i, j);

		public HashSet<Intersection> GetAdjacentConnectedIntersections(int i, int j) =>
			utils.GetAdjacentConnectedIntersections(i, j);

		public HashSet<Intersection> GetConnectedIntersections(int i, int j) => utils.GetConnectedIntersections(i, j);

		public HashSet<Intersection> GetLegalIntersections(PlayerColor color) => utils.GetLegalIntersections(color);

		public HashSet<Intersection> GetChain(int i, int j) => utils.GetChain(i, j);

		public HashSet<HashSet<Intersection>> GetChains() => utils.GetChains();

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
