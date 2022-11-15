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
            // 순서 바꾸지 말 것
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
        
        public enum GameScene
        {
            Main,
            InBattle
        }

        // 행동 순서 결정에 사용되는 상수
        // ORDER_MAX 값은 for문에 사용되므로 새로운 행동 순서를 추가할 때마다 증가시켜 줄 것
        public const int ORDER_BEFORE_ATTACK = 0;
        public const int ORDER_ATTACK = 1;
        //public const int ORDER_AFTER_ATTACK = 2;
        public const int ORDER_MAX = 2;

        // 상성 대미지 계산에 사용되는 상수
        public const float NEUTRAL_DAMAGE_PERCENT = 0.0f;
        public const float WEAK_DAMAGE_PERCENT = 0.33f;
        public const float RESISTANT_DAMAGE_PERCENT = -0.33f;
    }
}
