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
    }
}

