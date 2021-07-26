using MathlibNET.Random;
using Source.Game;

namespace SharpGo.Game.Players
{
	public class RandomPlayer : Player
	{
		public RandomPlayer(Color color) : base(color) { }

		public override void MakeMove(Board board)
		{
			if (Pass())
			{
				return;
			}
			else
			{
				(State, int, int)[] cells = board.GetUnoccupiedCellsArray(Color);

				if (cells.Length == 0)
				{
					return;
				}

				int index = Rng.Rand(cells.Length);
				(_, int i, int j) = cells[index];

				AddStone(board, i, j);
			}
		}

		protected override bool Pass() => Rng.Rand() < 0.25;
	}
}
