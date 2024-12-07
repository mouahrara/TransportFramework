namespace TransportFramework.Classes
{
	public class SEvent
	{
		internal Station	Station = null;

		public string		Type = "Departure";
		public string		Id = null;
		public string		Script = null;
		public string		Location = null;
		public SEFilter		Filter = new();
	}
}
