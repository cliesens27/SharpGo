using DrawingBoardNET.Drawing;
using SharpGo.Game;
using SharpGo.Game.Players;
using SharpGo.Render;

namespace SharpGo.Tests
{
	public static class TestUtils
	{
		public static void DrawBoard(Board board, Player p1, Player p2, int frameRate)
		{
			DrawingBoard db;
			Renderer r = new Renderer(out db, frameRate);
			db.Draw = () => r.Render(board, p1, p2);
			db.Start();
		}
	}
}
