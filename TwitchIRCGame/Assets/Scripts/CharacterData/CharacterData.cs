using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    [Serializable]
    public class CharacterData      // 캐릭터에 종속된 데이터
    {
        public string characterName;
        public int level;
        public CharacterClass characterClass;

        public CharacterType characterType;
        public int maxHealth;
        public int health;
        public int basicDamage;
        /// <summary>치명타 발생 확률입니다.</summary>
        public float basicCritPercentage;
        /// <summary>치명타 발생 시의 대미지 배율입니다.</summary>
        public float basicCritDamageScale;

        public int place;

        public List<CharacterAction> actions;

        public CharacterData(CharacterClass characterClass, int place, string name = "Anon")
        {
            this.characterClass = characterClass;
            characterName = name;

            level = 1;
            this.characterType = CharacterType.None;
            maxHealth = 100;
            health = maxHealth;
            basicDamage = 10;
            basicCritPercentage = .2f;
            basicCritDamageScale = 2f;
            this.place = place;
            this.actions = new List<CharacterAction>(3);

        }

    }
}
