using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TransportFramework.Api
{
	/// <summary>
	/// The API for interacting with the Transport Framework.
	/// </summary>
	public interface ITransportFrameworkApi
	{
		[Flags]
		public enum TypeMask : byte
		{
			None = 0,
			StationCondition = 1,
			SpriteCondition = 2,
			All = StationCondition | SpriteCondition
		}

		[Flags]
		public enum UpdateMask : byte
		{
			None = 0,
			OnSaveLoaded = 1,
			OnDayStart = 2,
			OnLocationChange = 4,
			OnInteract = 8,
			All = OnSaveLoaded | OnDayStart | OnLocationChange | OnInteract
		}

		/// <summary>
		/// Gets the station with the provided identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the station.</param>
		/// <returns>The <c>IStation</c> that matches the given identifier, or <c>null</c> if not found.</returns>
		IStation GetStation(string id);

		/// <summary>
		/// Sets the station with the provided identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the station.</param>
		/// <param name="station">The station to be set.</param>
		/// <returns><c>true</c> if the station was successfully set; otherwise, <c>false</c>.</returns>
		bool SetStation(string id, IStation station);

		/// <summary>
		/// Updates the station with the provided identifier based on the specified type and update masks.
		/// </summary>
		/// <param name="id">The unique identifier of the station.</param>
		/// <param name="typeMask">A bitmask specifying the types of conditions to be updated. This determines whether to update station conditions, sprite conditions, or both.</param>
		/// <param name="updateMask">A bitmask specifying which specific conditions should be updated. This determines which of the predefined condition categories (OnDayStart, OnLocationChange, or OnInteract) should be updated.</param>
		/// <returns><c>true</c> if the station was successfully updated; otherwise, <c>false</c>.</returns>
		bool UpdateStation(string id, TypeMask typeMask = TypeMask.All, UpdateMask updateMask = UpdateMask.All);
	}

	public interface IStation
	{
		public string				Id							{ get; set; }
		public string				DisplayName					{ get; set; }
		public string				Location					{ get; set; }
		public Point				Tile						{ get; set; }
		public string				Direction					{ get; set; }
		public int					Price						{ get; set; }
		public string				Network						{ get; set; }
		public IList<Point>			AccessTiles					{ get; set; }
		public IList<ISSprite>		Sprites						{ get; set; }
		public IList<ISCondition>	Conditions					{ get; set; }
		public IList<ISEvent>		Events						{ get; set; }
		public string				Sound						{ get; set; }
		public IList<string>		Include						{ get; set; }
		public IList<string>		IncludeDeparture			{ get; set; }
		public IList<string>		IncludeArrival				{ get; set; }
		public IList<string>		Exclude						{ get; set; }
		public IList<string>		ExcludeDeparture			{ get; set; }
		public IList<string>		ExcludeArrival				{ get; set; }
		public IList<string>		IgnoreConditions			{ get; set; }
		public IList<string>		IgnoreConditionsDeparture	{ get; set; }
		public IList<string>		IgnoreConditionsArrival		{ get; set; }
		public IList<ISRequiredMod>	RequiredMods				{ get; set; }
	}

	public interface ISSprite
	{
		public string					Type			{ get; set; }
		public ISSData					Data			{ get; set; }
		public IList<ISSCollisionBox>	CollisionBoxes	{ get; set; }
		public IList<ISSCondition>		Conditions		{ get; set; }
	}

	public interface ISSData
	{
		public string		TextureName				{ get; set; }
		public Rectangle	SourceRectangle			{ get; set; }
		public Vector2		Position				{ get; set; }
		public float		Interval				{ get; set; }
		public int			AnimationLength			{ get; set; }
		public bool			Flicker					{ get; set; }
		public bool			Flip					{ get; set; }
		public bool			VerticalFlip			{ get; set; }
		public float		LayerDepth				{ get; set; }
		public string		Color					{ get; set; }
		public float		Scale					{ get; set; }
		public float		Rotation				{ get; set; }
		public float		ShakeIntensity			{ get; set; }
		public ISSDLight	Light					{ get; set; }
		public bool			PingPong				{ get; set; }
		public bool			DrawAboveAlwaysFront	{ get; set; }
	}

	public interface ISSDLight
	{
		public string	Color	{ get; set; }
		public float	Radius	{ get; set; }
	}

	public interface ISSCollisionBox
	{
		public float	X		{ get; set; }
		public float	Y		{ get; set; }
		public float	Width	{ get; set; }
		public float	Height	{ get; set; }
	}

	public interface ISSCondition
	{
		public string	Query	{ get; set; }
		public string	Update	{ get; set; }
	}

	public interface ISCondition
	{
		public string	Query			{ get; set; }
		public string	LockedMessage	{ get; set; }
		public string	Update			{ get; set; }
	}

	public interface ISEvent
	{
		public string		Type		{ get; set; }
		public string		Id			{ get; set; }
		public string		Script		{ get; set; }
		public string		Location	{ get; set; }
		public ISEFilter	Filter		{ get; set; }
	}

	public interface ISEFilter
	{
		IList<string>	IncludeStations	{ get; set; }
		IList<string>	ExcludeStations	{ get; set; }
		uint			TravelCount		{ get; set; }
	}

	public interface ISRequiredMod
	{
		string	Id				{ get; set; }
		string	MinimumVersion	{ get; set; }
		string	MaximumVersion	{ get; set; }
	}
}
