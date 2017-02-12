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

		public string ListGames()
		{
			string returnval = "";
			foreach (var game in DataManager.Data.Games)
			{
				string players = "";
				players += $"[{game.DateTime}] ";
				players += $"{DataManager.Data.Players[FindPlayerByID(game.Players[0])].name} vs. ";
				players += $"{DataManager.Data.Players[FindPlayerByID(game.Players[1])].name} - ";
				players += $"Winner: {game.winner + 1}";
				returnval += $"{players} \n";
			}
			return returnval;
		}
		/// <summary>
		/// Edits a game given by index.
		/// </summary>
		/// <returns><c>true</c>, if game was edited, <c>false</c> otherwise.</returns>
		/// <param name="index">Index.</param>
		/// <param name="datetime">Datetime.</param>
		/// <param name="p1">P1.</param>
		/// <param name="p2">P2.</param>
		/// <param name="winner">Winner.</param>
		public bool EditGame(int index, string datetime, string p1, string p2, int winner)
		{
			if (DataManager.Data.Games.Count <= index)
			{
				return false;
			}

			if (datetime != null)
			{
				DataManager.Data.Games[index].DateTime = datetime;
			}
			if (p1 != null)
			{
				if (FindPlayerByName(p1) > 0)
				{
					DataManager.Data.Games[index].Players.RemoveAt(0);
					DataManager.Data.Games[index].Players.Insert(0, DataManager.Data.Players[FindPlayerByName(p1)].id);
				}
			}
			if (p2 != null)
			{
				if (FindPlayerByName(p2) > 0)
				{
					DataManager.Data.Games[index].Players.RemoveAt(1);
					DataManager.Data.Games[index].Players.Insert(1, DataManager.Data.Players[FindPlayerByName(p2)].id);
				}
			}
			if (winner > 0)
			{
				DataManager.Data.Games[index].winner = winner;
			}

			return true;
		}

		/// <summary>
		/// Deletes a game.
		/// </summary>
		/// <returns><c>true</c>, if game was deleted, <c>false</c> otherwise.</returns>
		/// <param name="index">Index.</param>
		public bool DeleteGame(int index)
		{
			if (index >= DataManager.Data.Games.Count)
			{
				return false;
			}
			var Target = DataManager.Data.Games[index];
			MinusUpdateGameCounts(Target.Players[0], Target.Players[1], Target.winner);
			DataManager.Data.Games.RemoveAt(index);
			return true;
		}

		/// <summary>
		/// Adds a game.
		/// </summary>
		/// <returns><c>true</c>, if game was added, <c>false</c> otherwise.</returns>
		/// <param name="datetime">Datetime.</param>
		/// <param name="p1">P1.</param>
		/// <param name="p2">P2.</param>
		/// <param name="winner">Winner.</param>
		public bool AddGame(string datetime, string p1, string p2, int winner)
		{
			var NewGame = new DataManager.Game();
			NewGame.DateTime = datetime;
			NewGame.Players = new List<string>();
			NewGame.index = DataManager.Data.Games.Count;
			NewGame.id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
			try
			{
				NewGame.Players.Add(DataManager.Data.Players[FindPlayerByName(p1)].id);
				NewGame.Players.Add(DataManager.Data.Players[FindPlayerByName(p2)].id);
			}
			catch (IndexOutOfRangeException)
			{
				return false;
			}
			NewGame.winner = winner;

			AddUpdateGameCounts(p1, p2, winner);
			//AdjustElo(DataManager.Data.Players[FindPlayerByName(p1)], DataManager.Data.Players[FindPlayerByName(p2)], winner);

			DataManager.Data.Games.Add(NewGame);

			return true;
		}

		/// <summary>
		/// Updates the game counts for game add.
		/// </summary>
		/// <param name="p1">P1.</param>
		/// <param name="p2">P2.</param>
		/// <param name="winner">Winner.</param>
		private void AddUpdateGameCounts(string p1, string p2, int winner)
		{
			switch (winner)
			{
				case 0:
					DataManager.Data.Players[FindPlayerByName(p1)].wins++;
					DataManager.Data.Players[FindPlayerByName(p1)].totalgames++;
					DataManager.Data.Players[FindPlayerByName(p2)].totalgames++;
					break;
				case 1:
					DataManager.Data.Players[FindPlayerByName(p2)].wins++;
					DataManager.Data.Players[FindPlayerByName(p2)].totalgames++;
					DataManager.Data.Players[FindPlayerByName(p1)].totalgames++;
					break;
				case 2:
					DataManager.Data.Players[FindPlayerByName(p1)].draws++;
					DataManager.Data.Players[FindPlayerByName(p1)].totalgames++;
					DataManager.Data.Players[FindPlayerByName(p2)].totalgames++;
					DataManager.Data.Players[FindPlayerByName(p2)].draws++;
					break;
			}
		}
		/// <summary>
		/// Updates the game counts for game removal.
		/// </summary>
		/// <param name="p1">P1.</param>
		/// <param name="p2">P2.</param>
		/// <param name="winner">Winner.</param>
		private void MinusUpdateGameCounts(string p1, string p2, int winner)
		{
			switch (winner)
			{
				case 0:
					DataManager.Data.Players[FindPlayerByID(p1)].wins--;
					DataManager.Data.Players[FindPlayerByID(p1)].totalgames--;
					DataManager.Data.Players[FindPlayerByID(p2)].totalgames--;
					break;
				case 1:
					DataManager.Data.Players[FindPlayerByID(p2)].wins--;
					DataManager.Data.Players[FindPlayerByID(p2)].totalgames--;
					DataManager.Data.Players[FindPlayerByID(p1)].totalgames--;
					break;
				case 2:
					DataManager.Data.Players[FindPlayerByID(p1)].draws--;
					DataManager.Data.Players[FindPlayerByID(p1)].totalgames--;
					DataManager.Data.Players[FindPlayerByID(p2)].totalgames--;
					DataManager.Data.Players[FindPlayerByID(p2)].draws--;
					break;
			}
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
				NewPlayer.rating = 1000.00;
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
		/// Edits the player.
		/// </summary>
		/// <returns><c>true</c>, if player was edited, <c>false</c> otherwise.</returns>
		/// <param name="oldname">Oldname.</param>
		/// <param name="name">Name.</param>
		/// <param name="alias">Alias.</param>
		/// <param name="draws">Draws.</param>
		/// <param name="rating">Rating.</param>
		/// <param name="totalgames">Totalgames.</param>
		/// <param name="wins">Wins.</param>
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

		public void CalculateEloForAllGames()
		{
			foreach (var game in DataManager.Data.Games)
			{
				AdjustElo(DataManager.Data.Players[FindPlayerByID(game.Players[0])], DataManager.Data.Players[FindPlayerByID(game.Players[1])], game.winner);
			}
		}

		public void AdjustElo(DataManager.Player P1, DataManager.Player P2, int winner)
		{
			double[] NewElo = EloManager.CalculateElo(P1.rating, P2.rating, winner);
			EditPlayerElo(P1.id, NewElo[0]);
			EditPlayerElo(P2.id, NewElo[1]);
		}

		/// <summary>
		/// Edits the player elo.
		/// </summary>
		/// <returns><c>true</c>, if player elo was edited, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="elo">Elo.</param>
		public bool EditPlayerElo(string id, double elo)
		{
			int index = FindPlayerByID(id);
			if (index < 0)
			{
				return false;
			}
			DataManager.Data.Players[index].rating = elo;
			return true;
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
				returnval += $"{plr.index}: {plr.name} aka. {aliases} | ELO: {plr.rating.ToString("#.00")}. Total games: {plr.totalgames}. Wins: {plr.wins}\n";
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
