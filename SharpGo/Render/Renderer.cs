using DrawingBoardNET.Drawing;
using DrawingBoardNET.Drawing.Constants;
using MathlibNET;
using Source.Game;

namespace Source.Render
{
	public class Renderer
	{
		private const int HEIGHT = 900;
		private const int WIDTH = 2 * HEIGHT;
		private const float PANEL_BORDER = 15;
		private const float PANEL_SIZE = HEIGHT - 2 * PANEL_BORDER;
		private const float BASE_RADIUS = HEIGHT / 4.0f;
		private readonly DrawingBoard db;

		public Renderer(out DrawingBoard db)
		{
			db = new DrawingBoard(WIDTH, HEIGHT);
			db.Title = "SharpGo";
			db.TargetFrameRate = 999;
			this.db = db;
		}

		public void Render(Board board)
		{
			db.Background(0);

			db.StrokeWidth(1);
			db.Stroke(255);
			db.Line(WIDTH / 2.0f, 0, WIDTH / 2.0f, HEIGHT);

			db.RectMode = RectangleMode.Corner;
			DrawLeftPanel(board);
			DrawRightPanel();
		}

		private void DrawLeftPanel(Board board) => DrawBoard(board);

		private void DrawBoard(Board board)
		{
			db.NoStroke();
			db.Fill(35);
			db.Square(2 * PANEL_BORDER, 2 * PANEL_BORDER, PANEL_SIZE - 2 * PANEL_BORDER);

			DrawGrid(board);

			float r = BASE_RADIUS / board.NbRows;

			for (int i = 0; i < board.NbRows; i++)
			{
				for (int j = 0; j < board.NbCols; j++)
				{
					DrawStone(board[i, j], board.NbRows, board.NbCols, i, j, r);
				}
			}
		}

		private void DrawStone(State state, int nbRows, int nbCols, int i, int j, float radius)
		{
			switch (state)
			{
				case State.Empty:
					return;
				case State.White:
					db.Stroke(0);
					db.Fill(255);
					break;
				case State.Black:
					db.Stroke(255);
					db.Fill(0);
					break;
			}

			float x = SpecialFunctions.Lerp(j, -0.5f, nbCols - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);
			float y = SpecialFunctions.Lerp(i, -0.5f, nbRows - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);

			db.StrokeWidth(3);
			db.Circle(x, y, radius);
		}

		private void DrawGrid(Board board)
		{
			db.StrokeWidth(1);
			db.Stroke(255);

			float offset = 0.5f * PANEL_SIZE / board.NbRows;

			for (int i = 0; i < board.NbRows; i++)
			{
				float x1 = SpecialFunctions.Lerp(i, -0.5f, board.NbRows - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);
				float y1 = PANEL_BORDER + offset;
				float x2 = x1;
				float y2 = PANEL_SIZE + PANEL_BORDER - offset;

				db.Line(x1, y1, x2, y2);
			}

			for (int i = 0; i < board.NbCols; i++)
			{
				float x1 = PANEL_BORDER + offset;
				float y1 = SpecialFunctions.Lerp(i, -0.5f, board.NbCols - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);
				float x2 = PANEL_SIZE + PANEL_BORDER - offset;
				float y2 = y1;

				db.Line(x1, y1, x2, y2);
			}
		}

		private void DrawRightPanel()
		{
			db.NoStroke();
			db.Fill(35);
			db.Square(WIDTH / 2 + 1, 0, PANEL_SIZE + 2 * PANEL_BORDER);

			db.StrokeWidth(1);
			db.Stroke(255);
			db.Fill(0);
			db.Square(WIDTH / 2 + PANEL_BORDER, PANEL_BORDER, PANEL_SIZE);
		}
	}
}
