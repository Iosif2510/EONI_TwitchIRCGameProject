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

