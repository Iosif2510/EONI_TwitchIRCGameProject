using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public static class Utils
    {
        /// <summary>
        /// 0: 중립, 1: 취약, 2: 내성
        /// </summary>
        public static int TypeSynergy(CharacterType attackType, CharacterType damageType)
        {
            if ((attackType == CharacterType.None) || (damageType == CharacterType.None)) return 0;
            else
            {
                int returnInt = ((int)attackType - (int)damageType) % 3;
                if (returnInt < 0) returnInt += 3;
                return returnInt;
            }
        }

        public static float TypeDamagePercent(CharacterType attackType, CharacterType damageType)
        {
            var damagePercentTable = new Dictionary<int, float>()
            {
                {0, NEUTRAL_DAMAGE_PERCENT},
                {1, WEAK_DAMAGE_PERCENT},
                {2, RESISTANT_DAMAGE_PERCENT}
            };

            int synergy = TypeSynergy(attackType, damageType);
            // @assert 0 <= synergy && synergy < 3
            return damagePercentTable[synergy];
        }
    }
}

