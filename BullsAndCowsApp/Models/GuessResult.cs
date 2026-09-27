using System;
using System.Collections.Generic;
using System.Text;

namespace BullsAndCowsApp.Models
{
    internal class GuessResult //התוצאות של ניחוש בודד
    {
        private readonly int _codeLength;

        public GuessResult(int codeLength)
        {
            _codeLength = codeLength;
        }

        public int Bulls { get; set; }
        public int Cows { get; set; }
        public bool IsWinningGuess 
        {
            get { return Bulls == _codeLength; }
             
        }
    }
}
