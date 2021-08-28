using DrawingBoardNET.Drawing;
using DrawingBoardNET.Drawing.Constants;
using MathlibNET;
using SharpGo.Game;
using SharpGo.Game.Players;

namespace SharpGo.Render
{
	public class Renderer
	{
		private const int HEIGHT = 950;
		private const int WIDTH = 2 * HEIGHT;
		private const float PANEL_BORDER = 10;
		private const float PANEL_SIZE = HEIGHT - 2 * PANEL_BORDER;
		private const float BASE_RADIUS = HEIGHT / 4.0f;
		private readonly DrawingBoard db;

		public Renderer(out DrawingBoard db, int frameRate)
		{
			db = new DrawingBoard(WIDTH, HEIGHT);
			db.Title = "SharpGo";
			db.TargetFrameRate = frameRate;
			this.db = db;
		}

		public void Render(Board board, Player player1, Player player2)
		{
			db.Background(0);

			db.StrokeWidth(1);
			db.Stroke(255);
			db.Line(WIDTH / 2.0f, 0, WIDTH / 2.0f, HEIGHT);

			db.RectMode = RectangleMode.Corner;
			DrawPlayers(player1, player2);
			DrawLeftPanel(board);
		}

		public void Render(GoGame game)
		{
			db.Background(0);

			db.StrokeWidth(1);
			db.Stroke(255);
			db.Line(WIDTH / 2.0f, 0, WIDTH / 2.0f, HEIGHT);

			db.RectMode = RectangleMode.Corner;
			DrawRightPanel(game);
			DrawLeftPanel(game.Board);
		}

		private void DrawLeftPanel(Board board) => DrawBoard(board);

		private void DrawBoard(Board board)
		{
			db.NoStroke();
			db.Fill(35);
			db.Square(2 * PANEL_BORDER, 2 * PANEL_BORDER, PANEL_SIZE - 2 * PANEL_BORDER);

			DrawGrid(board);

			float radius = BASE_RADIUS / board.Size;

			for (int i = 0; i < board.Size; i++)
			{
				for (int j = 0; j < board.Size; j++)
				{
					DrawStone(board[i, j], board.Size, board.Size, i, j, radius);
				}
			}
		}

		private void DrawStone(State state, int nbRows, int Size, int i, int j, float radius)
		{
			switch (state)
			{
				case State.Empty:
				case State.EmptyWhite:
				case State.EmptyBlack:
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

			float x = SpecialFunctions.Lerp(j, -0.5f, Size - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);
			float y = SpecialFunctions.Lerp(i, -0.5f, nbRows - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);

			db.StrokeWidth(2);
			db.Circle(x, y, radius);
		}

		private void DrawGrid(Board board)
		{
			db.StrokeWidth(1);
			db.Stroke(255);

			float offset = 0.5f * PANEL_SIZE / board.Size;

			for (int i = 0; i < board.Size; i++)
			{
				float x1 = SpecialFunctions.Lerp(i, -0.5f, board.Size - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);
				float y1 = PANEL_BORDER + offset;
				float x2 = x1;
				float y2 = PANEL_SIZE + PANEL_BORDER - offset;

				db.Line(x1, y1, x2, y2);
			}

			for (int i = 0; i < board.Size; i++)
			{
				float x1 = PANEL_BORDER + offset;
				float y1 = SpecialFunctions.Lerp(i, -0.5f, board.Size - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER);
				float x2 = PANEL_SIZE + PANEL_BORDER - offset;
				float y2 = y1;

				db.Line(x1, y1, x2, y2);
			}
		}

		private void DrawPlayers(Player player1, Player player2)
		{
			db.NoStroke();
			db.Fill(35);
			db.Square(WIDTH / 2 + 1, 0, PANEL_SIZE + 2 * PANEL_BORDER);

			db.StrokeWidth(1);
			db.Stroke(255);
			db.Fill(0);
			db.Square(WIDTH / 2 + PANEL_BORDER, PANEL_BORDER, PANEL_SIZE);

			DrawPlayer(player1, 1, WIDTH / 2 + PANEL_BORDER, PANEL_BORDER);
			DrawPlayer(player2, 2, 3 * WIDTH / 4 + PANEL_BORDER, PANEL_BORDER);
		}

		private void DrawRightPanel(GoGame game)
		{
			db.NoStroke();
			db.Fill(35);
			db.Square(WIDTH / 2 + 1, 0, PANEL_SIZE + 2 * PANEL_BORDER);

			db.StrokeWidth(1);
			db.Stroke(255);
			db.Fill(0);
			db.Square(WIDTH / 2 + PANEL_BORDER, PANEL_BORDER, PANEL_SIZE);

			DrawPlayer(game.Player1, 1, WIDTH / 2 + PANEL_BORDER, PANEL_BORDER);
			DrawPlayer(game.Player2, 2, 3 * WIDTH / 4 + PANEL_BORDER, PANEL_BORDER);

			DrawGameInfo(game);
		}

		private void DrawPlayer(Player player, int playerNumber, float x, float y)
		{
			db.TextColor(255);
			db.FontSize(24);
			db.Text($"Player {playerNumber}", x, y);

			db.FontSize(18);
			db.Text($"{player.Color}", x, y + 60);
			db.Text($"{(player.HasPassed ? "Passed" : "Played")}", x, y + 90);
		}

		private void DrawGameInfo(GoGame game)
		{
			db.TextColor(255);
			db.FontSize(24);
			db.Text($"Turns : {game.NbTurns}", WIDTH / 2 + PANEL_BORDER, PANEL_BORDER + PANEL_SIZE / 2);
			db.Text($"{(game.GameHasEnded ? "Game has ended" : "")}", WIDTH / 2 + PANEL_BORDER, PANEL_BORDER + PANEL_SIZE / 2 + 50);
		}

		private void DrawConnectedIntersections(Board board, int i, int j, int r, int g, int b, float radius)
		{
			var connectedIntersections = board.GetConnectedIntersections(i, j);
			connectedIntersections.Add(new Intersection(board[i, j], i, j));

			db.Stroke(255 - r, 255 - g, 255 - b);
			db.StrokeWidth(3);
			db.Fill(r, g, b);

			foreach (var intersection in connectedIntersections)
			{
				float x = SpecialFunctions.Lerp(
					intersection.J, -0.5f, board.Size - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER
				);
				float y = SpecialFunctions.Lerp(
					intersection.I, -0.5f, board.Size - 0.5f, PANEL_BORDER, PANEL_SIZE + PANEL_BORDER
				);

				db.Circle(x, y, radius / 2);
			}
		}
	}
}
