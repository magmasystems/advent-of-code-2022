namespace AdventOfCode2022
{
    internal static class Program
    {
        private enum Weapon
        {
            Rock = 0,
            Paper = 1,
            Scissor = 2
        }
    
        private class Round
        {
            public Weapon Them { get; init; }
            public Weapon Me { get; init; }
        }

        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var listOfRounds = new List<Round>();
            foreach (var line in input)
            {
                var parts = line.Split(' ');
                listOfRounds.Add(new Round { Them = (Weapon) parts[0][0] - 'A', Me = (Weapon) parts[1][0] - 'X' });  // normalize each weapon to 0
            }
            
            Console.WriteLine($"Part 1: The total score is {PlayRound(listOfRounds)}");   // 14297
            Console.WriteLine($"Part 2: The total score is {PlayRound2(listOfRounds)}");  // 10498
        }

        private static int PlayRound(IEnumerable<Round> listOfRounds)
        {
            return listOfRounds.Sum(round => PlayOne(round.Me, round.Them));
        }
        
        private static int PlayRound2(IEnumerable<Round> listOfRounds)
        {
            // DesiredOutcome: X = lose, Y = draw, Z = win
            Dictionary<Weapon, List<Weapon>> MapDesiredOutcomeToWeapon = new()
            {
                { Weapon.Rock,    new List<Weapon> { Weapon.Scissor, Weapon.Rock,    Weapon.Paper   } },  // they have rock
                { Weapon.Paper,   new List<Weapon> { Weapon.Rock,    Weapon.Paper,   Weapon.Scissor } },  // they have paper
                { Weapon.Scissor, new List<Weapon> { Weapon.Paper,   Weapon.Scissor, Weapon.Rock    } },  // they have scissors
            };

            return listOfRounds.Sum(round => PlayOne(MapDesiredOutcomeToWeapon[round.Them][(int) round.Me], round.Them));
        }

        private static int PlayOne(Weapon myWeapon, Weapon theirWeapon)
        {
            int score;
            if (myWeapon == theirWeapon)
            {
                score = 3; // same weapon
            }
            else if (myWeapon == Weapon.Rock && theirWeapon == Weapon.Scissor ||  // rock defeats scissors
                     myWeapon == Weapon.Paper && theirWeapon == Weapon.Rock   ||  // paper defeats rock
                     myWeapon == Weapon.Scissor && theirWeapon == Weapon.Paper)   // scissors defeats paper
            {
                score = 6;
            }
            else
            {
                score = 0; // they beat me
            }

            score += (int) myWeapon + 1;
            return score;
        }
    }
}