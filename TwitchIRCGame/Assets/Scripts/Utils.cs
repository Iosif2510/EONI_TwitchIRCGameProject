using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public static class Utils
    {
        /// <summary>
        /// <c>damageType</c>이 <c>attackType</c>에 대해 가지는 상성을 반환합니다.<br/>
        /// 중립: <c>0</c>, 취약: <c>1</c>, 내성: <c>2</c>
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

        /// <summary>
        /// <c>damageType</c>이 <c>attackType</c>에 대해 가지는 상성에 따라
        /// 산출된 대미지 증감율을 반환합니다.<br/>
        /// </summary>
        public static float TypeDamagePercent(CharacterType attackType, CharacterType damageType)
        {
            var damagePercentTable = new Dictionary<int, float>()
            {
                {0, NEUTRAL_DAMAGE_PERCENT},
                {1, WEAK_DAMAGE_PERCENT},
                {2, RESISTANT_DAMAGE_PERCENT}
            };

            int synergy = TypeSynergy(attackType, damageType);
            return damagePercentTable[synergy];
        }
    }
}

