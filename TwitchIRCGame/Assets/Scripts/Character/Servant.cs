using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class Servant : Character
    {
        private ChatterData chatterData;

        public string ChatterID => Chatter.ChatterID;

        // Note: BattleManager.SelectAction()ì—ì„œ ì œë„¤ë¦­ í´ë˜ìŠ¤ <T>ë¥¼ ì‚¬ìš©í•´ ì¤‘ë³µì„ ì¤„ì´ë ¤ë©´
        //       ì´ í•„ë“œëŠ” ë¶€ëª¨ í´ë˜ìŠ¤ì¸ Characterë¡œ ê°€ì•¼ í•¨.
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
            // ¹èÆ² ½ºÅ×ÀÌÆ®¿¡¼­ HP°¡ 0ÀÌ µÇ¸é ±×·Î±â »óÅÂ°¡ µÊ
            // ¹èÆ² ½ºÅ×ÀÌÆ® ³¡³ª±â Àü±îÁö ±×·Î±â°¡ Ç®¸®Áö ¾ÊÀ¸¸é »ç¸Á
            isGroggy = true;
            transform.localScale *= .8f;        // ÀÓ½Ã Ç¥Çö
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

