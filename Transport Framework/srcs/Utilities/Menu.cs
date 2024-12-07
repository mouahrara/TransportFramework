using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using TransportFramework.Classes;

namespace TransportFramework.Utilities
{
	internal class MenuUtility
	{
		private static readonly PerScreen<IEnumerable<Station>>	stations = new(() => null);
		private static readonly PerScreen<Station>				station = new(() => null);
		private static readonly PerScreen<int>					startingIndex = new(() => 0);

		private static void Reset()
		{
			Stations = null;
			Station = null;
			StartingIndex = 0;
		}

		public static IEnumerable<Station> Stations
		{
			get => stations.Value;
			set => stations.Value = value;
		}

		public static Station Station
		{
			get => station.Value;
			set => station.Value = value;
		}

		private static int StartingIndex
		{
			get => startingIndex.Value;
			set => startingIndex.Value = value;
		}

		private static void Initialize(Station station)
		{
			StartingIndex = 0;
			Station = station;
			Stations = ModEntry.Stations?
							.Where(s => s.Id != Station.Id
								&& (
									(s.Network == Station.Network
										&& (Station.Exclude is null || (!Station.Exclude.Contains(s.Id) && !Station.Exclude.Contains("all")))
										&& (Station.ExcludeDeparture is null || (!Station.ExcludeDeparture.Contains(s.Id) && !Station.ExcludeDeparture.Contains("all")))
										&& (s.Exclude is null || (!s.Exclude.Contains(Station.Id) && !s.Exclude.Contains("all")))
										&& (s.ExcludeArrival is null || (!s.ExcludeArrival.Contains(Station.Id) && !s.ExcludeArrival.Contains("all"))))
									|| (Station.Include is not null && (Station.Include.Contains(s.Id) || Station.Include.Contains("all")))
									|| (Station.IncludeDeparture is not null && (Station.IncludeDeparture.Contains(s.Id) || Station.IncludeDeparture.Contains("all")))
									|| (s.Include is not null && (s.Include.Contains(Station.Id) || s.Include.Contains("all")))
									|| (s.IncludeArrival is not null && (s.IncludeArrival.Contains(Station.Id) || s.IncludeArrival.Contains("all")))
								)
								&& (
									s.ConditionsCache
									|| (Station.IgnoreConditions is not null && (Station.IgnoreConditions.Contains(s.Id) || Station.IgnoreConditions.Contains("all")))
									|| (Station.IgnoreConditionsDeparture is not null && (Station.IgnoreConditionsDeparture.Contains(s.Id) || Station.IgnoreConditionsDeparture.Contains("all")))
									|| (s.IgnoreConditions is not null && (s.IgnoreConditions.Contains(Station.Id) || s.IgnoreConditions.Contains("all")))
									|| (s.IgnoreConditionsArrival is not null && (s.IgnoreConditionsArrival.Contains(Station.Id) || s.IgnoreConditionsArrival.Contains("all")))
								)
							);
		}

		public static bool TryToOpen(Station station)
		{
			StationsUtility.UpdateOnInteract();
			if (!station.ConditionsCache)
			{
				if (!string.IsNullOrWhiteSpace(station.ConditionsLockedMessageCache))
				{
					Game1.drawObjectDialogue(station.ConditionsLockedMessageCache);
				}
				return false;
			}
			Open(station);
			return true;
		}

		private static void Open(Station station)
		{
			Initialize(station);
			if (!Stations.Any())
			{
				Reset();
				Game1.drawObjectDialogue(ModEntry.Helper.Translation.Get("Menu.NoDestinations"));
				return;
			}
			CreateQuestionDialogue();
		}

		public static void OnResponse(Farmer who, string whichAnser)
		{
			if (whichAnser.Equals("previousPage"))
			{
				StartingIndex -= ModEntry.Config.NumberOfDestinationsPerPage;
				CreateQuestionDialogue();
			}
			else if (whichAnser.Equals("nextPage"))
			{
				StartingIndex += ModEntry.Config.NumberOfDestinationsPerPage;
				CreateQuestionDialogue();
			}
			else if (whichAnser.Equals("cancel"))
			{
				Reset();
			}
			else
			{
				WarpUtility.TryToWarp(Station, ModEntry.Stations.Find(s => s.Id == whichAnser));
				Reset();
			}
		}

		private static void CreateQuestionDialogue()
		{
			Game1.activeClickableMenu?.emergencyShutDown();
			DelayedAction.functionAfterDelay(() =>
			{
				Game1.currentLocation?.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination"), GetAnswerChoices(), OnResponse);
				(Game1.activeClickableMenu as DialogueBox).safetyTimer = 0;
			}, 0);
		}

		private static Response[] GetAnswerChoices()
		{
			List<Response> answerChoices = new();
			int pageIndex = (StartingIndex / ModEntry.Config.NumberOfDestinationsPerPage) + 1;
			int pageCount = (Stations.Count() + ModEntry.Config.NumberOfDestinationsPerPage - 1) / ModEntry.Config.NumberOfDestinationsPerPage;

			StartingIndex = (pageIndex - 1) * ModEntry.Config.NumberOfDestinationsPerPage;
			AddPreviousPageAndNextPageAnswers(answerChoices, pageIndex, pageCount, "Top");
			for (int i = StartingIndex; i < StartingIndex + ModEntry.Config.NumberOfDestinationsPerPage && i < Stations.Count(); i++)
			{
				answerChoices.Add(new Response(Stations.ElementAt(i).Id, Stations.ElementAt(i).LocalizedDisplayName + (Stations.ElementAt(i).Price > 0 ? $" ({Stations.ElementAt(i).Price}$)" : "")));
			}
			AddPreviousPageAndNextPageAnswers(answerChoices, pageIndex, pageCount, "Bottom");
			answerChoices.Add(new Response("cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel")));
			return answerChoices.ToArray();
		}

		private static void AddPreviousPageAndNextPageAnswers(List<Response> answerChoices, int pageIndex, int pageCount, string position)
		{
			if (ModEntry.Config.PreviousPageButtonPosition == position)
			{
				AddPreviousPageAnswer(answerChoices, pageIndex, pageCount);
			}
			if (ModEntry.Config.NextPageButtonPosition == position)
			{
				AddNextPageAnswer(answerChoices, pageIndex, pageCount);
			}
		}

		private static void AddPreviousPageAnswer(List<Response> answerChoices, int pageIndex, int pageCount)
		{
			if (pageIndex > 1)
			{
				answerChoices.Add(new Response("previousPage", $"@ {Game1.content.LoadString("Strings\\UI:PreviousPage")} ({pageIndex - 1}/{pageCount})"));
			}
		}

		private static void AddNextPageAnswer(List<Response> answerChoices, int pageIndex, int pageCount)
		{
			if (pageIndex < pageCount)
			{
				answerChoices.Add(new Response("nextPage", $"{Game1.content.LoadString("Strings\\UI:NextPage")} ({pageIndex + 1}/{pageCount}) >"));
			}
		}
	}
}
