using DrawingBoardNET.Drawing;
using SharpGo.Game;
using SharpGo.Render;

namespace SharpGo.Tests
{
	public static class TestUtils
	{
		public static void DrawBoard(Board board)
		{
			DrawingBoard db;
			Renderer r = new Renderer(out db);
			db.Draw = () => r.Render(board);
			db.Start();
		}
	}
}
