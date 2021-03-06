using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpGo.Game.Players;
using SharpGo.Game;
using System;

namespace SharpGo.Tests
{
	[TestClass]
	public class BoardTests
	{
		[TestMethod]
		public void Test_Properties()
		{
			int size = 19;
			Board b = new();
			Assert.IsTrue(b.IsEmpty);
			Assert.AreEqual(0, b.NbBlackStones);
			Assert.AreEqual(0, b.NbWhiteStones);
			Assert.AreEqual(size, b.Size);
			Assert.AreEqual(size * size, b.NbEmptyIntersections);

			for (int i = 1; i < b.Size; i++)
			{
				b.PlaceStone(PlayerColor.White, i, 0);
				b.PlaceStone(PlayerColor.Black, 0, i);

				Assert.IsFalse(b.IsEmpty);
				Assert.AreEqual(i, b.NbBlackStones);
				Assert.AreEqual(i, b.NbWhiteStones);
				Assert.AreEqual(size, b.Size);
				Assert.AreEqual(b.Size * b.Size - 2 * i, b.NbEmptyIntersections);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException), "Indices should be outside [0, Size]")]
		public void Test_Indexer_IndexOutsideBoard()
		{
			Board b = new();
			var x = b[-1, -1];
		}

		[TestMethod]
		public void Test_PlaceStone()
		{
			Board b = new();
			b.PlaceStone(PlayerColor.Black, 0, 0);
			Assert.AreEqual(1, b.NbBlackStones);
			Assert.AreEqual(State.Black, b[0, 0].State);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "Intersection (0,0) should be occupied")]
		public void Test_PlaceStone_OccupiedIntersection()
		{
			Board b = new();
			b.PlaceStone(PlayerColor.Black, 0, 0);
			b.PlaceStone(PlayerColor.Black, 0, 0);
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException), "Indices should be outside [0, Size]")]
		public void Test_PlaceStone_IndexOutsideBoard()
		{
			Board b = new();
			b.PlaceStone(PlayerColor.Black, -1, -1);
		}

		[TestMethod]
		public void Test_Capture()
		{
			Board b = new();

			b.PlaceStone(PlayerColor.Black, 0, 0);
			Assert.AreEqual(1, b.NbBlackStones);
			Assert.AreEqual(State.Black, b[0, 0].State);

			b.Capture(0, 0);
			Assert.AreEqual(0, b.NbBlackStones);
			Assert.AreEqual(State.EmptyBlack, b[0, 0].State);
			Assert.IsTrue(b.IsEmpty);
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException), "Indices should be outside [0, Size]")]
		public void Test_Capture_IndexOutsideBoard()
		{
			Board b = new();
			b.PlaceStone(PlayerColor.Black, -1, -1);
		}

		[TestMethod]
		public void Test_UpdateEmptyIntersections()
		{
			Board b = new();

			for (int i = 0; i < b.Size; i++)
			{
				b.PlaceStone(PlayerColor.Black, i, 0);
				b.Capture(i, 0);
			}

			for (int i = 0; i < b.Size; i++)
			{
				Assert.AreEqual(State.EmptyBlack, b[i, 0].State);
			}

			b.UpdateEmptyIntersections();

			for (int i = 0; i < b.Size; i++)
			{
				Assert.AreEqual(State.Empty, b[i, 0].State);
			}
		}
	}
}
