using System;
using DrawingBoardNET.Drawing;
using SharpGo.Game.Players;
using Source.Game;
using Source.Render;

namespace SharpGo.Game
{
	public class GoGame
	{
		public int NbTurns { get; private set; }

		private readonly DrawingBoard db;
		private readonly Renderer renderer;
		private readonly Player player1;
		private readonly Player player2;
		private readonly Board board;
		private string errorMessage;
		private bool update = true;
		private int currentPlayer;

		public GoGame(Player p1, Player p2, int boardSize = 19)
		{
			renderer = new Renderer(out db);
			board = new Board(boardSize);
			player1 = p1;
			player2 = p2;
			db.Draw = Draw;

			if (player1.Color == player2.Color)
			{
				throw new ArgumentException($"Cannot run game, both players are the same color.");
			}

			if (player1.Color == PlayerColor.Black)
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
			if (update)
			{
				if (currentPlayer == 1)
				{
					player1.Play(board);
					currentPlayer = 2;
				}
				else
				{
					player2.Play(board);
					currentPlayer = 1;
				}

				if (player1.NbTurnsPlayed == player2.NbTurnsPlayed)
				{
					board.UpdateEmptyIntersections();
				}

				if (player1.Passed && player1.Passed)
				{
					update = false;
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
				update = false;
			}

			renderer.Render(board);

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
