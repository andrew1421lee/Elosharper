using System;
using System.IO;
using EloSharper.database;

namespace EloSharper
{
	class MainClass
	{
		private static Database db;
		private static bool modified = false;
		public static void Main(string[] args)
		{
			db = new Database();
			while (true)
			{
				Console.Write(">>");
				string input = Console.ReadLine();
				switch (input)
				{
					case "exit":
						Exit();
						break;
					case "add player":
						Console.WriteLine(AddPlayer());
						break;
					case "edit player":
						Console.WriteLine(EditPlayer());
						break;
					case "delete player":
						Console.WriteLine(DeletePlayer());
						break;
					case "list players":
						Console.WriteLine(db.ListPlayers());
						break;
					case "add game":
						Console.WriteLine(AddGame());
						break;
					case "delete game":
						Console.WriteLine(DeleteGame());
						break;
					case "edit game":
						Console.WriteLine(EditGame());
						break;
					case "list games":
						Console.WriteLine(db.ListGames());
						break;
					case "save":
						DataManager.Save();
						Console.WriteLine("Saved");
						modified = false;
						break;
					case "elotest":
						Console.WriteLine(EloTest());
						break;
					case "calculate elo":
						Console.WriteLine(CalculateElo());
						break;
					case "load players":
						Console.WriteLine(LoadPlayersFromFile());
						break;
					case "load games":
						Console.WriteLine(LoadGamesFromFile());
						break;
				}
			}
		}

		public static string LoadPlayersFromFile()
		{
			Console.Write("file:");
			string filelocation = Console.ReadLine();
			if (!File.Exists(filelocation))
			{
				return "File does not exist";
			}
			string[] lines = File.ReadAllLines(filelocation);
			foreach (string line in lines)
			{
				string[] playerinfo = line.Split(new char[0]);
				if (!db.AddPlayer(playerinfo[0], playerinfo[1]))
				{
					return "Error reading from file";
				}
			}
			modified = true;
			return "Players loaded from file";
		}

		public static string LoadGamesFromFile()
		{
			Console.Write("file:");
			string filelocation = Console.ReadLine();
			if (!File.Exists(filelocation))
			{
				return "File does not exist";
			}
			string[] lines = File.ReadAllLines(filelocation);
			foreach (string line in lines)
			{
				string[] gameinfo = line.Split(new char[0]);
				if (!db.AddGame(gameinfo[0], gameinfo[1], gameinfo[2], Int32.Parse(gameinfo[3])))
				{
					return "Error reading from file";
				}
			}
			modified = true;
			return "Games loaded from file";
		}

		public static string CalculateElo()
		{
			db.CalculateEloForAllGames();
			return "Elo Calculated";
		}

		public static string EloTest()
		{
			Console.Write("p1 elo:");
			float p1 = float.Parse(Console.ReadLine());
			Console.Write("p2 elo:");
			float p2 = float.Parse(Console.ReadLine());
			Console.Write("winner:");
			int winner = Int32.Parse(Console.ReadLine());
			double[] result = EloManager.CalculateElo(p1, p2, winner);
			return $"p1:{result[0]}, p2:{result[1]}";
		}

		public static string EditGame()
		{
			Console.Write("index of game:");
			int index = Int32.Parse(Console.ReadLine());

			Console.Write("datetime:");
			string datetime = Console.ReadLine();
			if (datetime.Length < 1)
			{
				datetime = null;
			}
			Console.Write("player 1:");
			string p1_name = Console.ReadLine();
			if (p1_name.Length < 1)
			{
				p1_name = null;
			}
			Console.Write("player 2:");
			string p2_name = Console.ReadLine();
			if (p2_name.Length < 1)
			{
				p2_name = null;
			}
			Console.Write("winner (0, 1, or 2)");
			int winner = Int32.Parse(Console.ReadLine());
			if (winner > 2 || winner < 0)
			{
				winner = -1;
			}
			if (db.EditGame(index, datetime, p1_name, p2_name, winner))
			{
				return "Game edited";
			}
			else {
				return "Edit failed";
			}
		}

		public static string DeleteGame()
		{
			Console.Write("index:");
			int index = Int32.Parse(Console.ReadLine());
			if (db.DeleteGame(index))
			{
				modified = true;
				return "Game deleted";
			}
			else {
				return "Delete failed";
			}
		}

		public static string AddGame()
		{
			Console.Write("date:");
			string date = Console.ReadLine();
			if (date.Length < 1)
			{
				return "Invalid input";
			}
			Console.Write("time:");
			string time = Console.ReadLine();
			if (time.Length < 1)
			{
				return "Invalid input";
			}
			Console.Write("player 1:");
			string p1_name = Console.ReadLine();
			if (p1_name.Length < 1)
			{
				return "Invalid input";
			}
			Console.Write("player 2:");
			string p2_name = Console.ReadLine();
			if (p2_name.Length < 1)
			{
				return "Invalid input";
			}
			Console.Write("winner (0, 1, or 2):");
			int winner = Int32.Parse(Console.ReadLine());
			if (winner > 2 || winner < 0)
			{
				return "Invalid input";
			}
			if (db.AddGame($"{date} {time}", p1_name, p2_name, winner))
			{
				modified = true;
				return "Game Added";
			}
			else {
				return "Add failed";
			}
		}

		public static string EditPlayer()
		{
			Console.Write("Name of player to edit:");
			string oldname = Console.ReadLine();
			Console.Write("new name:");
			string name = Console.ReadLine();
			if (name.Length < 1)
			{
				name = null;
			}
			Console.Write("new aliases:");
			string alias = Console.ReadLine();
			if (alias.Length < 1)
			{
				alias = null;
			}

			if (db.EditPlayer(oldname, name, alias, -1, -1, -1, -1))
			{
				modified = true;
				return "Player editted";
			}
			else {
				return "Edit failed";
			}
		}

		public static string DeletePlayer()
		{
			Console.Write("name:");
			if (db.RemovePlayerByName(Console.ReadLine()))
			{
				return "Player deleted";
			}
			return "Delete failed";
		}

		public static void Exit()
		{
			if (modified)
			{
				Console.WriteLine("There are unsaved changes, continue? (Y/N)");
				if (Console.ReadKey().Key == ConsoleKey.Y)
				{
					System.Environment.Exit(1);
				}
			}
			else {
				System.Environment.Exit(1);
			}
		}
		public static string AddPlayer()
		{
			Console.Write("name:");
			string name = Console.ReadLine();
			if (name.Length < 1)
			{
				return "Invalid input";
			}
			Console.Write("aliases:");
			string aliases = Console.ReadLine();
			if (aliases.Length < 1)
			{
				return "Invalid input";
			}

			if (!db.AddPlayer(name, aliases))
			{
				return("Failed to add player");
			}

			modified = true;
			return("Added");
		}
	}
}
