using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpGo.Game.Players;
using SharpGo.Game;
using MathlibNET.Random;

namespace SharpGo.Tests
{
	[TestClass]
	public class PerfTests
	{
		[TestMethod]
		public void Test_RandomNoPassPlayer()
		{
			Player p1 = new RandomNoPassPlayer(PlayerColor.Black);
			Player p2 = new RandomNoPassPlayer(PlayerColor.White);
			Rng.Seed = 1000000000;
			GoGame game = new GoGame(p1, p2, boardSize: 8, closeOnGameEnd: true, render: true, frameRate: 999);
			game.Start();
		}
	}
}
