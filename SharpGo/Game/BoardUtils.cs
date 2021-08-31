using System.Collections.Generic;
using System.Linq;
using SharpGo.Game.Players;

namespace SharpGo.Game
{
	public class BoardUtils
	{
		private readonly HashSet<Intersection> visited = new HashSet<Intersection>();
		private readonly Board board;

		public BoardUtils(Board board) => this.board = board;

		public bool IsOccupied(int i, int j) => board[i, j].State == State.White || board[i, j].State == State.Black;

		public bool IsInsideBoard(int i, int j) => (i >= 0 && i < board.Size) && (j >= 0 && j < board.Size);

		public bool IsLegal(PlayerColor color, int i, int j) => !IsSuicide(color, i, j) && CanPlaceStone(color, i, j);

		public bool IsSuicide(PlayerColor color, int i, int j)
		{
			int nbLiberties;

			if (IsEmpty(i, j))
			{
				State previousState = board[i, j].State;

				board.PlaceTempStone(color, i, j);
				nbLiberties = CountIntersectionLiberties(i, j);
				board.RemoveTempStone(previousState, i, j);
			}
			else
			{
				nbLiberties = CountIntersectionLiberties(i, j);
			}

			return nbLiberties == 0;
		}

		public bool IsEmpty(int i, int j) => !IsOccupied(i, j);

		public bool AreAdjacent(int i1, int j1, int i2, int j2) =>
			(i1 == i2 && (j1 == j2 + 1 || j1 == j2 - 1)) ||
			(j1 == j2 && (i1 == i2 + 1 || i1 == i2 - 1));

		public bool CanBeConnected(int i1, int j1, int i2, int j2) =>
			board[i1, j1].State == board[i2, j2].State ||
			(IsEmpty(i1, j1) && IsEmpty(i2, j2));

		public bool AreAdjacentConnected(int i1, int j1, int i2, int j2) =>
			AreAdjacent(i1, j1, i2, j2) &&
			CanBeConnected(i1, j1, i2, j2);

		public int CountIntersectionLiberties(int i, int j)
		{
			if (IsEmpty(i, j))
			{
				return 0;
			}

			HashSet<Intersection> chain = GetChain(i, j);
			var visited = new HashSet<Intersection> { board[i, j] };
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

		public int ComputeTerritory(PlayerColor color)
		{
			HashSet<HashSet<Intersection>> emptyChains = GetEmptyChains();
			int territory = 0;

			foreach (var chain in emptyChains)
			{
				var adj = new HashSet<Intersection>();

				foreach (var intersection in chain)
				{
					adj = SmartUnion(adj, board.Utils.GetAdjacentIntersections(intersection.I, intersection.J));
				}

				adj.RemoveWhere(x => IsEmpty(x.I, x.J));

				bool bordersWhite = false;
				bool bordersBlack = false;

				foreach (var intersection in adj)
				{
					if (intersection.State == State.White)
					{
						bordersWhite = true;
					}
					else if (intersection.State == State.Black)
					{
						bordersBlack = true;
					}
				}

				if (color == PlayerColor.White && (bordersWhite && !bordersBlack))
				{
					territory += chain.Count;
				}
				else if (color == PlayerColor.Black && (!bordersWhite && bordersBlack))
				{
					territory += chain.Count;
				}
			}

			return territory;
		}

		public int ComputeScore(PlayerColor color)
		{
			if (board.IsEmpty)
			{
				return 0;
			}

			int score = (color == PlayerColor.Black) ? board.NbBlackStones : board.NbWhiteStones;

			if (score <= 1)
			{
				return score;
			}

			return score + ComputeTerritory(color);
		}

		public HashSet<Intersection> GetAdjacentIntersections(int i, int j)
		{
			var intersections = new HashSet<Intersection>();

			if (IsInsideBoard(i + 1, j))
			{
				intersections.Add(board[i + 1, j]);
			}

			if (IsInsideBoard(i - 1, j))
			{
				intersections.Add(board[i - 1, j]);
			}

			if (IsInsideBoard(i, j + 1))
			{
				intersections.Add(board[i, j + 1]);
			}

			if (IsInsideBoard(i, j - 1))
			{
				intersections.Add(board[i, j - 1]);
			}

			return intersections;
		}

		public HashSet<Intersection> GetConnectedIntersections(int i, int j)
		{
			visited.Clear();
			HashSet<Intersection> connectedIntersections = GetConnectedIntersectionsAux(i, j);
			connectedIntersections.Remove(board[i, j]);
			return connectedIntersections;
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

		public HashSet<Intersection> GetLegalIntersections(PlayerColor color)
		{
			var intersections = new HashSet<Intersection>();

			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
				{
					if (IsLegal(color, i, j))
					{
						intersections.Add(board[i, j]);
					}
				}
			}

			return intersections;
		}

		public HashSet<Intersection> GetCapturableIntersections(PlayerColor color)
		{
			HashSet<Intersection> intersections = new HashSet<Intersection>();

			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
				{
					if (IsOccupied(i, j) && board[i, j].State != Player.PlayerColorToState(color))
					{
						int nbLiberties = CountIntersectionLiberties(i, j);

						if (nbLiberties == 0)
						{
							intersections.Add(board[i, j]);
						}
					}
				}
			}

			return intersections;
		}

