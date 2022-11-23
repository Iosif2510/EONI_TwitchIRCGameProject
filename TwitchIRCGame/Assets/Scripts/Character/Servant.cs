using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class Servant : Character
    {
        protected string chatterID;
        public string ChatterID => chatterID;
        
        protected override void Awake()
        {
            base.Awake();
            opponentTarget = new List<Character>(GameManager.Battle.MaxEnemyNum);
            friendlyTarget = new List<Character>(GameManager.Battle.MaxServantNum);
        }
        
        protected override void OnHealthZero()
        {
            if (!isGroggy) Groggy();
        }

        public void Groggy()
        {
            // 배틀 스테이트에서 HP가 0이 되면 그로기 상태가 됨
            // 배틀 스테이트 끝나기 전까지 그로기가 풀리지 않으면 사망
            isGroggy = true;
            transform.localScale *= .8f;        // 임시 표현
        }

        public void RecoverGroggy()
        {
            isGroggy = false;
            transform.localScale /= .8f;
        }

        public override void Die()
        {
            base.Die();
            //TODO die effect
            GameManager.Instance.ServantDelete(this);
            gameObject.SetActive(false);
        }
        
    }
}

