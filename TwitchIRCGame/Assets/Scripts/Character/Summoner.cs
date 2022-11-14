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
            base.Attack(typedAttack);
            foreach (var target in opponentTarget)
            {
                //Servant Animation
                Debug.Log($"{characterName} attacked {target.Name}!");
                AttackTarget(target, typedAttack);
            }
        }

        public override void Taunt()
        {
            base.Taunt();
            foreach (var enemy in GameManager.Battle.enemies)
            {
                //Taunt Animation
                TauntTarget(enemy);
            }
            Debug.Log($"{characterName} taunted!");
        }

        public override void Heal()
        {
            base.Heal();
            foreach (var target in friendlyTarget)
            {
                Debug.Log($"{characterName} Healing {target.Name}!");
                HealingTarget(target);
            }
        }
        public override void Guard()
        {
            base.Guard();
            GuardingTarget(this);
            Debug.Log($"{characterName} get guard!");
        }

        public override void AttackAll(bool typedAttack)
        {
            base.AttackAll(typedAttack);
            foreach (var target in GameManager.Battle.enemies)
            {
                Debug.Log($"{characterName} attacked {target.Name}!");
                AttackTarget(target, typedAttack);
            }
        }

        public override void AllHeal()
        {
            base.Heal();
            foreach (var target in GameManager.Battle.servants)
            {
                Debug.Log($"{characterName} Healing {target.Name}!");
                HealingTarget(target);
            }
        }
    }
}

