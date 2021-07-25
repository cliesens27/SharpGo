using System;
using DrawingBoardNET.Drawing;
using DrawingBoardNET.Drawing.Constants;
using DrawingBoardNET.Window;
using MathlibNET;
using Source.Game;

namespace Source.Render
{
	public class Renderer
	{
		private const int HEIGHT = 800;
		private const int WIDTH = 2 * HEIGHT;
		private const int PANEL_BORDER = 10;
		private const int PANEL_SIZE = HEIGHT - 2 * PANEL_BORDER;

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
			db.Square(PANEL_BORDER, PANEL_BORDER, PANEL_SIZE);

			DrawBoard(board);
		}

		private void DrawBoard(Board board)
		{
			DrawGrid(board);

			for (int i = 0; i < board.NbRows; i++)
			{
				for (int j = 0; j < board.NbCols; j++)
				{
					State state = board[i, j];

				}
			}
		}

		private void DrawGrid(Board board)
		{
			db.Stroke(255);

			for (int i = 0; i <= board.NbRows; i++)
			{
				float x1 = SpecialFunctions.Lerp(i, 0, board.NbRows, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);
				float y1 = PANEL_BORDER;
				float x2 = x1;
				float y2 = PANEL_SIZE + PANEL_BORDER;

				db.Line(x1, y1, x2, y2);
			}

			for (int i = 0; i <= board.NbCols; i++)
			{
				float x1 = PANEL_BORDER;
				float y1 = SpecialFunctions.Lerp(i, 0, board.NbRows, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);
				float x2 = PANEL_SIZE + PANEL_BORDER;
				float y2 = y1;

				db.Line(x1, y1, x2, y2);
			}
		}

		private void DrawRightPanel()
		{
			db.Stroke(255);
			db.NoFill();
			db.Square(WIDTH / 2 + PANEL_BORDER, PANEL_BORDER, PANEL_SIZE);
		}
	}
}
