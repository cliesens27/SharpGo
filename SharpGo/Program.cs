﻿using SharpGo.Game;
using SharpGo.Game.Players;

namespace SharpGo
{
	public class Program
	{
		private static GoGame game;

		public static void Main()
		{
			Player p1 = new RandomPlayer(PlayerColor.Black);
			Player p2 = new RandomPlayer(PlayerColor.White);

			game = new GoGame(p1, p2, 10);
			game.Start();
		}
	}
}
