using System.Collections.Generic;

namespace TransportFramework.Classes
{
	public class SEFilter
	{
		public IList<string>	IncludeStations = null;
		public IList<string>	ExcludeStations = null;
		public uint				TravelCount = 0;
	}
}
