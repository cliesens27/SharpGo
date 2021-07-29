﻿using SharpGo.Game;
using SharpGo.Game.Players;

namespace SharpGo
{
	public class Program
	{
		private static GoGame game;

		public static void Main()
		{
			Player p1 = new RandomPlayer(Color.Black);
			Player p2 = new RandomPlayer(Color.White);

			game = new GoGame(p1, p2, 19);
			game.Start();
		}
	}
}
