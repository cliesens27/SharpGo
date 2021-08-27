using System.Collections.Generic;
using SharpGo.Game.Players;

namespace SharpGo.Game
{
	public class BoardUtils
	{
		private readonly HashSet<Intersection> visited = new HashSet<Intersection>();
		private readonly Board board;

		public BoardUtils(Board board) => this.board = board;

		public bool IsOccupied(int i, int j) => board[i, j] == State.White || board[i, j] == State.Black;

		public bool IsInsideBoard(int i, int j) => (i >= 0 && i < board.Size) && (j >= 0 && j < board.Size);

		public bool IsLegal(PlayerColor color, int i, int j) =>
			color == PlayerColor.White && (board[i, j] == State.Empty || board[i, j] == State.EmptyBlack) ||
			color == PlayerColor.Black && (board[i, j] == State.Empty || board[i, j] == State.EmptyWhite);

		public bool IsEmpty(int i, int j) => !IsOccupied(i, j);

		public bool AreAdjacent(int i1, int j1, int i2, int j2) =>
			(i1 == i2 && (j1 == j2 + 1 || j1 == j2 - 1)) ||
			(j1 == j2 && (i1 == i2 + 1 || i1 == i2 - 1));

		public bool AreAdjacentConnected(int i1, int j1, int i2, int j2) =>
			AreAdjacent(i1, j1, i2, j2) &&
			CanBeConnected(i1, j1, i2, j2);

		public bool CanBeConnected(int i1, int j1, int i2, int j2) =>
			board[i1, j1] == board[i2, j2] ||
			(IsEmpty(i1, j1) && IsEmpty(i2, j2));

		public int CountIntersectionLiberties(int i, int j)
		{
			if (IsEmpty(i, j))
			{
				return 0;
			}

			HashSet<Intersection> chain = GetChain(i, j);
			var visited = new HashSet<Intersection> { new Intersection(board[i, j], i, j) };
			int nbLiberties = 0;

			foreach (var intersection in chain)
			{
				var adjacentIntersections = GetAdjacentIntersections(intersection.I, intersection.J);

				foreach (var adj in adjacentIntersections)
				{
					if (!visited.Contains(adj))
					{
						if (IsEmpty(adj.I, adj.J))
						{
							nbLiberties++;
						}

						visited.Add(adj);
					}
				}
			}

			return nbLiberties;
		}

		public HashSet<Intersection> GetLegalIntersectionsWithNoLiberties(PlayerColor color)
		{
			HashSet<Intersection> intersections = new HashSet<Intersection>();

			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
				{
					if (IsOccupied(i, j) && board[i, j] != Player.PlayerColorToState(color))
					{
						int nbLiberties = CountIntersectionLiberties(i, j);

						if (nbLiberties == 0)
						{
							intersections.Add(new Intersection(board[i, j], i, j));
						}
					}
				}
			}

			return intersections;
		}

		public HashSet<Intersection> GetAdjacentIntersections(int i, int j)
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

		public HashSet<Intersection> GetAdjacentConnectedIntersections(int i, int j)
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

		public HashSet<Intersection> GetConnectedIntersections(int i, int j)
		{
			visited.Clear();
			HashSet<Intersection> connectedIntersections = GetConnectedIntersectionsAux(i, j);
			connectedIntersections.Remove(new Intersection(board[i, j], i, j));
			return connectedIntersections;
		}

		public HashSet<Intersection> GetLegalIntersections(PlayerColor color)
		{
			var intersections = new HashSet<Intersection>();

			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
				{
					if (IsLegal(color, i, j))
					{
						intersections.Add(new Intersection(board[i, j], i, j));
					}
				}
			}

			return intersections;
		}

		public HashSet<Intersection> GetChain(int i, int j)
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

		public HashSet<HashSet<Intersection>> GetChains()
		{
			var chains = new HashSet<HashSet<Intersection>>();

			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
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
