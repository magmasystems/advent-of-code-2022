namespace AdventOfCode2022
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // This is part 1
            
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            var listOfCalories = new List<int>();
            
            var sum = 0;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    listOfCalories.Add(sum);
                    sum = 0;
                }
                else
                {
                    sum += Convert.ToInt32(line);
                }
            }

            var max = -1;
            var idxMax = -1;
            for (var i = 0; i < listOfCalories.Count; i++)
            {
                if (listOfCalories[i] > max)
                {
                    idxMax = i;
                    max = listOfCalories[i];
                }
            }
            
            Console.WriteLine($"Part 1: The max calories is {max} and the elf that had this is elf number {idxMax+1}");  // 68467
            
            // This is part 2
            listOfCalories.Sort();
            sum = listOfCalories.TakeLast(3).Sum();
            Console.WriteLine($"Part 2: The total calories carried by the top 3 elves is {sum}");  // 203420
        }
    }
}
