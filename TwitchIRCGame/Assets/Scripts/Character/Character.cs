using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static TwitchIRCGame.Define;
using static TwitchIRCGame.Utils;

namespace TwitchIRCGame
{
    public class Character : MonoBehaviour
    {
        [SerializeField]
        protected string characterName;
        [SerializeField]
        protected int level = 1;

        [SerializeField]
        protected CharacterType characterType;
        [SerializeField]
        protected int maxHealth;
        protected int health;
        [SerializeField]
        protected int basicDamage = 10;
        /*
        [SerializeField]
        protected int speed;              // 공격 순서 결정, 미사용
        */
        [SerializeField]
        protected float basicCritPercentage = .02f;     // 크리티컬 확률
        [SerializeField]
        protected float basicCritDamageScale = 2f;     // 크리티컬 시 대미지 배율


        [SerializeField]
        protected int place;
        protected List<Character> opponentTarget;
        protected List<Character> friendlyTarget;

        // 액션 리스트: 턴 종료 후 순차적으로 액션 시행
        protected List<CharacterAction> actions;

        public string Name => characterName;
        public int Health => health;
        public int Place => place;  // playerTeam 혹은 enemyTeam index
        public List<Character> OpponentTarget => opponentTarget;

        protected UnityEvent ReturnAfterAction = new UnityEvent();     // 도발, 디버프 등으로 바뀐 수치가 한 턴 후에 원복되는 이벤트

        public int Level
        {
            get { return level; }
            set
            {
                if (value <= 0) level = 1;
                else level = value;
            }
        }

        public List<CharacterAction> Actions => actions;

        private void Awake()
        {
            actions = new List<CharacterAction>(3);
            health = maxHealth;
            level = 1;
            opponentTarget = new List<Character>(GameManager.Battle.MaxTeamNum);
            friendlyTarget = new List<Character>(GameManager.Battle.MaxTeamNum);
        }

        private void Start()
        {

        }

        public void Damage(CharacterType attackType, int damage)
        {
            float typeDamagePercent;
            // TODO: 상성 관련 상수들 함수 밖으로 빼기
            // 상성 처리 자체를 Character.Damage()가 아닌 외부에서 해도 좋음
            switch (TypeSynergy(attackType, this.characterType))
            {
                case 0:
                    typeDamagePercent = 0;
                    break;
                case 1:
                    typeDamagePercent = 0.33f;
                    break;
                case 2:
                    typeDamagePercent = -0.33f;
                    break;
                default:
                    typeDamagePercent = 0;
                    break;
            }
            int finalDamage = Mathf.FloorToInt(damage * (1 + typeDamagePercent));
            health -= finalDamage;
            if (health < 0) health = 0; // 사망에 관한처리를 해야함
            Debug.Log($"{characterName} got {finalDamage} damage!");
        }

        public int Showhealth()
        {
            return this.health;
        }

        public void AddAction(CharacterAction action)
        {            
            if (actions.Count >= 3) return;     // 3개까지만 수용
            else
            {
                action.SetUser(this);
                actions.Add(action);
            }
        }

        public void DeleteAction(CharacterAction action)
        {
            actions.Remove(action);
        }

        public void SetSingleTarget(Character target)
        {
            this.opponentTarget.Clear();
            this.opponentTarget.Add(target);
        }

        public virtual void Attack(bool typedAttack) 
        {
            ReturnAfterAction.Invoke();
            ReturnAfterAction.RemoveAllListeners();
        }

        protected void AttackTarget(Character target, bool typedAttack)
        {
            AttackTarget(target, typedAttack, basicDamage, basicCritPercentage, basicCritDamageScale);
        }

        protected void AttackTarget(Character target, bool typedAttack, int damage, float critPercentage, float critScale)
        {
            if (target == null) return;
            int finalDamage = damage;
            if (Random.Range(0, 100) < critPercentage * 100) finalDamage = (int)(finalDamage * critScale);
            target.Damage(typedAttack ? characterType : CharacterType.None, finalDamage);
        }

        protected void Taunted(Character target)
        {
            //ReturnAfterAction.AddListener(() => ReturnTaunt(opponentTarget[0]));
            SetSingleTarget(target);
        }

        public virtual void Taunt() 
        {
            ReturnAfterAction.Invoke();
            ReturnAfterAction.RemoveAllListeners();
        }

        protected void TauntTarget(Character target)
        {
            if (target == null) return;
            if (target.OpponentTarget.Count == 1)
            {
                target.Taunted(this);
            }
        }

        protected void ReturnTaunt(Character originalTarget)
        {
            this.opponentTarget.Clear();
            this.opponentTarget.Add(originalTarget);
        }

    }
}
