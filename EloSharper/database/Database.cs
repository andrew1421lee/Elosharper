using System;
using System.Collections.Generic;

namespace EloSharper.database
{
	public class Database
	{
		public Database()
		{
			DataManager.Load();
		}

		/// <summary>
		/// Adds a new player.
		/// </summary>
		/// <returns><c>true</c>, if player was added, <c>false</c> otherwise.</returns>
		/// <param name="name">Name.</param>
		/// <param name="alias">Alias.</param>
		public bool AddPlayer(string name, string alias)
		{

			if (FindPlayerByName(name) < 0)
			{
				var NewPlayer = new DataManager.Player();
				NewPlayer.name = name;
				NewPlayer.aliases = new List<string>(alias.Split(new char[0]));
				NewPlayer.draws = 0;
				NewPlayer.index = DataManager.Data.Players.Count;
				NewPlayer.id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
				NewPlayer.rating = 1000;
				NewPlayer.totalgames = 0;
				NewPlayer.wins = 0;
				DataManager.Data.Players.Add(NewPlayer);
				return true;
			}
			else {
				return false;
			}
		}

		public bool EditPlayer(string oldname, string name, string alias, int draws, int rating, int totalgames, int wins)
		{
			int PlayerIndex = FindPlayerByName(oldname);
			if (PlayerIndex < 0)
			{
				return false;	
			}
			else {
				if (name != null)
				{
					DataManager.Data.Players[PlayerIndex].name = name;
				}
				if (alias != null)
				{
					DataManager.Data.Players[PlayerIndex].aliases = new List<string>(alias.Split(new char[0]));
				}
				if (draws != -1)
				{
					DataManager.Data.Players[PlayerIndex].draws =  draws;
				}
				if (rating != -1)
				{
					DataManager.Data.Players[PlayerIndex].rating = rating;
				}
				if (totalgames != -1)
				{
					DataManager.Data.Players[PlayerIndex].totalgames = totalgames;
				}
				if (wins != -1)
				{
					DataManager.Data.Players[PlayerIndex].wins = wins;
				}
				return true;
			}
		}

		/// <summary>
		/// Lists the players.
		/// </summary>
		/// <returns>The players.</returns>
		public string ListPlayers()
		{
			
			string returnval = "";
			foreach (var plr in DataManager.Data.Players)
			{
				string aliases = "";
				foreach (string nickname in plr.aliases)
				{
					aliases += $"{nickname},";
				}
				returnval += $"{plr.index}: {plr.name} aka. {aliases} | Total games: {plr.totalgames}. Wins: {plr.wins}\n";
			}
			return returnval;

		}

		/// <summary>
		/// Removes a player by name.
		/// </summary>
		/// <returns><c>true</c>, if player was removed, <c>false</c> otherwise.</returns>
		/// <param name="name">Name.</param>
		public bool RemovePlayerByName(string name)
		{
			int IndexOfPlayer = FindPlayerByName(name);
			if (IndexOfPlayer < 0)
			{
				return false;
			}
			else {
				DataManager.Data.Players.RemoveAt(IndexOfPlayer);
				RebuildPlayerIndex();
				return true;
			}
		}

		/// <summary>
		/// Rebuilds the index of player objects. Call after removal of a player.
		/// </summary>
		private void RebuildPlayerIndex()
		{
			int index = 0;
			foreach (var plr in DataManager.Data.Players)
			{
				plr.index = index;
				index++;
			}
		}

		/// <summary>
		/// Finds the index of the player by name.
		/// </summary>
		/// <returns>The player index.</returns>
		/// <param name="name">Name.</param>
		private int FindPlayerByName(string name)
		{
			foreach (var plr in DataManager.Data.Players)
			{
				if (plr.name.ToUpper() == name.ToUpper())
				{
					return plr.index;
				}
				foreach (string alias in plr.aliases)
				{
					if (alias.ToUpper() == name.ToUpper())
					{
						return plr.index;
					}
				}
			}
			return -1;
		}

		/// <summary>
		/// Finds the player index by identifier.
		/// </summary>
		/// <returns>The player index.</returns>
		/// <param name="id">Identifier.</param>
		private int FindPlayerByID(string id)
		{
			foreach (var plr in DataManager.Data.Players)
			{
				if (plr.id == id)
				{
					return plr.index;
				}
			}
			return -1;
		}

	}
}
