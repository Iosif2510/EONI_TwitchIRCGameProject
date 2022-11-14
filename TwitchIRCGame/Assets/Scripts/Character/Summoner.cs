using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class Summoner : Character
    {
        protected string chatterID;

        protected override void Awake()
        {
            base.Awake();
            opponentTarget = new List<Character>(GameManager.Battle.MaxEnemyNum);
            friendlyTarget = new List<Character>(GameManager.Battle.MaxServantNum);
        }
        
        public override void Attack(bool typedAttack)
        {
            foreach (var target in opponentTarget)
            {
                //Servant Animation
                Debug.Log($"{characterName} attacked {target.Name}!");
                AttackTarget(target, typedAttack);
            }
            base.Attack(typedAttack);   // 액션 이후 이벤트 발생(정상화 등)
        }

        public override void Taunt()
        {
            foreach (var enemy in GameManager.Battle.enemies)
            {
                //Taunt Animation
                TauntTarget(enemy);
            }
            Debug.Log($"{characterName} taunted!");
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

