using MathlibNET.Random;
using Source.Game;

namespace SharpGo.Game.Players
{
	public class RandomPlayer : Player
	{
		public RandomPlayer(State color) : base(color) { }

		public override void MakeMove(Board board)
		{
			(State, int, int)[] cells = board.GetUnoccupiedCellsArray();

			if (cells.Length == 0)
			{
				return;
			}

			int index = Rng.Rand(cells.Length);
			(State s, int i, int j) = cells[index];

			board.AddStone(Color, i, j);
		}
	}
}
