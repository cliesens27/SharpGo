﻿using System;
using System.Collections.Generic;
using Source.Game;

namespace SharpGo.Game.Players
{
	public abstract class Player
	{
		public Color Color { get; }
		public bool Passed { get; private set; }
		public int NbTurnsPlayed{ get; private set; }

		protected Player(Color color) => Color = color;

		protected void PlaceStone(Board board, int i, int j) => board.PlaceStone(Color, i, j);

		protected abstract bool Pass(Board board, HashSet<Intersection> validIntersections);

		protected abstract (int, int) PickPosition(Board board, HashSet<Intersection> validIntersections);

		public void Play(Board board)
		{
			HashSet<Intersection> validIntersections = board.GetUnoccupiedValidIntersections(Color);

			if (Pass(board, validIntersections) || validIntersections.Count == 0)
			{
				Passed = true;
				return;
			}

			(int i, int j) = PickPosition(board, validIntersections);

			PlaceStone(board, i, j);
			// Capture
			// Self-capture

			Passed = false;
			NbTurnsPlayed++;
		}

		public static State PlayerColorToState(Color color)
		{
			if (color == Color.White)
			{
				return State.White;
			}
			else if (color == Color.Black)
			{
				return State.Black;
			}
			else
			{
				throw new ArgumentException("Bad player color.");
			}
		}
	}
}
