using System;
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
					case "save":
						DataManager.Save();
						Console.WriteLine("Saved");
						modified = false;
						break;
				}
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
			Console.Write("aliases:");
			string aliases = Console.ReadLine();

			if (!db.AddPlayer(name, aliases))
			{
				return("Failed to add player");
			}

			modified = true;
			return("Added");
		}
	}
}
