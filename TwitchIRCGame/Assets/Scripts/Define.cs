using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRCGame
{
    public static class Define
    {
        public enum CharacterType
        {
            // 순서바꾸지말것
            Fire,
            Water,
            Grass,
            None
        }

        public enum CharacterClass
        {
            None,
            Summoner,
            Servant,
            Enemy
        }

        public const float NEUTRAL_DAMAGE_PERCENT = 0.0f;
        public const float WEAK_DAMAGE_PERCENT = 0.33f;
        public const float RESISTANT_DAMAGE_PERCENT = -0.33f;
    }
}
