

using Microsoft.Xna.Framework;

namespace Wundee
{
	public class Habitat : Entity
	{

		public bool IsOccupied()
		{


			return false;
		}

		public Habitat(World world, Vector2 startPosition) : base(world, startPosition)
		{

		}
	}

}
