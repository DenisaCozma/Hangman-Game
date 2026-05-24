using System;
using System.Collections.Generic;
using System.Text;

namespace Spanzuratoare.Models
{
    public class GameState
    {
        public string Username { get; set; }
        public int Level { get; set; }
        public int Index { get; set; }
        public int TimeLeft { get; set; }
        public string SelectedCategory { get; set; }
        public string Word { get; set; }
        public List<string> WordDisplay { get; set; }
        public List<string> UsedLetters { get; set; }
    }
}
