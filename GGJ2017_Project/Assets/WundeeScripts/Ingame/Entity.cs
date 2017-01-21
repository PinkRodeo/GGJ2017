

using Microsoft.Xna.Framework;

namespace Wundee
{
	public class Entity
	{
		protected World world;

		public Entity(World world, Vector2 startPosition)
		{
			this.world = world;
			
		}

		public virtual void LateUpdate()
		{

		}
	}

}

