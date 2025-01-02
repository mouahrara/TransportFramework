using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class StringBuilderUtility
	{
		private static int tabs = 0;

		private static void Append(StringBuilder stringBuilder, string text)
		{
			for (int i = 0; i < tabs; i++)
			{
				stringBuilder.Append('\t');
			}
			stringBuilder.Append(text);
		}

		private static void AppendProperty(StringBuilder stringBuilder, string name, object value)
		{
			if (name is not null)
			{
				Append(stringBuilder, $"{name}: {value ?? "null"}\n");
			}
			else
			{
				Append(stringBuilder, $"{value ?? "null"}\n");
			}
		}

		private static void AppendOpen(StringBuilder stringBuilder, ref int tabs, string name, object value, char character)
		{
			if (value is null)
			{
				Append(stringBuilder, $"{name}: null\n");
			}
			else
			{
				if (name is not null)
				{
					Append(stringBuilder, $"{name}:\n");
				}
				Append(stringBuilder, $"{character}\n");
				tabs++;
			}
		}

		private static void AppendClose(StringBuilder stringBuilder, ref int tabs, object value, char character)
		{
			if (value is not null)
			{
				tabs--;
				Append(stringBuilder, $"{character}\n");
			}
		}

		private static void AppendObjectOpen(StringBuilder stringBuilder, string name, object value)
		{
			AppendOpen(stringBuilder, ref tabs, name, value, '{');
		}

		private static void AppendObjectClose(StringBuilder stringBuilder, object value)
		{
			AppendClose(stringBuilder, ref tabs, value, '}');
		}

		private static void AppendArrayOpen(StringBuilder stringBuilder, string name, object value)
		{
			AppendOpen(stringBuilder, ref tabs, name, value, '[');
		}

		private static void AppendArrayClose(StringBuilder stringBuilder, object value)
		{
			AppendClose(stringBuilder, ref tabs, value, ']');
		}

		private static void AppendArrayComma(StringBuilder stringBuilder)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append(",\n");
		}

		private static void RemoveArrayLastComma(StringBuilder stringBuilder)
		{
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			stringBuilder.Append('\n');
		}

		private static void AppendPoint(StringBuilder stringBuilder, string name, Point point)
		{
			AppendObjectOpen(stringBuilder, name, point);
			AppendProperty(stringBuilder, nameof(point.X), point.X);
			AppendArrayComma(stringBuilder);
			AppendProperty(stringBuilder, nameof(point.Y), point.Y);
			AppendObjectClose(stringBuilder, point);
		}

		private static void AppendSSprite(StringBuilder stringBuilder, string name, SSprite ssprite)
		{
			AppendObjectOpen(stringBuilder, name, ssprite);
			if (ssprite is not null)
			{
				AppendProperty(stringBuilder, nameof(ssprite.Type), ssprite.Type);
				AppendArrayComma(stringBuilder);
				AppendSSData(stringBuilder, nameof(ssprite.Data), ssprite.Data);
				AppendArrayComma(stringBuilder);
				AppendItems(stringBuilder, nameof(ssprite.AbsoluteCollisionBoxes), ssprite.AbsoluteCollisionBoxes, AppendRectangle);
				AppendArrayComma(stringBuilder);
				AppendItems(stringBuilder, nameof(ssprite.ComputedCollisionBoxes), ssprite.ComputedCollisionBoxes, AppendRectangle);
				AppendArrayComma(stringBuilder);
				AppendItems(stringBuilder, nameof(ssprite.CollisionBoxes), ssprite.CollisionBoxes, AppendSSCollisionBox);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssprite.ConditionsCache), ssprite.ConditionsCache);
				AppendArrayComma(stringBuilder);
				AppendItems(stringBuilder, nameof(ssprite.Conditions), ssprite.Conditions, AppendSSCondition);
			}
			AppendObjectClose(stringBuilder, ssprite);
		}

		private static void AppendSSData(StringBuilder stringBuilder, string name, SSData ssdata)
		{
			AppendObjectOpen(stringBuilder, name, ssdata);
			if (ssdata is not null)
			{
				AppendProperty(stringBuilder, nameof(ssdata.InternalTextureName), ssdata.InternalTextureName);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.TextureName), ssdata.TextureName);
				AppendArrayComma(stringBuilder);
				AppendRectangle(stringBuilder, nameof(ssdata.SourceRectangle), ssdata.SourceRectangle);
				AppendArrayComma(stringBuilder);
				AppendVector(stringBuilder, nameof(ssdata.ComputedPosition), ssdata.ComputedPosition);
				AppendArrayComma(stringBuilder);
				AppendVector(stringBuilder, nameof(ssdata.Position), ssdata.Position);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.Interval), ssdata.Interval);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.AnimationLength), ssdata.AnimationLength);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.Flicker), ssdata.Flicker);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.Flip), ssdata.Flip);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.VerticalFlip), ssdata.VerticalFlip);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.ComputedLayerDepth), ssdata.ComputedLayerDepth);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.LayerDepth), ssdata.LayerDepth);
				AppendArrayComma(stringBuilder);
				AppendColor(stringBuilder, nameof(ssdata.ColorAsColor), ssdata.ColorAsColor);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.Color), ssdata.Color);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.Scale), ssdata.Scale);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.Rotation), ssdata.Rotation);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.ShakeIntensity), ssdata.ShakeIntensity);
				AppendArrayComma(stringBuilder);
				AppendSSDLight(stringBuilder, nameof(ssdata.Light), ssdata.Light);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.PingPong), ssdata.PingPong);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdata.DrawAboveAlwaysFront), ssdata.DrawAboveAlwaysFront);
			}
			AppendObjectClose(stringBuilder, ssdata);
		}

		private static void AppendVector(StringBuilder stringBuilder, string name, Vector2 vector)
		{
			AppendObjectOpen(stringBuilder, name, vector);
			AppendProperty(stringBuilder, nameof(vector.X), vector.X);
			AppendArrayComma(stringBuilder);
			AppendProperty(stringBuilder, nameof(vector.Y), vector.Y);
			AppendObjectClose(stringBuilder, vector);
		}

		private static void AppendColor(StringBuilder stringBuilder, string name, Color color)
		{
			AppendObjectOpen(stringBuilder, name, color);
			AppendProperty(stringBuilder, nameof(color.R), color.R);
			AppendArrayComma(stringBuilder);
			AppendProperty(stringBuilder, nameof(color.G), color.G);
			AppendArrayComma(stringBuilder);
			AppendProperty(stringBuilder, nameof(color.B), color.B);
			AppendArrayComma(stringBuilder);
			AppendProperty(stringBuilder, nameof(color.A), color.A);
			AppendObjectClose(stringBuilder, color);
		}

		private static void AppendSSDLight(StringBuilder stringBuilder, string name, SSDLight ssdLight)
		{
			AppendObjectOpen(stringBuilder, name, ssdLight);
			if (ssdLight is not null)
			{
				AppendColor(stringBuilder, nameof(ssdLight.ColorAsColor), ssdLight.ColorAsColor);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdLight.Color), ssdLight.Color);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(ssdLight.Radius), ssdLight.Radius);
			}
			AppendObjectClose(stringBuilder, ssdLight);
		}

		private static void AppendRectangle(StringBuilder stringBuilder, string name, Rectangle rectangle)
		{
			AppendObjectOpen(stringBuilder, name, rectangle);
			AppendProperty(stringBuilder, nameof(rectangle.X), rectangle.X);
			AppendArrayComma(stringBuilder);
			AppendProperty(stringBuilder, nameof(rectangle.Y), rectangle.Y);
			AppendArrayComma(stringBuilder);
			AppendProperty(stringBuilder, nameof(rectangle.Width), rectangle.Width);
			AppendArrayComma(stringBuilder);
			AppendProperty(stringBuilder, nameof(rectangle.Height), rectangle.Height);
			AppendObjectClose(stringBuilder, rectangle);
		}

		private static void AppendSSCollisionBox(StringBuilder stringBuilder, string name, SSCollisionBox sscollisionBox)
		{
			AppendObjectOpen(stringBuilder, name, sscollisionBox);
			if (sscollisionBox is not null)
			{
				AppendProperty(stringBuilder, nameof(sscollisionBox.X), sscollisionBox.X);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(sscollisionBox.Y), sscollisionBox.Y);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(sscollisionBox.Width), sscollisionBox.Width);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(sscollisionBox.Height), sscollisionBox.Height);
			}
			AppendObjectClose(stringBuilder, sscollisionBox);
		}

		private static void AppendSSCondition(StringBuilder stringBuilder, string name, SSCondition sscondition)
		{
			AppendObjectOpen(stringBuilder, name, sscondition);
			if (sscondition is not null)
			{
				AppendProperty(stringBuilder, nameof(sscondition.Cache), sscondition.Cache);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(sscondition.Query), sscondition.Query);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(sscondition.Update), sscondition.Update);
			}
			AppendObjectClose(stringBuilder, sscondition);
		}

		private static void AppendSCondition(StringBuilder stringBuilder, string name, SCondition scondition)
		{
			AppendObjectOpen(stringBuilder, name, scondition);
			if (scondition is not null)
			{
				AppendProperty(stringBuilder, nameof(scondition.Cache), scondition.Cache);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(scondition.Query), scondition.Query);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(scondition.LockedMessage), scondition.LockedMessage);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(scondition.Update), scondition.Update);
			}
			AppendObjectClose(stringBuilder, scondition);
		}

		private static void AppendSEvent(StringBuilder stringBuilder, string name, SEvent sevent)
		{
			AppendObjectOpen(stringBuilder, name, sevent);
			if (sevent is not null)
			{
				AppendProperty(stringBuilder, nameof(sevent.Type), sevent.Type);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(sevent.Id), sevent.Id);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(sevent.Script), sevent.Script);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(sevent.Location), sevent.Location);
				AppendArrayComma(stringBuilder);
				AppendSEFilter(stringBuilder, nameof(sevent.Filter), sevent.Filter);
			}
			AppendObjectClose(stringBuilder, sevent);
		}

		private static void AppendSEFilter(StringBuilder stringBuilder, string name, SEFilter sefilter)
		{
			AppendObjectOpen(stringBuilder, name, sefilter);
			if (sefilter is not null)
			{
				AppendItems(stringBuilder, nameof(sefilter.IncludeStations), sefilter.IncludeStations, AppendProperty);
				AppendArrayComma(stringBuilder);
				AppendItems(stringBuilder, nameof(sefilter.ExcludeStations), sefilter.ExcludeStations, AppendProperty);
				AppendArrayComma(stringBuilder);
				AppendProperty(stringBuilder, nameof(sefilter.TravelCount), sefilter.TravelCount);
			}
			AppendObjectClose(stringBuilder, sefilter);
		}

		private static void AppendItems<T>(StringBuilder stringBuilder, string name, IEnumerable<T> items, Action<StringBuilder, string, T> appendItem)
		{
			AppendArrayOpen(stringBuilder, name, items);
			if (items is not null)
			{
				foreach (T item in items)
				{
					appendItem(stringBuilder, null, item);
					AppendArrayComma(stringBuilder);
				}
				RemoveArrayLastComma(stringBuilder);
			}
			AppendArrayClose(stringBuilder, items);
		}

		public static void AppendTemplate(StringBuilder stringBuilder, string manifestUniqueID, Template template)
		{
			stringBuilder.Append($"\n=== {manifestUniqueID} ===\n");
			if (template is not null)
			{
				AppendProperty(stringBuilder, nameof(template.Id), template.Id);
				AppendProperty(stringBuilder, nameof(template.Price), template.Price);
				AppendProperty(stringBuilder, nameof(template.Network), template.Network);
				AppendItems(stringBuilder, nameof(template.AccessTiles), template.AccessTiles, AppendPoint);
				AppendItems(stringBuilder, nameof(template.Sprites), template.Sprites, AppendSSprite);
				AppendItems(stringBuilder, nameof(template.Conditions), template.Conditions, AppendSCondition);
				AppendItems(stringBuilder, nameof(template.Events), template.Events, AppendSEvent);
				AppendProperty(stringBuilder, nameof(template.Sound), template.Sound);
			}
		}

		public static void AppendStation(StringBuilder stringBuilder, string manifestUniqueID, Station station)
		{
			stringBuilder.Append($"\n=== {manifestUniqueID} ===\n");
			if (station is not null)
			{
				AppendProperty(stringBuilder, nameof(station.Id), station.Id);
				AppendProperty(stringBuilder, nameof(station.DisplayName), station.DisplayName);
				AppendProperty(stringBuilder, nameof(station.LocalizedDisplayName), station.LocalizedDisplayName);
				AppendProperty(stringBuilder, nameof(station.Location), station.Location);
				AppendPoint(stringBuilder, nameof(station.Tile), station.Tile);
				AppendProperty(stringBuilder, nameof(station.Direction), station.Direction);
				AppendProperty(stringBuilder, nameof(station.DirectionAsInt), station.DirectionAsInt);
				AppendProperty(stringBuilder, nameof(station.Price), station.Price);
				AppendProperty(stringBuilder, nameof(station.Network), station.Network);
				AppendItems(stringBuilder, nameof(station.AccessTiles), station.AccessTiles, AppendPoint);
				AppendProperty(stringBuilder, nameof(station.ConditionsCache), station.ConditionsCache);
				AppendProperty(stringBuilder, nameof(station.ConditionsLockedMessageCache), station.ConditionsLockedMessageCache);
				AppendItems(stringBuilder, nameof(station.Sprites), station.Sprites, AppendSSprite);
				AppendItems(stringBuilder, nameof(station.Conditions), station.Conditions, AppendSCondition);
				AppendItems(stringBuilder, nameof(station.Events), station.Events, AppendSEvent);
				AppendProperty(stringBuilder, nameof(station.Sound), station.Sound);
				AppendItems(stringBuilder, nameof(station.Include), station.Include, AppendProperty);
				AppendItems(stringBuilder, nameof(station.IncludeDeparture), station.IncludeDeparture, AppendProperty);
				AppendItems(stringBuilder, nameof(station.IncludeArrival), station.IncludeArrival, AppendProperty);
				AppendItems(stringBuilder, nameof(station.Exclude), station.Exclude, AppendProperty);
				AppendItems(stringBuilder, nameof(station.ExcludeDeparture), station.ExcludeDeparture, AppendProperty);
				AppendItems(stringBuilder, nameof(station.ExcludeArrival), station.ExcludeArrival, AppendProperty);
				AppendItems(stringBuilder, nameof(station.IgnoreConditions), station.IgnoreConditions, AppendProperty);
				AppendItems(stringBuilder, nameof(station.IgnoreConditionsDeparture), station.IgnoreConditionsDeparture, AppendProperty);
				AppendItems(stringBuilder, nameof(station.IgnoreConditionsArrival), station.IgnoreConditionsArrival, AppendProperty);
			}
		}
	}
}
