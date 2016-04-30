using System;
using System.IO;
using System.Collections.Generic;
using Moserware.Skills;
using Moserware.Skills.TrueSkill;

namespace main
{
	class MainClass
	{

		public static void Main (string[] args)
		{
            var calculator = new TwoTeamTrueSkillCalculator();
            var gameInfo = GameInfo.DefaultGameInfo;

			var ratings = new Dictionary<object, Rating>();
			var lines = File.ReadLines(args[0]);
			foreach (var line in lines)
			{
				var newline = line;
				string[] tokens = line.Split(',');
				for (int i = 1; i <= 22; i++)
				{
					if (!ratings.ContainsKey(tokens[i]))
					{
					   ratings[tokens[i]] = gameInfo.DefaultRating;
					}
				}
				var players = new List<Player>();
				var team1 = new Team();
				for (int i = 1; i <= 11; i++)
				{
					var player = new Player(tokens[i]);
					team1.AddPlayer(player, ratings[tokens[i]]);
					players.Add(player);
				}
				var team2 = new Team();
				for (int i = 12; i <= 22; i++)
				{
					var player = new Player(tokens[i]);
					team2.AddPlayer(player, ratings[tokens[i]]);
					players.Add(player);
				}
              	var teams = Teams.Concat(team1, team2);
				var rank1 = 1;
				var rank2 = 1;
				if (tokens[23] == "-1")
				{
					rank1 = 1;
					rank2 = 2;
				}
				if (tokens[23] == "1")
				{
					rank1 = 2;
					rank2 = 1;
				}
				var newRatings = calculator.CalculateNewRatings(gameInfo, teams, rank1, rank2);
				foreach (var player in players)
				{
					ratings[player.Id] = newRatings[player];
					newline = newline + "," + newRatings [player].Mean;
				}
				Console.WriteLine (newline);
			}
		}
	}
}
