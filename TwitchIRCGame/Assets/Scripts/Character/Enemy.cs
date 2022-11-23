using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TwitchIRCGame
{
    //InBattle State¿¡¼­¸¸ »ç¿ë... BattleManager Safe
    public class Enemy : Character
    {
        [SerializeField]
        private TMP_Text positionTextObject;
        private string positionText;

        protected override void Awake()
        {
            base.Awake();
            opponentTarget = new List<Character>(GameManager.Battle.MaxServantNum);
            friendlyTarget = new List<Character>(GameManager.Battle.MaxEnemyNum);
            positionText = $"{this.place}";
        }

        /// <summary>행동 선택 시에만 위치 텍스트가 표시됩니다.</summary>
        public void SetPositionTextDisplay(bool isSelecting)
        {
            if (isSelecting)
                positionTextObject.text = positionText;
            else
                positionTextObject.text = "";
        }
        
        protected override void OnHealthZero()
        {
            Die();
        }
        public override void Die()
        {
            gameObject.SetActive(false);
            base.Die();
        }

    }
}

