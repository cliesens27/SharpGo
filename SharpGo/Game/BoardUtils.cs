using System.Collections.Generic;
using SharpGo.Game.Players;
using Source.Game;

namespace SharpGo.Game
{
	internal class BoardUtils
	{
		private readonly HashSet<Intersection> visited = new HashSet<Intersection>();
		private readonly Board board;

		internal BoardUtils(Board board) => this.board = board;

		internal bool IsOccupied(int i, int j) => board[i, j] == State.White || board[i, j] == State.Black;

		internal bool IsInsideBoard(int i, int j) =>
			i >= 0 && i < board.NbRows && j >= 0 && j < board.NbCols;

		internal bool IsLegal(PlayerColor color, int i, int j) =>
			(color == PlayerColor.White && (board[i, j] == State.Empty || board[i, j] == State.EmptyBlack)) ||
			(color == PlayerColor.Black && (board[i, j] == State.Empty || board[i, j] == State.EmptyWhite));

		internal bool IsEmpty(int i, int j) => !IsOccupied(i, j);

		internal bool AreAdjacent(int i1, int j1, int i2, int j2) =>
			(i1 == i2 && j1 == j2 + 1) ||
			(i1 == i2 && j1 == j2 - 1) ||
			(i1 == i2 + 1 && j1 == j2) ||
			(i1 == i2 - 1 && j1 == j2);

		internal bool AreAdjacentConnected(int i1, int j1, int i2, int j2) =>
			AreAdjacent(i1, j1, i2, j2) && CanBeConnected(i1, j1, i2, j2);

		internal bool CanBeConnected(int i1, int j1, int i2, int j2) =>
			board[i1, j1] == board[i2, j2] || IsEmpty(i1, j1) && IsEmpty(i2, j2);

		internal (int nbWhite, int nbBlack, int nbEmpty) CountIntersections()
		{
			(int nbWhite, int nbBlack, int nbEmpty) = (0, 0, 0);

			for (int i = 0; i < board.NbRows; i++)
			{
				for (int j = 0; j < board.NbCols; j++)
				{
					if (board[i, j] == State.White)
					{
						nbWhite++;
					}
					else if (board[i, j] == State.Black)
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

		internal HashSet<Intersection> GetAdjacentIntersections(int i, int j)
		{
			var intersections = new HashSet<Intersection>();

			if (IsInsideBoard(i + 1, j))
			{
				intersections.Add(new Intersection(board[i, j], i + 1, j));
			}

			if (IsInsideBoard(i - 1, j))
			{
				intersections.Add(new Intersection(board[i, j], i - 1, j));
			}

			if (IsInsideBoard(i, j + 1))
			{
				intersections.Add(new Intersection(board[i, j], i, j + 1));
			}

			if (IsInsideBoard(i, j - 1))
			{
				intersections.Add(new Intersection(board[i, j], i, j - 1));
			}

			return intersections;
		}

		internal HashSet<Intersection> GetAdjacentConnectedIntersections(int i, int j)
		{
			HashSet<Intersection> intersections = GetAdjacentIntersections(i, j);
			var toRemove = new HashSet<Intersection>();

			foreach (var intersection in intersections)
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

		internal HashSet<Intersection> GetConnectedIntersections(int i, int j)
		{
			visited.Clear();
			HashSet<Intersection> connectedIntersections = GetConnectedIntersectionsAux(i, j);
			connectedIntersections.Remove(new Intersection(board[i, j], i, j));
			return connectedIntersections;
		}

		internal HashSet<Intersection> GetUnoccupiedLegalIntersections(PlayerColor color)
		{
			HashSet<Intersection> intersections = GetLegalIntersections(color);
			var toRemove = new HashSet<Intersection>();

			foreach (var intersection in intersections)
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

		internal HashSet<Intersection> GetLegalIntersections(PlayerColor color)
		{
			var intersections = new HashSet<Intersection>();

			for (int i = 0; i < board.NbRows; i++)
			{
				for (int j = 0; j < board.NbCols; j++)
				{
					if (IsLegal(color, i, j))
					{
						intersections.Add(new Intersection(board[i, j], i, j));
					}
				}
			}

			return intersections;
		}

		internal HashSet<Intersection> GetChain(int i, int j)
		{
			HashSet<HashSet<Intersection>> chains = GetChains();

			foreach (var chain in chains)
			{
				foreach (var intersection in chain)
				{
					if (intersection.I == i && intersection.J == j)
					{
						return chain;
					}
				}
			}

			return new HashSet<Intersection>();
		}

		internal HashSet<HashSet<Intersection>> GetChains()
		{
			var chains = new HashSet<HashSet<Intersection>>();

			for (int i = 0; i < board.NbRows; i++)
			{
				for (int j = 0; j < board.NbCols; j++)
				{
					HashSet<Intersection> chain = GetConnectedIntersections(i, j);
					chain.Add(new Intersection(board[i, j], i, j));
					chains.Add(chain);
				}
			}

			var toRemove = new HashSet<HashSet<Intersection>>();

			foreach (var chain1 in chains)
			{
				foreach (var chain2 in chains)
				{
					if (chain1 != chain2 && chain1.SetEquals(chain2) &&
						!(toRemove.Contains(chain1) || toRemove.Contains(chain2)))
					{
						toRemove.Add(chain1);
					}
				}
			}

			chains.ExceptWith(toRemove);
			return chains;
		}

		private HashSet<Intersection> GetConnectedIntersectionsAux(int i, int j)
		{
			HashSet<Intersection> connectedIntersections = GetAdjacentConnectedIntersections(i, j);
			var toAdd = new HashSet<Intersection>();

			foreach (var intersection in connectedIntersections)
			{
				if (visited.Add(intersection))
				{
					(int row, int col) = (intersection.I, intersection.J);
					HashSet<Intersection> intersections = GetConnectedIntersectionsAux(row, col);
					toAdd.UnionWith(intersections);
				}
			}

			connectedIntersections.UnionWith(toAdd);
			return connectedIntersections;
		}
	}
}
