using Source.Game;

namespace SharpGo.Game.Players
{
	public abstract class Player
	{
		public State Color { get; }

		protected Player(State color)
		{
			Color = color;
		}

		public abstract void MakeMove(Board board);
	}
}
