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

		[TestMethod]
		public void Test_IsEmpty()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			Assert.IsTrue(utils.IsEmpty(0, 0));

			b.PlaceStone(PlayerColor.Black, 0, 0);
			Assert.IsFalse(utils.IsEmpty(0, 0));

			b.Capture(new Intersection(State.Black, 0, 0));
			Assert.IsTrue(utils.IsEmpty(0, 0));

			b.UpdateEmptyIntersections();
			Assert.IsTrue(utils.IsEmpty(0, 0));
		}

		[TestMethod]
		public void Test_AreAdjacent()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			Assert.IsTrue(utils.AreAdjacent(0, 0, 1, 0));
			Assert.IsTrue(utils.AreAdjacent(18, 18, 18, 17));
			Assert.IsFalse(utils.AreAdjacent(0, 0, 0, 18));
			Assert.IsFalse(utils.AreAdjacent(0, 0, 1, 1));
		}

		[TestMethod]
		public void Test_CanBeConnected()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);
			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 1, 0);

			b.PlaceStone(PlayerColor.Black, 18, 18);
			b.PlaceStone(PlayerColor.Black, 18, 17);

			b.PlaceStone(PlayerColor.White, 0, 18);
			b.PlaceStone(PlayerColor.White, 1, 1);

			Assert.IsTrue(utils.CanBeConnected(0, 0, 1, 0));
			Assert.IsTrue(utils.CanBeConnected(18, 18, 18, 17));
			Assert.IsFalse(utils.CanBeConnected(0, 0, 0, 18));
			Assert.IsFalse(utils.CanBeConnected(0, 0, 1, 1));
		}

		[TestMethod]
		public void Test_AreAdjacentConnected()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);
			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 1, 0);

			b.PlaceStone(PlayerColor.Black, 18, 18);
			b.PlaceStone(PlayerColor.Black, 18, 17);

			b.PlaceStone(PlayerColor.White, 1, 1);
			b.PlaceStone(PlayerColor.White, 0, 1);

			Assert.IsTrue(utils.AreAdjacentConnected(0, 0, 1, 0));
			Assert.IsTrue(utils.AreAdjacentConnected(18, 18, 18, 17));
			Assert.IsFalse(utils.AreAdjacentConnected(0, 0, 1, 1));
			Assert.IsFalse(utils.AreAdjacentConnected(0, 0, 0, 1));
		}

		[TestMethod]
		public void Test_CountIntersectionLiberties()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			b.PlaceStone(PlayerColor.Black, 0, 0);
			Assert.AreEqual(2, utils.CountIntersectionLiberties(0, 0));

			b.PlaceStone(PlayerColor.Black, 1, 1);
			b.PlaceStone(PlayerColor.Black, 0, 2);
			Assert.AreEqual(0, utils.CountIntersectionLiberties(0, 1));
			Assert.AreEqual(4, utils.CountIntersectionLiberties(1, 1));
			Assert.AreEqual(3, utils.CountIntersectionLiberties(0, 2));
		}

		[TestMethod]
		public void Test_GetAdjacentIntersections()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			Assert.AreEqual(2, utils.GetAdjacentIntersections(0, 0).Count);
			Assert.AreEqual(4, utils.GetAdjacentIntersections(1, 1).Count);
			Assert.AreEqual(3, utils.GetAdjacentIntersections(0, 2).Count);
			Assert.AreEqual(2, utils.GetAdjacentIntersections(18, 18).Count);
		}

		[TestMethod]
		public void Test_GetConnectedIntersections()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 0, 1);
			b.PlaceStone(PlayerColor.Black, 0, 2);
			b.PlaceStone(PlayerColor.Black, 0, 3);
			b.PlaceStone(PlayerColor.Black, 1, 3);
			b.PlaceStone(PlayerColor.Black, 2, 3);
			b.PlaceStone(PlayerColor.Black, 2, 2);
			b.PlaceStone(PlayerColor.Black, 2, 1);

			b.PlaceStone(PlayerColor.White, 1, 1);
			b.PlaceStone(PlayerColor.White, 1, 0);
			b.PlaceStone(PlayerColor.White, 2, 0);
			b.PlaceStone(PlayerColor.White, 3, 0);
			b.PlaceStone(PlayerColor.White, 4, 0);

			Assert.AreEqual(7, utils.GetConnectedIntersections(0, 0).Count);
			Assert.AreEqual(4, utils.GetConnectedIntersections(1, 1).Count);
			Assert.AreEqual(0, utils.GetConnectedIntersections(1, 2).Count);
			Assert.AreEqual(b.Size * b.Size - 15, utils.GetConnectedIntersections(0, 4).Count);
		}

		[TestMethod]
		public void Test_GetAdjacentConnectedIntersections()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 0, 1);
			b.PlaceStone(PlayerColor.Black, 0, 2);
			b.PlaceStone(PlayerColor.Black, 0, 3);
			b.PlaceStone(PlayerColor.Black, 1, 3);
			b.PlaceStone(PlayerColor.Black, 2, 3);
			b.PlaceStone(PlayerColor.Black, 2, 2);
			b.PlaceStone(PlayerColor.Black, 2, 1);

			b.PlaceStone(PlayerColor.White, 1, 1);
			b.PlaceStone(PlayerColor.White, 1, 0);
			b.PlaceStone(PlayerColor.White, 2, 0);
			b.PlaceStone(PlayerColor.White, 3, 0);
			b.PlaceStone(PlayerColor.White, 4, 0);

			Assert.AreEqual(1, utils.GetAdjacentConnectedIntersections(0, 0).Count);
			Assert.AreEqual(1, utils.GetAdjacentConnectedIntersections(1, 1).Count);
			Assert.AreEqual(0, utils.GetAdjacentConnectedIntersections(1, 2).Count);
			Assert.AreEqual(2, utils.GetAdjacentConnectedIntersections(0, 4).Count);
		}

		[TestMethod]
		public void Test_GetLegalIntersections()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 0, 1);
			b.PlaceStone(PlayerColor.Black, 0, 2);
			b.PlaceStone(PlayerColor.Black, 0, 3);
			b.PlaceStone(PlayerColor.Black, 1, 3);
			b.PlaceStone(PlayerColor.Black, 2, 3);
			b.PlaceStone(PlayerColor.Black, 2, 2);
			b.PlaceStone(PlayerColor.Black, 2, 1);

			b.PlaceStone(PlayerColor.White, 1, 1);
			b.PlaceStone(PlayerColor.White, 1, 0);
			b.PlaceStone(PlayerColor.White, 2, 0);
			b.PlaceStone(PlayerColor.White, 3, 0);
			b.PlaceStone(PlayerColor.White, 4, 0);

			Assert.AreEqual(b.Size * b.Size - 13, utils.GetLegalIntersections(PlayerColor.Black).Count);
		}

		[TestMethod]
		public void Test_GetCapturableIntersections()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 0, 1);
			b.PlaceStone(PlayerColor.Black, 0, 2);
			b.PlaceStone(PlayerColor.Black, 0, 3);
			b.PlaceStone(PlayerColor.Black, 1, 3);
			b.PlaceStone(PlayerColor.Black, 2, 3);
			b.PlaceStone(PlayerColor.Black, 2, 2);
			b.PlaceStone(PlayerColor.Black, 2, 1);
			b.PlaceStone(PlayerColor.Black, 4, 1);
			b.PlaceStone(PlayerColor.Black, 1, 1);

			b.PlaceStone(PlayerColor.White, 1, 0);
			b.PlaceStone(PlayerColor.White, 2, 0);
			b.PlaceStone(PlayerColor.White, 3, 0);
			b.PlaceStone(PlayerColor.White, 4, 0);
			b.PlaceStone(PlayerColor.White, 1, 2);
			b.PlaceStone(PlayerColor.White, 3, 1);
			b.PlaceStone(PlayerColor.White, 3, 2);
			b.PlaceStone(PlayerColor.White, 4, 2);
			b.PlaceStone(PlayerColor.White, 5, 1);

			Assert.AreEqual(1, utils.GetCapturableIntersections(PlayerColor.Black).Count);
			Assert.AreEqual(1, utils.GetCapturableIntersections(PlayerColor.White).Count);
		}

		[TestMethod]
		public void Test_GetChain()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 0, 1);
			b.PlaceStone(PlayerColor.Black, 0, 2);
			b.PlaceStone(PlayerColor.Black, 0, 3);
			b.PlaceStone(PlayerColor.Black, 1, 3);
			b.PlaceStone(PlayerColor.Black, 2, 3);
			b.PlaceStone(PlayerColor.Black, 2, 2);
			b.PlaceStone(PlayerColor.Black, 2, 1);
			b.PlaceStone(PlayerColor.Black, 4, 1);
			b.PlaceStone(PlayerColor.Black, 1, 1);

			b.PlaceStone(PlayerColor.White, 1, 0);
			b.PlaceStone(PlayerColor.White, 2, 0);
			b.PlaceStone(PlayerColor.White, 3, 0);
			b.PlaceStone(PlayerColor.White, 4, 0);
			b.PlaceStone(PlayerColor.White, 1, 2);
			b.PlaceStone(PlayerColor.White, 3, 1);
			b.PlaceStone(PlayerColor.White, 3, 2);
			b.PlaceStone(PlayerColor.White, 4, 2);
			b.PlaceStone(PlayerColor.White, 5, 1);

			Assert.AreEqual(7, utils.GetChain(1, 0).Count);
			Assert.AreEqual(9, utils.GetChain(0, 0).Count);
		}

		[TestMethod]
		public void Test_GetChains()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 0, 1);
			b.PlaceStone(PlayerColor.Black, 0, 2);
			b.PlaceStone(PlayerColor.Black, 0, 3);
			b.PlaceStone(PlayerColor.Black, 1, 3);
			b.PlaceStone(PlayerColor.Black, 2, 3);
			b.PlaceStone(PlayerColor.Black, 2, 2);
			b.PlaceStone(PlayerColor.Black, 2, 1);
			b.PlaceStone(PlayerColor.Black, 4, 1);
			b.PlaceStone(PlayerColor.Black, 1, 1);

			b.PlaceStone(PlayerColor.White, 1, 0);
			b.PlaceStone(PlayerColor.White, 2, 0);
			b.PlaceStone(PlayerColor.White, 3, 0);
			b.PlaceStone(PlayerColor.White, 4, 0);
			b.PlaceStone(PlayerColor.White, 1, 2);
			b.PlaceStone(PlayerColor.White, 3, 1);
			b.PlaceStone(PlayerColor.White, 3, 2);
			b.PlaceStone(PlayerColor.White, 4, 2);
			b.PlaceStone(PlayerColor.White, 5, 1);

			Assert.AreEqual(5, utils.GetChains().Count);
		}

		[TestMethod]
		public void Test_ChainsAndLiberties()
		{
			Board b = new Board();
			BoardUtils utils = new BoardUtils(b);

			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 0, 1);
			b.PlaceStone(PlayerColor.Black, 0, 2);
			b.PlaceStone(PlayerColor.Black, 0, 3);
			b.PlaceStone(PlayerColor.Black, 1, 3);
			b.PlaceStone(PlayerColor.Black, 2, 3);
			b.PlaceStone(PlayerColor.Black, 2, 2);
			b.PlaceStone(PlayerColor.Black, 2, 1);
			b.PlaceStone(PlayerColor.Black, 4, 1);
			b.PlaceStone(PlayerColor.Black, 1, 1);

			b.PlaceStone(PlayerColor.White, 1, 0);
			b.PlaceStone(PlayerColor.White, 2, 0);
			b.PlaceStone(PlayerColor.White, 3, 0);
			b.PlaceStone(PlayerColor.White, 4, 0);
			b.PlaceStone(PlayerColor.White, 1, 2);
			b.PlaceStone(PlayerColor.White, 3, 1);
			b.PlaceStone(PlayerColor.White, 3, 2);
			b.PlaceStone(PlayerColor.White, 4, 2);
			b.PlaceStone(PlayerColor.White, 5, 1);

			var chains = utils.GetChains();

			foreach (var chain in chains)
			{
				int nbLiberties = -1;

				foreach (var intersection in chain)
				{
					if (nbLiberties == -1)
					{
						nbLiberties = utils.CountIntersectionLiberties(intersection.I, intersection.J);
					}
					else
					{
						Assert.AreEqual(nbLiberties, utils.CountIntersectionLiberties(intersection.I, intersection.J));
					}
				}
			}

			Assert.AreEqual(5, chains.Count);
		}
	}
}
