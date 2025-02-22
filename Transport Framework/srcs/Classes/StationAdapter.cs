using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TransportFramework.Api;

namespace TransportFramework.Classes
{
	public class StationAdapter : IStation
	{
		private readonly Station _station;

		public StationAdapter(Station station)
		{
			_station = station ?? throw new ArgumentNullException(nameof(station));
		}

		public Station Station
		{
			get => _station;
		}

		public ISTemplate Template
		{
			get => new ISTemplateAdapter(_station.Template);
			set => _station.Template = ((ISTemplateAdapter)value).Template;
		}

		public string Id
		{
			get => _station.Id;
			set => _station.Id = value;
		}

		public string DisplayName
		{
			get => _station.DisplayName;
			set => _station.DisplayName = value;
		}

		public string Location
		{
			get => _station.Location;
			set => _station.Location = value;
		}

		public Point Tile
		{
			get => _station.Tile;
			set => _station.Tile = value;
		}

		public string Direction
		{
			get => _station.Direction;
			set => _station.Direction = value;
		}

		public int Price
		{
			get => _station.Price;
			set => _station.Price = value;
		}

		public string Network
		{
			get => _station.Network;
			set => _station.Network = value;
		}

		public IList<Point> AccessTiles
		{
			get => _station.AccessTiles;
			set => _station.AccessTiles = new List<Point>(value);
		}

		public IList<ISSprite> Sprites
		{
			get => _station.Sprites.Select(c => new ISSpriteAdapter(c)).Cast<ISSprite>().ToList();
			set => _station.Sprites = value.Select(c => ((ISSpriteAdapter)c).Sprite).ToList();
		}

		public IList<ISCondition> Conditions
		{
			get => _station.Conditions.Select(c => new ISConditionAdapter(c)).Cast<ISCondition>().ToList();
			set => _station.Conditions = value.Select(c => ((ISConditionAdapter)c).Condition).ToList();
		}

		public IList<ISEvent> Events
		{
			get => _station.Events.Select(c => new ISEventAdapter(c)).Cast<ISEvent>().ToList();
			set => _station.Events = value.Select(c => ((ISEventAdapter)c).Event).ToList();
		}

		public string Sound
		{
			get => _station.Sound;
			set => _station.Sound = value;
		}

		public IList<string> Include
		{
			get => _station.Include;
			set => _station.Include = new List<string>(value);
		}

		public IList<string> IncludeDeparture
		{
			get => _station.IncludeDeparture;
			set => _station.IncludeDeparture = new List<string>(value);
		}

		public IList<string> IncludeArrival
		{
			get => _station.IncludeArrival;
			set => _station.IncludeArrival = new List<string>(value);
		}

		public IList<string> Exclude
		{
			get => _station.Exclude;
			set => _station.Exclude = new List<string>(value);
		}

		public IList<string> ExcludeDeparture
		{
			get => _station.ExcludeDeparture;
			set => _station.ExcludeDeparture = new List<string>(value);
		}

		public IList<string> ExcludeArrival
		{
			get => _station.ExcludeArrival;
			set => _station.ExcludeArrival = new List<string>(value);
		}

		public IList<string> IgnoreConditions
		{
			get => _station.IgnoreConditions;
			set => _station.IgnoreConditions = new List<string>(value);
		}

		public IList<string> IgnoreConditionsDeparture
		{
			get => _station.IgnoreConditionsDeparture;
			set => _station.IgnoreConditionsDeparture = new List<string>(value);
		}

		public IList<string> IgnoreConditionsArrival
		{
			get => _station.IgnoreConditionsArrival;
			set => _station.IgnoreConditionsArrival = new List<string>(value);
		}

		public IList<ISRequiredMod> RequiredMods
		{
			get => _station.RequiredMods.Select(c => new ISRequiredModAdapter(c)).Cast<ISRequiredMod>().ToList();
			set => _station.RequiredMods = value.Select(c => ((ISRequiredModAdapter)c).RequiredMod).ToList();
		}
	}

	public class ISTemplateAdapter : ISTemplate
	{
		private readonly STemplate _template;

		public ISTemplateAdapter(STemplate template)
		{
			_template = template ?? throw new ArgumentNullException(nameof(template));
		}

