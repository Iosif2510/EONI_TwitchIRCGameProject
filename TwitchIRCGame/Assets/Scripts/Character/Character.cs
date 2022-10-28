using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static TwitchIRCGame.Define;
using static TwitchIRCGame.Utils;

namespace TwitchIRCGame
{
    public class Character : MonoBehaviour
    {
        private string servantName;
        private string chatterID;

        private int level;

        [SerializeField]
        private CharacterType characterType;
        [SerializeField]
        private int maxHealth;
        private int health;
        [SerializeField]
        private int damage;
        [SerializeField]
        private int speed;              // 공격 순서 결정, 미사용
        [SerializeField]
        private int critPercentage;     // 크리티컬 확률, 백분율

        private List<int> target = new List<int>(GameManager.Battle.MaxTeamNum);

        [SerializeField]
        private CharacterAction[] actions = new CharacterAction[3];

        public int Health => health;

        public int Level
        {
            get { return level; }
            set
            {
                if (value <= 0) level = 1;
                else level = value;
            }
        }

        private void Start()
        {
            actions[0].DoAction();
        }

        public void Damage(CharacterType attackType, int damage)
        {
            float typeDamagePercent;
            switch (TypeSynergy(attackType, this.characterType))
            {
                case 0:
                    typeDamagePercent = 0;
                    break;
                case 1:
                    typeDamagePercent = 0.33f;
                    break;
                case 2:
                    typeDamagePercent = -0.33f;
                    break;
                default:
                    typeDamagePercent = 0;
                    break;
            }
            health -= Mathf.FloorToInt(damage * (1 + typeDamagePercent));
        }

        protected void AttackTarget(Character target, bool typedAttack)
        {
            if (target == null) return;
            int finalDamage = damage;
            if (Random.Range(0, 100) < critPercentage) finalDamage *= 2;
            target.Damage(typedAttack ? characterType : CharacterType.None, finalDamage);
        }

    }
}
