using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    //InBattle State������ ���... BattleManager Safe
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
            //TODO: Datamanager���� ���ǵ� �� �����Ϳ��� �޾ƿ� ��
            //TEMP
            characterData = new CharacterData(Define.CharacterClass.Enemy, 0, "Enemy");
        }

        public override void Attack(bool typedAttack)
        {
            base.Attack(typedAttack);
            foreach (var target in opponentTarget)
            {
                //Servant Animation
                Debug.Log($"{Name} attacked {target.Name}!");
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
            Debug.Log($"{Name} taunted!");
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

