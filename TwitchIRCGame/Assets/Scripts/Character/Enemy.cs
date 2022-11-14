using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class Enemy : Character
    {
        protected override void Awake()
        {
            base.Awake();
            opponentTarget = new List<Character>(GameManager.Battle.MaxServantNum);
            friendlyTarget = new List<Character>(GameManager.Battle.MaxEnemyNum);
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
            foreach (var servants in GameManager.Battle.servants)
            {
                //Taunt Animation
                TauntTarget(servants);
            }
            Debug.Log($"{characterName} taunted!");
        }
        public override void Guard()
        {
            base.Guard();
            GuardingTarget(this);
            Debug.Log($"{characterName} get guard!");
        }
    }
}