		public STemplate Template
		{
			get => _template;
		}

		public string Id
		{
			get => _template.Id;
			set => _template.Id = value;
		}

		public Point ReferenceTile
		{
			get => _template.ReferenceTile;
			set => _template.ReferenceTile = value;
		}
	}

	public class ISSpriteAdapter : ISSprite
	{
		private readonly SSprite _sprite;

		public ISSpriteAdapter(SSprite sprite)
		{
			_sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
		}

		public SSprite Sprite
		{
			get => _sprite;
		}

		public string Type
		{
			get => _sprite.Type;
			set => _sprite.Type = value;
		}

		public ISSData Data
		{
			get => new ISSDataAdapter(_sprite.Data);
			set => _sprite.Data = ((ISSDataAdapter)value).Data;
		}

		public IList<ISSCollisionBox> CollisionBoxes
		{
			get => _sprite.CollisionBoxes.Select(c => new ISSCollisionBoxAdapter(c)).Cast<ISSCollisionBox>().ToList();
			set => _sprite.CollisionBoxes = value.Select(c => ((ISSCollisionBoxAdapter)c).CollisionBox).ToList();
		}

		public IList<ISSCondition> Conditions
		{
			get => _sprite.Conditions.Select(c => new ISSConditionAdapter(c)).Cast<ISSCondition>().ToList();
			set => _sprite.Conditions = value.Select(c => ((ISSConditionAdapter)c).Condition).ToList();
		}
	}

	public class ISSDataAdapter : ISSData
	{
		private readonly SSData _data;

		public ISSDataAdapter(SSData data)
		{
			_data = data ?? throw new ArgumentNullException(nameof(data));
		}

		public SSData Data
		{
			get => _data;
		}

		public string TextureName
		{
			get => _data.TextureName;
			set => _data.TextureName = value;
		}

		public Rectangle SourceRectangle
		{
			get => _data.SourceRectangle;
			set => _data.SourceRectangle = value;
		}

		public Vector2 Position
		{
			get => _data.Position;
			set => _data.Position = value;
		}

		public float Interval
		{
			get => _data.Interval;
			set => _data.Interval = value;
		}

		public int AnimationLength
		{
			get => _data.AnimationLength;
			set => _data.AnimationLength = value;
		}

		public bool Flicker
		{
			get => _data.Flicker;
			set => _data.Flicker = value;
		}

		public bool Flip
		{
			get => _data.Flip;
			set => _data.Flip = value;
		}

		public bool VerticalFlip
		{
			get => _data.VerticalFlip;
			set => _data.VerticalFlip = value;
		}

		public float LayerDepth
		{
			get => _data.LayerDepth;
			set => _data.LayerDepth = value;
		}

		public string Color
		{
			get => _data.Color;
			set => _data.Color = value;
		}

		public float Scale
		{
			get => _data.Scale;
			set => _data.Scale = value;
		}

		public float Rotation
		{
			get => _data.Rotation;
			set => _data.Rotation = value;
		}

		public float ShakeIntensity
		{
			get => _data.ShakeIntensity;
			set => _data.ShakeIntensity = value;
		}

		public ISSDLight Light
		{
			get => new ISSDLightAdapter(_data.Light);
			set => _data.Light = ((ISSDLightAdapter)value).Light;
		}

		public bool PingPong
		{
			get => _data.PingPong;
			set => _data.PingPong = value;
		}

		public bool DrawAboveAlwaysFront
		{
			get => _data.DrawAboveAlwaysFront;
			set => _data.DrawAboveAlwaysFront = value;
		}
	}

	public class ISSDLightAdapter : ISSDLight
	{
		private readonly SSDLight _light;

		public ISSDLightAdapter(SSDLight light)
		{
			_light = light ?? throw new ArgumentNullException(nameof(light));
		}

		public SSDLight Light
		{
			get => _light;
		}

		public string Color
		{
			get => _light.Color;
			set => _light.Color = value;
		}

		public float Radius
		{
			get => _light.Radius;
			set => _light.Radius = value;
		}
	}

	public class ISSCollisionBoxAdapter : ISSCollisionBox
	{
		private readonly SSCollisionBox _collisionBox;

