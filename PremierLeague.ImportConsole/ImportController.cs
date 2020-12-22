using PremierLeague.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace PremierLeague.ImportConsole
{
    public static class ImportController
    {
        const int Idx_Round = 0;
        const int Idx_HomeTeam = 1;
        const int Idx_AwayTeam = 2;
        const int Idx_HomeGoals = 3;
        const int Idx_AwayGoals = 4;
        
        
        public async static Task<IEnumerable<Game>> ReadFromCsvAsync()
        {
            string[][] matrix = await MyFile.ReadStringMatrixFromCsvAsync("PremierLeague.csv", false);  // keine Titelzeile
                                                                                                        // Einlesen der Spiele und der Teams
                                                                                                        // Zuerst die Teams

            //1; Manchester United; Tottenham Hotspur; 1; 0
            //1; AFC Bournemouth; Aston Villa; 0; 1

            var teams = matrix.Select(_ => $"{_[1]}~{_[2]}")
                .SelectMany(_ => _.Split('~'))
                .Distinct()
                .Select(_ => new Team
                {
                    Name = _
                })
                .ToDictionary(_ => _.Name);

            var games = matrix.Select(_ => new Game
            {
                Round = int.Parse(_[Idx_Round]),
                HomeTeam = teams[_[Idx_HomeTeam]],
                GuestTeam = teams[_[Idx_AwayTeam]],
                HomeGoals = int.Parse(_[Idx_HomeGoals]),
                GuestGoals = int.Parse(_[Idx_AwayGoals])
            })
            .ToList();

            return games;
        }

    }
}
