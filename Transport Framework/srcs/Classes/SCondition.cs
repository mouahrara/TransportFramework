namespace TransportFramework.Classes
{
	public class SCondition
	{
		internal Station	Station = null;
		internal bool		Cache = true;

		public string		Query = null;
		public string		LockedMessage = null;
		public string		Update = "OnDayStart";
	}
}
