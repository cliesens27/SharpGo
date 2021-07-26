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

		public static void Main(string[] args) => Run();

		private static void Run()
		{
			board = new Board(19);
			renderer = new Renderer(out db, Draw);

			player1 = new RandomPlayer(State.Black);
			player2 = new RandomPlayer(State.White);

			db.Start();
		}

		private static void Update()
		{
			if (player1.Color == player2.Color)
			{
				throw new ArgumentException($"Cannot run game, both players are the same color.");
			}

			if (player1.Color == State.Black)
			{
				player1.MakeMove(board);
				player2.MakeMove(board);
			}
			else
			{
				player2.MakeMove(board);
				player1.MakeMove(board);
			}
		}

		private static void Draw()
		{
			Update();
			renderer.Render(board);
		}
	}
}
