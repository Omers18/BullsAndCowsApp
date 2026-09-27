using System;
using System.Collections.Generic;
using System.Text;

namespace BullsAndCowsApp.Models
{
    internal class GuessRecord // רשומה של ניחוש אחד בהיסטוריה
    {
        public int AttemptNumber { get; set; }
        public string Guess { get; set; }
        public int Bulls { get; set; }
        public int Cows { get; set; }
        public string Feedback 
        {
            get 
            {
                return $"{Bulls} Bulls, {Cows} Cows";
            } 
        }
    }
}
