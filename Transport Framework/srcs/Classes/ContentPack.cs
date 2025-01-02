using System.Collections.Generic;
using StardewModdingAPI;

namespace TransportFramework.Classes
{
	public class ContentPack
	{
		internal IManifest			Manifest = null;
		internal IModContentHelper	ModContent = null;
		internal ITranslationHelper	Translation = null;

		public string				Format = null;
		public List<Template>		Templates = null;
		public List<Station>		Stations = null;
	}
}
