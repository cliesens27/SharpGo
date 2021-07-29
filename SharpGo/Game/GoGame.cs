using System;
using DrawingBoardNET.Drawing;
using SharpGo.Game.Players;
using Source.Game;
using Source.Render;

namespace SharpGo.Game
{
	public class GoGame
	{
		private readonly DrawingBoard db;
		private readonly Renderer renderer;
		private readonly Player player1;
		private readonly Player player2;
		private readonly Board board;
		private bool update = true;
		private string errorMessage;

		public GoGame(Player p1, Player p2, int boardSize = 19)
		{
			renderer = new Renderer(out db);
			board = new Board(boardSize);
			player1 = p1;
			player2 = p2;
			db.Draw = Draw;
		}

		public void Start() => db.Start();

		private void Update()
		{
			if (update)
			{
				if (player1.Color == player2.Color)
				{
					throw new ArgumentException($"Cannot run game, both players are the same color.");
				}

				if (player1.Color == Color.Black)
				{
					player1.Play(board);
					player2.Play(board);
				}
				else
				{
					player2.Play(board);
					player1.Play(board);
				}
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

			db.TextColor(255, 0, 0);
			db.FontSize(25);
			db.Text(errorMessage, 0, 0);
		}
	}
}
