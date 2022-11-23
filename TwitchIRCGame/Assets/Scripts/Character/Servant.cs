using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class Servant : Character
    {
        private ChatterData chatterData;

        public string ChatterID => Chatter.ChatterID;

        protected bool isGroggy;
        public bool IsGroggy => isGroggy;

        public ChatterData Chatter
        {
            get 
            {
                if (chatterData == null)
                {
                    throw new System.NotImplementedException();
                }
                return chatterData; 
            }
        }

        public override void SetCharacterData(ChatterData chatterData)
        {
            this.chatterData = chatterData;
            this.characterData = GameManager.Data.servantTeamData[chatterData];
        }

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
            // ��Ʋ ������Ʈ���� HP�� 0�� �Ǹ� �׷α� ���°� ��
            // ��Ʋ ������Ʈ ������ ������ �׷αⰡ Ǯ���� ������ ���
            isGroggy = true;
            transform.localScale *= .8f;        // �ӽ� ǥ��
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
            GameManager.Data.ServantDataDelete(chatterData);
            gameObject.SetActive(false);
        }


    }
}

