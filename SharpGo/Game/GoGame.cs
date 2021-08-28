using System;
using DrawingBoardNET.Drawing;
using SharpGo.Game.Players;
using SharpGo.Render;

namespace SharpGo.Game
{
	public class GoGame
	{
		public int NbTurns { get; private set; } = 0;
		public bool GameHasEnded { get; private set; } = false;
		public Player Player1 { get; }
		public Player Player2 { get; }
		public Board Board { get; }

		private readonly DrawingBoard db;
		private readonly Renderer renderer;
		private string errorMessage;
		private int currentPlayer;

		public GoGame(Player p1, Player p2, int frameRate, int boardSize = 19)
		{
			renderer = new Renderer(out db, frameRate);
			Board = new Board(boardSize);
			Player1 = p1;
			Player2 = p2;
			db.Draw = Draw;

			if (Player1.Color == Player2.Color)
			{
				throw new ArgumentException($"Cannot run game, both players are the same color.");
			}

			if (Player1.Color == PlayerColor.Black)
			{
				currentPlayer = 1;
			}
			else
			{
				currentPlayer = 2;
			}
		}

		public void Start() => db.Start();

		private void Update()
		{
			if (!GameHasEnded)
			{
				if (currentPlayer == 1)
				{
					Player1.Play(Board);
					currentPlayer = 2;
				}
				else if (currentPlayer == 2)
				{
					Player2.Play(Board);
					currentPlayer = 1;
				}

				if (Player1.NbTurnsPlayed == Player2.NbTurnsPlayed)
				{
					Board.UpdateEmptyIntersections();
				}

				if (Player1.HasPassed && Player1.HasPassed)
				{
					GameHasEnded = true;
				}

				NbTurns++;
			}
		}

		private void Draw()
		{
			try
			{
				Update();
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
				GameHasEnded = true;
			}

			renderer.Render(this);

			DrawErrorMessage(errorMessage);
		}

		private void DrawErrorMessage(string errorMessage)
		{
			if (!string.IsNullOrWhiteSpace(errorMessage))
			{
				db.TextColor(255, 0, 0);
				db.FontSize(25);
				db.Text(errorMessage, 0, 0);
			}
		}
	}
}
