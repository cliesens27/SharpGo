using System;
using System.Collections.Generic;
using SharpGo.Game;
using SharpGo.Game.Players;

namespace Source.Game
{
	internal class Board
	{
		internal State this[int i, int j] => states[i][j];
		internal int NbRows { get; }
		internal int NbCols { get; }
		internal int NbIntersections { get; }

		private readonly State[][] states;
		private readonly BoardUtils utils;

		internal Board(int size)
		{
			utils = new BoardUtils(this);

			NbRows = NbCols = size;
			NbIntersections = NbRows * NbCols;
			states = new State[NbRows][];

			for (int i = 0; i < NbRows; i++)
			{
				states[i] = new State[NbCols];
			}

			Reset();
		}

		internal void PlaceStone(PlayerColor color, int i, int j)
		{
			if (utils.IsOccupied(i, j))
			{
				throw new InvalidOperationException($"Cannot place stone on intersection ({i},{j}), it is already occupied.");
			}

			if (!utils.IsInsideBoard(i, j))
			{
				throw new IndexOutOfRangeException($"Cannot place stone on intersection ({i},{j}), it is outside the board.");
			}

			states[i][j] = Player.PlayerColorToState(color);
		}

		internal (int nbWhite, int nbBlack, int nbEmpty) CountIntersections() => utils.CountIntersections();

		internal HashSet<Intersection> GetAdjacentIntersections(int i, int j) => utils.GetAdjacentIntersections(i, j);

		internal HashSet<Intersection> GetAdjacentConnectedIntersections(int i, int j) =>
			utils.GetAdjacentConnectedIntersections(i, j);

		internal HashSet<Intersection> GetConnectedIntersections(int i, int j) => utils.GetConnectedIntersections(i, j);

		internal HashSet<Intersection> GetUnoccupiedLegalIntersections(PlayerColor color) =>
			utils.GetUnoccupiedLegalIntersections(color);

		internal HashSet<Intersection> GetLegalIntersections(PlayerColor color) => utils.GetLegalIntersections(color);

		internal HashSet<Intersection> GetChain(int i, int j) => utils.GetChain(i, j);

		internal HashSet<HashSet<Intersection>> GetChains() => utils.GetChains();

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
