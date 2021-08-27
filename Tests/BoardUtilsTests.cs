using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpGo.Game;
using SharpGo.Game.Players;

namespace SharpGo.Tests
{
	[TestClass]
	public class BoardUtilsTests
	{
		[TestMethod]
		public void Test_IsOccupied()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			Assert.IsFalse(utils.IsOccupied(0, 0));

			b.PlaceStone(PlayerColor.Black, 0, 0);
			Assert.IsTrue(utils.IsOccupied(0, 0));

			b.Capture(new Intersection(State.Black, 0, 0));
			Assert.IsFalse(utils.IsOccupied(0, 0));
		}

		[TestMethod]
		public void Test_IsInsideBoard()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			Assert.IsTrue(utils.IsInsideBoard(0, 0));
			Assert.IsTrue(utils.IsInsideBoard(5, 5));
			Assert.IsTrue(utils.IsInsideBoard(18, 18));

			Assert.IsFalse(utils.IsInsideBoard(-1, 0));
			Assert.IsFalse(utils.IsInsideBoard(-1, -1));
			Assert.IsFalse(utils.IsInsideBoard(19, 19));
		}

		[TestMethod]
		public void Test_IsLegal()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			Assert.IsTrue(utils.IsLegal(PlayerColor.Black, 0, 0));

			b.PlaceStone(PlayerColor.Black, 0, 0);
			Assert.IsFalse(utils.IsLegal(PlayerColor.Black, 0, 0));

			b.Capture(new Intersection(State.Black, 0, 0));
			Assert.IsFalse(utils.IsLegal(PlayerColor.Black, 0, 0));

			b.UpdateEmptyIntersections();
			Assert.IsTrue(utils.IsLegal(PlayerColor.Black, 0, 0));
		}
	}
}
