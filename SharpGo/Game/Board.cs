using System;
using System.Collections.Generic;
using SharpGo.Game;
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
		private readonly HashSet<Intersection> visited;

		public Board(int size)
		{
			NbRows = NbCols = size;
			NbIntersections = NbRows * NbCols;
			states = new State[NbRows][];
			visited = new HashSet<Intersection>();

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

		public (int nbWhite, int nbBlack, int nbEmpty) CountIntersections()
		{
			(int nbWhite, int nbBlack, int nbEmpty) = (0, 0, 0);

			for (int i = 0; i < NbRows; i++)
			{
				for (int j = 0; j < NbCols; j++)
				{
					if (this[i, j] == State.White)
					{
						nbWhite++;
					}
					else if (this[i, j] == State.Black)
					{
						nbBlack++;
					}
					else if (IsEmpty(i, j))
					{
						nbEmpty++;
					}
				}
			}

			return (nbWhite, nbBlack, nbEmpty);
		}

		public HashSet<Intersection> GetAdjacentIntersections(int i, int j)
		{
			HashSet<Intersection> intersections = new HashSet<Intersection>();

			if (IsInsideBoard(i + 1, j))
			{
				intersections.Add(new Intersection(this[i, j], i + 1, j));
			}

			if (IsInsideBoard(i - 1, j))
			{
				intersections.Add(new Intersection(this[i, j], i - 1, j));
			}

			if (IsInsideBoard(i, j + 1))
			{
				intersections.Add(new Intersection(this[i, j], i, j + 1));
			}

			if (IsInsideBoard(i, j - 1))
			{
				intersections.Add(new Intersection(this[i, j], i, j - 1));
			}

			return intersections;
		}

		public HashSet<Intersection> GetAdjacentConnectedIntersections(int i, int j)
		{
			HashSet<Intersection> intersections = GetAdjacentIntersections(i, j);
			HashSet<Intersection> toRemove = new HashSet<Intersection>();

			foreach (Intersection intersection in intersections)
			{
				(int row, int col) = (intersection.I, intersection.J);

				if (!AreAdjacentConnected(i, j, row, col))
				{
					toRemove.Add(intersection);
				}
			}

			intersections.ExceptWith(toRemove);
			return intersections;
		}

		public HashSet<Intersection> GetConnectedIntersections(int i, int j)
		{
			visited.Clear();
			HashSet<Intersection> connectedIntersections = _GetConnectedIntersections(i, j);
			connectedIntersections.Remove(new Intersection(this[i, j], i, j));
			return connectedIntersections;
		}

		public HashSet<Intersection> GetUnoccupiedValidIntersections(Color color)
		{
			HashSet<Intersection> intersections = GetValidIntersections(color);
			HashSet<Intersection> toRemove = new HashSet<Intersection>();

			foreach (Intersection intersection in intersections)
			{
				(int i, int j) = (intersection.I, intersection.J);

				if (IsOccupied(i, j))
				{
					toRemove.Add(intersection);
				}
			}

			intersections.ExceptWith(toRemove);
			return intersections;
		}

		public HashSet<Intersection> GetValidIntersections(Color color)
		{
			HashSet<Intersection> intersections = new HashSet<Intersection>();

			for (int i = 0; i < NbRows; i++)
			{
				for (int j = 0; j < NbCols; j++)
				{
					if (IsValidPosition(color, i, j))
					{
						intersections.Add(new Intersection(this[i, j], i, j));
					}
				}
			}

			return intersections;
		}

		public HashSet<HashSet<Intersection>> GetChains()
		{
			HashSet<HashSet<Intersection>> chains = new HashSet<HashSet<Intersection>>();

			for (int i = 0; i < NbRows; i++)
			{
				for (int j = 0; j < NbCols; j++)
				{
					HashSet<Intersection> chain = GetConnectedIntersections(i, j);
					chain.Add(new Intersection(this[i, j], i, j));
					chains.Add(chain);
				}
			}

			HashSet<HashSet<Intersection>> toRemove = new HashSet<HashSet<Intersection>>();

			foreach (HashSet<Intersection> s1 in chains)
			{
				foreach (HashSet<Intersection> s2 in chains)
				{
					if (s1 != s2 && s1.SetEquals(s2) && !(toRemove.Contains(s1) || toRemove.Contains(s2)))
					{
						toRemove.Add(s1);
					}
				}
			}

			chains.ExceptWith(toRemove);
			return chains;
		}

		private HashSet<Intersection> _GetConnectedIntersections(int i, int j)
		{
			HashSet<Intersection> connectedIntersections = GetAdjacentConnectedIntersections(i, j);
			HashSet<Intersection> toAdd = new HashSet<Intersection>();

			foreach (Intersection intersection in connectedIntersections)
			{
				if (visited.Add(intersection))
				{
					(int row, int col) = (intersection.I, intersection.J);
					HashSet<Intersection> intersections = _GetConnectedIntersections(row, col);
					toAdd.UnionWith(intersections);
				}
			}

			connectedIntersections.UnionWith(toAdd);
			return connectedIntersections;
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

		private bool AreAdjacentConnected(int i1, int j1, int i2, int j2) =>
			CanBeConnected(i1, j1, i2, j2) && AreAdjacent(i1, j1, i2, j2);

		private bool CanBeConnected(int i1, int j1, int i2, int j2) =>
			this[i1, j1] == this[i2, j2] || IsEmpty(i1, j1) && IsEmpty(i2, j2);

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
