using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TransportFramework.Classes
{
	public class SSData
	{
		internal Texture2D	Texture = null;
		internal string		InternalTextureName = null;
		internal Vector2	ComputedPosition = Vector2.Zero;
		internal float		ComputedLayerDepth = 0f;
		internal Color		ColorAsColor = Microsoft.Xna.Framework.Color.White;

		public string		TextureName = null;
		public Rectangle	SourceRectangle = Rectangle.Empty;
		public Vector2		Position = new(float.MinValue, float.MinValue);
		public float		Interval = 999999f;
		public int			AnimationLength = 1;
		public bool			Flicker = false;
		public bool			Flip = false;
		public bool			VerticalFlip = false;
		public float		LayerDepth = float.MinValue;
		public string		Color = null;
		public float		Scale = 1f;
		public float		Rotation = 0f;
		public float		ShakeIntensity = 0f;
		public SSDLight		Light = null;
		public bool			PingPong = false;
		public bool			DrawAboveAlwaysFront = false;
	}
}
