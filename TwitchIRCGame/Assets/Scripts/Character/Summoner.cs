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

