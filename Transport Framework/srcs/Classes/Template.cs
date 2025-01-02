using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TransportFramework.Classes
{
	public class Template
	{
		internal ContentPack	ContentPack = null;

		public string			Id = null;
		public int				Price = int.MinValue;
		public string			Network = null;
		public List<Point>		AccessTiles = null;
		public List<SSprite>	Sprites = null;
		public List<SCondition>	Conditions = null;
		public List<SEvent>		Events = null;
		public string			Sound = null;
	}
}