		public HashSet<Intersection> GetChain(int i, int j, bool emptyIntersections = false)
		{
			if ((!emptyIntersections && IsEmpty(i, j)) ||
				(emptyIntersections && IsOccupied(i, j)))
			{
				return new HashSet<Intersection>();
			}

			HashSet<Intersection> chain = GetConnectedIntersections(i, j);
			chain.Add(board[i, j]);
			return chain;
		}

		public HashSet<HashSet<Intersection>> GetChains()
		{
			var chains = new HashSet<HashSet<Intersection>>();

			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
				{
					if (IsEmpty(i, j))
					{
						continue;
					}

					HashSet<Intersection> chain = GetConnectedIntersections(i, j);
					chain.Add(board[i, j]);
					chains.Add(chain);
				}
			}

			var toRemove = new HashSet<HashSet<Intersection>>();
			HashSet<Intersection>[] arr = chains.ToArray();

			for (int i = 0; i < arr.Length; i++)
			{
				HashSet<Intersection> chain1 = arr[i];

				for (int j = i; j < arr.Length; j++)
				{
					HashSet<Intersection> chain2 = arr[j];

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

		public HashSet<HashSet<Intersection>> GetEmptyChains()
		{
			var emptyIntersections = new HashSet<Intersection>();

			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
				{
					if (IsEmpty(i, j))
					{
						emptyIntersections.Add(board[i, j]);
					}
				}
			}

			var emptyChains = new HashSet<HashSet<Intersection>>();

			foreach (var intersection in emptyIntersections)
			{
				bool alreadyPresent = false;

				foreach (var chain in emptyChains)
				{
					if (chain.Contains(intersection))
					{
						alreadyPresent = true;
						break;
					}
				}

				if (!alreadyPresent)
				{
					emptyChains.Add(GetChain(intersection.I, intersection.J, emptyIntersections: true));
				}
			}

			return emptyChains;
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

					toAdd = SmartUnion(toAdd, intersections);
				}
			}

			return SmartUnion(connectedIntersections, toAdd);
		}

		private HashSet<Intersection> SmartUnion(HashSet<Intersection> a, HashSet<Intersection> b)
		{
			if (a.Count > b.Count)
			{
				a.UnionWith(b);
				return a;
			}
			else
			{
				b.UnionWith(a);
				return b;
			}
		}

		private bool CanPlaceStone(PlayerColor color, int i, int j) =>
			(color == PlayerColor.White && (board[i, j].State == State.Empty || board[i, j].State == State.EmptyBlack)) ||
			(color == PlayerColor.Black && (board[i, j].State == State.Empty || board[i, j].State == State.EmptyWhite));
	}
}
