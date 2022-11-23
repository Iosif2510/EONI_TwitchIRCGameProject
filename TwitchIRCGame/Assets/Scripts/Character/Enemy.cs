using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    //InBattle State에서만 사용... BattleManager Safe
    public class Enemy : Character
    {
        protected override void Awake()
        {
            base.Awake();
            opponentTarget = new List<Character>(GameManager.Battle.MaxServantNum);
            friendlyTarget = new List<Character>(GameManager.Battle.MaxEnemyNum);
        }

        public override void SetCharacterData(ChatterData chatterData)
        {
            //TODO: Datamanager에서 정의된 적 데이터에서 받아올 것
            //TEMP
            characterData = new CharacterData(Define.CharacterClass.Enemy, 0, "Enemy");
        }

        protected override void OnHealthZero()
        {
            Die();
        }

        public override void Die()
        {
            base.Die();
            gameObject.SetActive(false);
        }
    }
}

