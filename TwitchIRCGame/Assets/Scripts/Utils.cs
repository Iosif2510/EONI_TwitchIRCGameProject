using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public static class Utils
    {
        /// <summary>
        /// 0: ����, 1: ����, 2: ����
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
    }
}

