using System;
using System.Collections.Generic;
using System.Text;

namespace Spanzuratoare.Models
{
    public class User
    {
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public Dictionary<string, int> WinsPerCategory { get; set; } = new();
        public Dictionary<string, int> GamesPerCategory { get; set; } = new();
    }
}
