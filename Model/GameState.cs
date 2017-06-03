using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KevinBaconBot.Model
{
    [Serializable]
    public class GameState
    {
        public int NumberOfTurns { get; set; }
        public bool IsMovieAnswer { get; set; } // If false safe to assume it's an actor answer

        public string PreviousAnswer { get; set; }

        public string StartingActor { get; set; }
    }
}