		public ISSCollisionBoxAdapter(SSCollisionBox collisionBox)
		{
			_collisionBox = collisionBox ?? throw new ArgumentNullException(nameof(collisionBox));
		}

		public SSCollisionBox CollisionBox
		{
			get => _collisionBox;
		}

		public float X
		{
			get => _collisionBox.X;
			set => _collisionBox.X = value;
		}

		public float Y
		{
			get => _collisionBox.Y;
			set => _collisionBox.Y = value;
		}

		public float Width
		{
			get => _collisionBox.Width;
			set => _collisionBox.Width = value;
		}

		public float Height
		{
			get => _collisionBox.Height;
			set => _collisionBox.Height = value;
		}
	}

	public class ISSConditionAdapter : ISSCondition
	{
		private readonly SSCondition _condition;

		public ISSConditionAdapter(SSCondition condition)
		{
			_condition = condition ?? throw new ArgumentNullException(nameof(condition));
		}

		public SSCondition Condition
		{
			get => _condition;
		}

		public string Query
		{
			get => _condition.Query;
			set => _condition.Query = value;
		}

		public string Update
		{
			get => _condition.Update;
			set => _condition.Update = value;
		}
	}

	public class ISConditionAdapter : ISCondition
	{
		private readonly SCondition _condition;

		public ISConditionAdapter(SCondition condition)
		{
			_condition = condition ?? throw new ArgumentNullException(nameof(condition));
		}

		public SCondition Condition
		{
			get => _condition;
		}

		public string Query
		{
			get => _condition.Query;
			set => _condition.Query = value;
		}

		public string LockedMessage
		{
			get => _condition.LockedMessage;
			set => _condition.LockedMessage = value;
		}

		public string Update
		{
			get => _condition.Update;
			set => _condition.Update = value;
		}
	}

	public class ISEventAdapter : ISEvent
	{
		private readonly SEvent _event;

		public ISEventAdapter(SEvent @event)
		{
			_event = @event ?? throw new ArgumentNullException(nameof(@event));
		}

		public SEvent Event
		{
			get => _event;
		}

		public string Type
		{
			get => _event.Type;
			set => _event.Type = value;
		}

		public string Id
		{
			get => _event.Id;
			set => _event.Id = value;
		}

		public string Script
		{
			get => _event.Script;
			set => _event.Script = value;
		}

		public string Location
		{
			get => _event.Location;
			set => _event.Location = value;
		}

		public ISEFilter Filter
		{
			get => new ISEFilterAdapter(_event.Filter);
			set => _event.Filter = ((ISEFilterAdapter)value).Filter;
		}
	}

	public class ISEFilterAdapter : ISEFilter
	{
		private readonly SEFilter _filter;

		public ISEFilterAdapter(SEFilter filter)
		{
			_filter = filter ?? throw new ArgumentNullException(nameof(filter));
		}

		public SEFilter Filter
		{
			get => _filter;
		}

		public IList<string> IncludeStations
		{
			get => _filter.IncludeStations;
			set => _filter.IncludeStations = value;
		}

		public IList<string> ExcludeStations
		{
			get => _filter.ExcludeStations;
			set => _filter.ExcludeStations = value;
		}

		public uint TravelCount
		{
			get => _filter.TravelCount;
			set => _filter.TravelCount = value;
		}
	}

	public class ISRequiredModAdapter : ISRequiredMod
	{
		private readonly SRequiredMod _requiredMod;

		public ISRequiredModAdapter(SRequiredMod requiredMod)
		{
			_requiredMod = requiredMod ?? throw new ArgumentNullException(nameof(requiredMod));
		}

		public SRequiredMod RequiredMod
		{
			get => _requiredMod;
		}

		public string Id
		{
			get => _requiredMod.Id;
			set => _requiredMod.Id = value;
		}

		public string MinimumVersion
		{
			get => _requiredMod.MinimumVersion;
			set => _requiredMod.MinimumVersion = value;
		}

		public string MaximumVersion
		{
			get => _requiredMod.MaximumVersion;
			set => _requiredMod.MaximumVersion = value;
		}
	}
}
