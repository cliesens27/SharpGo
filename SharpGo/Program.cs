using DrawingBoardNET.Drawing;
using Source.Game;
using Source.Render;

namespace SharpGo
{
	public class Program
	{
		private static DrawingBoard db;
		private static Board board;
		private static Renderer renderer;

		static void Main(string[] args)
		{
			board = new Board(19);
			renderer = new Renderer(out db, Draw);

			db.Start();
		}

		private static void Update() { }

		private static void Draw()
		{
			Update();
			renderer.Render(board);
		}
	}
}
