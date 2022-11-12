using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class Servant : Character
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

        
    }
}

