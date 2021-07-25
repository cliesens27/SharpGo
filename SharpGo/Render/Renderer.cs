using System;
using DrawingBoardNET.Drawing;
using DrawingBoardNET.Drawing.Constants;
using DrawingBoardNET.Window;
using Source.Game;

namespace Source.Render
{
	public class Renderer
	{
		private const int HEIGHT = 800;
		private const int WIDTH = 2 * HEIGHT;
		private const int PANEL_BORDER = 10;
		private const int PANEL_HEIGHT = HEIGHT - 2 * PANEL_BORDER;
		private const int PANEL_WIDTH = PANEL_HEIGHT;

		private DrawingBoard db;

		public Renderer(out DrawingBoard db, DrawMethod draw)
		{
			db = new DrawingBoard(WIDTH, HEIGHT);
			db.Title = "SharpGo";
			db.TargetFrameRate = 60;
			db.Draw = draw;
			this.db = db;
		}

		public void Render(Board board)
		{
			db.Background(0);

			db.Stroke(255);
			db.Line(WIDTH / 2, 0, WIDTH / 2, HEIGHT);

			db.RectMode = RectangleMode.Corner;
					DrawLeftPanel(board);
			DrawRightPanel();
		}

		private void DrawLeftPanel(Board board)
		{
						db.Stroke(255);
			db.NoFill();
			db.Square(PANEL_BORDER, PANEL_BORDER, PANEL_HEIGHT);

			DrawBoard(board);
		}

		private void DrawBoard(Board board)
		{
			for (int i = 0; i < board.NbRows; i++)
			{
				for (int j = 0; j < board.NbCols; j++)
				{
					State state = board[i, j];
				}
			}
		}

		private void DrawRightPanel()
		{
			db.Stroke(255);
			db.NoFill();
			db.Square(WIDTH / 2 + PANEL_BORDER, PANEL_BORDER, PANEL_HEIGHT);
		}
	}
}
