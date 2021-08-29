using DrawingBoardNET.Drawing;
using SharpGo.Game;
using SharpGo.Game.Players;
using SharpGo.Render;

namespace SharpGo.Tests
{
	public static class TestUtils
	{
		public static void DrawBoard(Board board, int frameRate = 30, Player p1 = null, Player p2 = null)
		{
			DrawingBoard db;
			Renderer r = new Renderer(out db, frameRate);
			db.Draw = () => r.Render(board, p1, p2);
			db.Start();
		}
	}
}
