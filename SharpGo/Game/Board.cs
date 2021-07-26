using System;
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
		public int NbIntersections { get; }

		private readonly State[][] states;

		public Board(int size)
		{
			NbRows = NbCols = size;
			NbIntersections = NbRows * NbCols;
			states = new State[NbRows][];

			for (int i = 0; i < NbRows; i++)
			{
				states[i] = new State[NbCols];
			}

			Reset();
		}

		public void PlaceStone(Color color, int i, int j)
		{
			if (IsOccupied(i, j))
			{
				throw new InvalidOperationException($"Cannot place stone on intersection ({i},{j}), it is already occupied.");
			}

			if (!IsInsideBoard(i, j))
			{
				throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
			}

			states[i][j] = Player.PlayerColorToState(color);
		}

		public HashSet<(State, int, int)> GetAdjacentIntersections(int i, int j)
		{
			HashSet<(State, int, int)> intersections = new HashSet<(State, int, int)>();

			if (IsInsideBoard(i + 1, j))
			{
				intersections.Add((this[i, j], i + 1, j));
			}

			if (IsInsideBoard(i - 1, j))
			{
				intersections.Add((this[i, j], i - 1, j));
			}

			if (IsInsideBoard(i, j + 1))
			{
				intersections.Add((this[i, j], i, j + 1));
			}

			if (IsInsideBoard(i, j - 1))
			{
				intersections.Add((this[i, j], i, j - 1));
			}

			return intersections;
		}

		public HashSet<(State, int, int)> GetAdjacentConnectedIntersections(int i, int j)
		{
			HashSet<(State, int, int)> intersections = GetAdjacentIntersections(i, j);
			HashSet<(State, int, int)> toRemove = new HashSet<(State, int, int)>();

			foreach ((State, int, int) intersection in intersections)
			{
				(_, int row, int col) = intersection;

				if (!AreAdjacentConnected(i, j, row, col))
				{
					toRemove.Add(intersection);
				}
			}

			intersections.Except(toRemove);
			return intersections;
		}

		public HashSet<(State, int, int)> GetConnectedIntersections(int i, int j, HashSet<(State, int, int)> set=null)
		{
			HashSet<(State, int, int)> intersections = GetAdjacentConnectedIntersections(i, j);
			HashSet<(State, int, int)> toAdd = new HashSet<(State, int, int)>();

			foreach ((State, int, int) intersection in intersections)
			{
				(_, int row, int col) = intersection;
				// TODO: needs fixing!
				HashSet<(State, int, int)> connectedIntersections = GetConnectedIntersections(row, col,intersections);
				toAdd.Union(connectedIntersections);
			}

			intersections.Union(toAdd);
			intersections.Remove((this[i, j], i, j));
			return intersections;
		}

		public HashSet<(State, int, int)> GetUnoccupiedValidIntersections(Color color)
		{
			HashSet<(State, int, int)> intersections = GetValidIntersections(color);
			HashSet<(State, int, int)> toRemove = new HashSet<(State, int, int)>();

			foreach ((State, int, int) intersection in intersections)
			{
				(_, int i, int j) = intersection;

				if (IsOccupied(i, j))
				{
					toRemove.Add(intersection);
				}
			}

			intersections.Except(toRemove);
			return intersections;
		}

		public HashSet<(State, int, int)> GetValidIntersections(Color color)
		{
			HashSet<(State, int, int)> intersections = new HashSet<(State, int, int)>();

			for (int i = 0; i < NbRows; i++)
			{
				for (int j = 0; j < NbCols; j++)
				{
					if (IsValidPosition(color, i, j))
					{
						intersections.Add((this[i, j], i, j));
					}
				}
			}

			return intersections;
		}

		private bool IsValidPosition(Color color, int i, int j) =>
			(color == Color.White && (this[i, j] == State.Empty || this[i, j] == State.EmptyBlack)) ||
			(color == Color.Black && (this[i, j] == State.Empty || this[i, j] == State.EmptyWhite));

		private bool IsOccupied(int i, int j) => this[i, j] == State.White || this[i, j] == State.Black;

		private bool IsEmpty(int i, int j) => !IsOccupied(i, j);

		private bool AreAdjacent(int i1, int j1, int i2, int j2) =>
			(i1 == i2 && j1 == j2 + 1) ||
			(i1 == i2 && j1 == j2 - 1) ||
			(i1 == i2 + 1 && j1 == j2) ||
			(i1 == i2 - 1 && j1 == j2);

		private bool AreAdjacentConnected(int i1, int j1, int i2, int j2)
		{
			if (this[i1, j1] == this[i2, j2] || IsEmpty(i1, j1) && IsEmpty(i2, j2))
			{
				return AreAdjacent(i1, j1, i2, j2);
			}
			else
			{
				return false;
			}
		}

		private bool IsInsideBoard(int i, int j) => i >= 0 && i < NbRows && j >= 0 && j < NbCols;

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
