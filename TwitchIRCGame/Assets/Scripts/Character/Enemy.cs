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

