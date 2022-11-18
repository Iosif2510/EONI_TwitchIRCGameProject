using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class Summoner : Character
    {
        protected ChatterData chatterData;

        protected override void Awake()
        {
            base.Awake();
            opponentTarget = new List<Character>(GameManager.Battle.MaxEnemyNum);
            friendlyTarget = new List<Character>(GameManager.Battle.MaxServantNum);
        }

        public override void SetCharacterData(ChatterData chatterData)
        {
            this.chatterData = GameManager.Data.summonerData.chatterData;
            this.characterData = GameManager.Data.summonerData.characterData;
        }

        public override void Attack(bool typedAttack)
        {
            foreach (var target in opponentTarget)
            {
                //Servant Animation
                Debug.Log($"{Name} attacked {target.Name}!");
                AttackTarget(target, typedAttack);
            }
            base.Attack(typedAttack);   // �׼� ���� �̺�Ʈ �߻�(����ȭ ��)
        }

        public override void Taunt()
        {
            foreach (var enemy in GameManager.Battle.enemies)
            {
                //Taunt Animation
                TauntTarget(enemy);
            }
            Debug.Log($"{Name} taunted!");
            base.Taunt();
        }

        protected override void OnHealthZero()
        {
            Die();
        }

        public override void Die()
        {
            base.Die();
            GameManager.Instance.GameOver();
        }
    }
}

