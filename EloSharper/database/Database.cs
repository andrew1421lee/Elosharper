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

		/// <summary>
		/// Finds the index of the player by name.
		/// </summary>
		/// <returns>The player index.</returns>
		/// <param name="name">Name.</param>
		private int FindPlayerByName(string name)
		{
			foreach (var plr in DataManager.Data.Players)
			{
				if (plr.name == name)
				{
					return plr.index;
				}
				foreach (string alias in plr.aliases)
				{
					if (alias == name)
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
