using MathlibNET.Random;
using Source.Game;

namespace SharpGo.Game.Players
{
	public class RandomPlayer : Player
	{
		public RandomPlayer(Color color) : base(color) { }

		protected override (int, int) PickPosition(Board board, (State, int, int)[] cells)
		{
			int index = Rng.Rand(cells.Length);
			(_, int i, int j) = cells[index];
			return (i, j);
		}

		protected override bool Pass() => Rng.Rand() < 0.25;
	}
}
