using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class Enemy : Character
    {
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
    }
}

