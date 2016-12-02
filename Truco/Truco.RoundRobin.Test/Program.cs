using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truco.Models;

namespace Truco.RoundRobin.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Robin> equipes = new List<Robin>()
            {
                new Robin { Equipe = "Equipe 1" },
                new Robin { Equipe = "Equipe 2" },
                new Robin { Equipe = "Equipe 3" },
                new Robin { Equipe = "Equipe 4" },
                new Robin { Equipe = "Equipe 5" },
                new Robin { Equipe = "Equipe 6" },
            };

            Truco.Infraestrutura.RoundRobin roundRobin = new Truco.Infraestrutura.RoundRobin();
            int num_teams = equipes.Count();
            int[,] results = roundRobin.GenerateRoundRobin(num_teams);

            // Display the result.
            string txt = "";
            for (int round = 0; round <= results.GetUpperBound(1); round++)
            {
                txt += "Round " + round + ":\r\n";
                for (int team = 0; team < num_teams; team++)
                {
                    if (results[team, round] == Truco.Infraestrutura.RoundRobin.BYE)
                    {
                        txt += "    " + equipes[team].Equipe + " (bye)\r\n";
                    }
                    else if (team < results[team, round])
                    {
                        txt += "    " + equipes[team].Equipe + " v " + equipes[results[team, round]].Equipe + "\r\n";
                    }
                }
            }

            Console.WriteLine(txt);

            Console.ReadKey();
        }
    }

    public class Robin
    {
        public int Numero { get; set; }
        public string Equipe { get; set; }
    }

}
