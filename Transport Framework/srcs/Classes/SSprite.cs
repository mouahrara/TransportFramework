using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TransportFramework.Classes
{
	public class SSprite
	{
		internal Station			Station = null;
		internal List<Rectangle>	AbsoluteCollisionBoxes = null;
		internal List<Rectangle>	ComputedCollisionBoxes = null;
		internal bool				ConditionsCache = true;

		public string				Type = "All";
		public SSData				Data = null;
		public List<SSCollisionBox>	CollisionBoxes = null;
		public List<SSCondition>	Conditions = null;
	}
}
