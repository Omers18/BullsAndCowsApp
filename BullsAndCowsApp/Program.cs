using BullsAndCowsApp.Models;

namespace BullsAndCowsApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PrintWelcome();
            Console.WriteLine();

            int codeLength = ChooseDifficulty();

            BullsAndCowsGame game = new BullsAndCowsGame(codeLength);
            int bestScore = 0;

            Console.WriteLine();
            Console.WriteLine("Enter your name");
            string playerName = Console.ReadLine();
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine($"Please enter a {game.CodeLength} digit number. 'n'=new game, 'q'=quit");

                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                else if (IsQuit(input))
                {
                    Console.WriteLine();
                    Console.WriteLine("Goodbye!");
                    break;
                }

                else if (IsNewGame(input))
                {
                    Console.WriteLine();

                    codeLength = ChooseDifficulty();
                    game.StartNewGame(codeLength);

                    Console.WriteLine();
                    Console.WriteLine("New game started.");

                    continue;
                }

                else if (game.IsGameOver)
                {
                    Console.WriteLine("The game is already over. Start a new game.");
                    continue;
                }

                else
                {
                    if (!BullsAndCowsGame.IsValidGuess(input, game.CodeLength,
                        out string errorMessage))
                    {
                        Console.WriteLine();
                        Console.WriteLine(errorMessage);
                        Console.WriteLine();
                        continue;
                    }

                    else
                    {
                        GuessResult result = game.SubmitGuess(input);

                        Console.WriteLine(
                            $"Your guess: {input}. Bulls: {result.Bulls}. Cows: {result.Cows}");

                        Console.WriteLine();

                        if (result.IsWinningGuess)
                        {
                            int attempts = game.AttemptCount;

                            Console.WriteLine(
                                $"Correct, {playerName}! You guessed the secret number in {attempts} attempts.");

                            if (bestScore == 0 || attempts < bestScore)
                            {
                                if (bestScore != 0)
                                {
                                    Console.WriteLine("New record achieved!");
                                }

                                bestScore = attempts;
                            }

                            Console.WriteLine(
                                $"Your current best score is {bestScore} attempts.");

                            Console.WriteLine(
                                "Type 'n' for a new game or 'q' to quit.");
                        }
                    }
                }
            }
        }

        static int ParseDifficulty(string input, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (input.Equals("1") || input.Equals("4") ||
                input.Equals("4 digits", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("easy", StringComparison.OrdinalIgnoreCase))
            {
                return 4;
            }
            else if (input.Equals("2") || input.Equals("5") ||
                     input.Equals("5 digits", StringComparison.OrdinalIgnoreCase) ||
                     input.Equals("medium", StringComparison.OrdinalIgnoreCase))
            {
                return 5;
            }
            else if (input.Equals("3") || input.Equals("6") ||
                     input.Equals("6 digits", StringComparison.OrdinalIgnoreCase) ||
                     input.Equals("hard", StringComparison.OrdinalIgnoreCase))
            {
                return 6;
            }

            errorMessage = "Error: Invalid difficulty. Please choose a difficulty from 1-3.";
            return -1;
        }

        static int ChooseDifficulty()
        {
            while (true)
            {
                Console.WriteLine("Choose difficulty:");
                Console.WriteLine("1 - Easy (4 digits)");
                Console.WriteLine("2 - Medium (5 digits)");
                Console.WriteLine("3 - Hard (6 digits)");

                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Error: Difficulty cannot be empty");
                    Console.WriteLine();
                    continue;
                }

                int codeLength = ParseDifficulty(input, out string errorMessage);

                if (codeLength != -1)
                {
                    return codeLength;
                }

                Console.WriteLine(errorMessage);
                Console.WriteLine();
            }
        }

        static void PrintWelcome()
        {
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("           BULLS AND COWS");
            Console.WriteLine(new string('=', 40));

            Console.WriteLine();
            Console.WriteLine("Try to guess the secret number. All digits are different.");
            Console.WriteLine("The number of digits depends on the difficulty you choose.");
            Console.WriteLine("Bulls = right digit, right position.");
            Console.WriteLine("Cows = right digit, wrong position.");
            Console.WriteLine();
        }

        static bool IsQuit(string input)
        {
            return input.Equals("q", StringComparison.OrdinalIgnoreCase)
                || input.Equals("quit", StringComparison.OrdinalIgnoreCase)
                || input.Equals("exit", StringComparison.OrdinalIgnoreCase);

        }

        static bool IsNewGame(string input)
        {
            return input.Equals("n", StringComparison.OrdinalIgnoreCase)
                || input.Equals("new", StringComparison.OrdinalIgnoreCase);
        }
    
    }
}
