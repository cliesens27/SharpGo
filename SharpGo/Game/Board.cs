namespace Source.Game
{
	public class Board
	{
		public State this[int i, int j] => states[i][j];
		public int NbRows { get; }
		public int NbCols { get; }

		private readonly State[][] states;

		public Board(int size) : this(size, size) { }

		public Board(int rows, int cols)
		{
			NbRows = rows;
			NbCols = cols;

			states = new State[rows][];

			for (int i = 0; i < rows; i++)
			{
				states[i] = new State[cols];
			}

			Reset();
		}

		private void Reset()
		{
			for (int i = 0; i < NbRows; i++)
			{
				for (int j = 0; j < NbCols; j++)
				{
					states[i][j] = State.Empty;
				}
			}
		}
	}
}
