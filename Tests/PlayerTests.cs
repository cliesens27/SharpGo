using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpGo.Game.Players;
using SharpGo.Game;

namespace SharpGo.Tests
{
	[TestClass]
	public class PlayerTests
	{
		[TestMethod]
		public void TestProperties()
		{
			Board b = new();
			Player p = new RandomNoPassPlayer(PlayerColor.Black);

			Assert.AreEqual(0, p.NbTurnsPlayed);

			for (int i = 0; i < b.Size / 2; i++)
			{
				p.Play(b);
				Assert.AreEqual(i + 1, p.NbTurnsPlayed);
			}
		}
	}
}
