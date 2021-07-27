using System;
using DrawingBoardNET.Drawing;
using SharpGo.Game.Players;
using Source.Game;
using Source.Render;

namespace SharpGo
{
	public class GoGame
	{
		private static DrawingBoard db;
		private static Renderer renderer;
		private static Player player1;
		private static Player player2;
		private static Board board;
		private static bool update = true;
		private static string errorMessage;

		public static void Main(string[] args) => Run();

		private static void Run()
		{
			renderer = new Renderer(out db);
			player1 = new RandomPlayer(Color.Black);
			player2 = new RandomPlayer(Color.White);
			board = new Board(19);

			db.Draw = Draw;

			db.Start();
		}

		private static void Update()
		{
			if (db.FrameCount < 999999999 && update)
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

		private static void Draw()
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
