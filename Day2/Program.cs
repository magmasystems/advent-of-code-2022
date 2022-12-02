namespace AdventOfCode2022
{
    internal static class Program
    {
        private class Round
        {
            public int Them { get; init; }
            public int Me { get; init; }
        }

        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var listOfRounds = new List<Round>();
            foreach (var line in input)
            {
                var parts = line.Split(' ');
                listOfRounds.Add(new Round { Them = parts[0][0] - 'A', Me = parts[1][0] - 'X' });  // normalize each weapon to 0
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
            Dictionary<int, List<int>> MapDesiredOutcomeToWeapon = new()
            {
                { 0, new List<int> { 2, 0, 1 } },  // they have rock
                { 1, new List<int> { 0, 1, 2 } },  // they have paper
                { 2, new List<int> { 1, 2, 0 } },  // they have scissors
            };

            return listOfRounds.Sum(round => PlayOne(MapDesiredOutcomeToWeapon[round.Them][round.Me], round.Them));
        }

        private static int PlayOne(int myWeapon, int theirWeapon)
        {
            int score;
            if (myWeapon == theirWeapon)
            {
                score = 3; // same weapon
            }
            else if (myWeapon == 0 && theirWeapon == 2 || // rock defeats scissors
                     myWeapon == 1 && theirWeapon == 0 || // paper defeats rock
                     myWeapon == 2 && theirWeapon == 1)   // scissors defeats paper
            {
                score = 6;
            }
            else
            {
                score = 0; // they beat me
            }

            score += myWeapon + 1;
            return score;
        }
    }
}