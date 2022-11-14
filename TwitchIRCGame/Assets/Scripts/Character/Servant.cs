using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class Servant : Character
    {
        protected string chatterID;
        public string ChatterID => chatterID;

        protected bool isGroggy;
        public bool IsGroggy => isGroggy;

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
            GameManager.Instance.ServantDelete(this);
            gameObject.SetActive(false);
        }


    }
}

