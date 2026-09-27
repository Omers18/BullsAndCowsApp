using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BullsAndCowsApp.Models
{
    internal class BullsAndCowsGame
    {
        public int CodeLength { get; private set; }
        private Random _random;
        private string _secretNumber;
        public int AttemptCount { get; private set; }
        public bool IsGameOver { get; private set; }

        public BullsAndCowsGame(int codeLength)
        {
            _random = new Random();
            StartNewGame(codeLength);
        }

        public void StartNewGame(int codeLength)
        {
            if (codeLength < 4 || codeLength > 6)
            {
                throw new ArgumentException("Code length must be between 4 and 6.");
            }

            CodeLength = codeLength;
            _secretNumber = GenerateSecretNumber();
            AttemptCount = 0;
            IsGameOver = false;
        }

        private string GenerateSecretNumber()
        {
            string number = string.Empty;
            List<int> digits = new List<int>();

            for (int i = 0; i < 10; i++)
                digits.Add(i);

            for (int i = 0; i < digits.Count - 1; i++)
            {
                int randomIndex = _random.Next(i, digits.Count);

                int temp = digits[i];
                digits[i] = digits[randomIndex];
                digits[randomIndex] = temp;
            }

            if (digits[0] == 0)
            {
                int randomIndex = _random.Next(1, digits.Count);
                digits[0] = digits[randomIndex];
                digits[randomIndex] = 0;
            }

            for (int i = 0; i < CodeLength; i++)
            {
                number += digits[i];
            }

            return number;
        }

        public static bool IsValidGuess(string guess, int codeLength, out string errorMessage)
        {

            errorMessage = $"Error: Please enter exactly {codeLength} digits.";
            if (guess == null || guess.Length != codeLength)
            {
                return false;
            }

            for (int i = 0; i < guess.Length; i++) 
            {
                if (guess[i] < '0' || guess[i] > '9')
                {
                    return false;
                }
            }


            if (guess[0] == '0')
            {
                errorMessage = $"Error: The first digit must be different from '0'";
                return false;
            }

            for (int i = 0; i < guess.Length - 1; i++)
            {
                char curr = guess[i];
                for (int j = i + 1; j < guess.Length; j++)
                {
                    if (curr == guess[j])
                    {
                        errorMessage = "Error: All digits must be different from each other.";
                        return false;
                    }
                }
            }
            errorMessage = string.Empty;
            return true;
        }

        public GuessResult SubmitGuess(string guess) 
        {
            if (IsGameOver)
            {
                throw new InvalidOperationException("The game is already over.");
            }
            int bulls = 0;
            int cows = 0;
            GuessResult result = new GuessResult(CodeLength);

            for (int i = 0; i < CodeLength; i++)
            {
                for (int j = 0; j < CodeLength; j++)
                {
                    if (guess[j] == _secretNumber[i])
                    {
                        if (j == i)
                        {
                            bulls++;
                        }
                        else
                        {
                            cows++;
                        }
                        break;
                    }
                }
            }
            result.Bulls = bulls;
            result.Cows = cows;
            AttemptCount++;
            if (result.IsWinningGuess) { IsGameOver = true; }
            return result;
        }
    }
}
