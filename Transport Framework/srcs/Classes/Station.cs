using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TransportFramework.Classes
{
	public class Station
	{
		internal ContentPack		ContentPack = null;
		internal string				LocalizedDisplayName = null;
		internal int				DirectionAsInt = 2;
		internal bool				ConditionsCache = true;
		internal string				ConditionsLockedMessageCache = null;
		internal bool				Disabled = false;

		public STemplate			Template = null;
		public string				Id = null;
		public string				DisplayName = null;
		public string				Location = null;
		public Point				Tile = new(int.MinValue, int.MinValue);
		public string				Direction = null;
		public int					Price = int.MinValue;
		public string				Network = null;
		public List<Point>			AccessTiles = null;
		public List<SSprite>		Sprites = null;
		public List<SCondition>		Conditions = null;
		public List<SEvent>			Events = null;
		public string				Sound = null;
		public List<string>			Include = null;
		public List<string>			IncludeDeparture = null;
		public List<string>			IncludeArrival = null;
		public List<string>			Exclude = null;
		public List<string>			ExcludeDeparture = null;
		public List<string>			ExcludeArrival = null;
		public List<string>			IgnoreConditions = null;
		public List<string>			IgnoreConditionsDeparture = null;
		public List<string>			IgnoreConditionsArrival = null;
		public List<SRequiredMod>	RequiredMods = null;
	}
}
