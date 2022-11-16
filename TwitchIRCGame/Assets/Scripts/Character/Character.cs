using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static TwitchIRCGame.Define;
using static TwitchIRCGame.Utils;
using TMPro;

namespace TwitchIRCGame
{
    public abstract class Character : MonoBehaviour
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
        /// <summary>치명타 발생 확률입니다.</summary>
        [SerializeField]
        protected float basicCritPercentage = .02f;
        /// <summary>치명타 발생 시의 대미지 배율입니다.</summary>
        [SerializeField]
        protected float basicCritDamageScale = 2f;
        
        /// <summary>사역마에게만 적용되는 빈사 상태입니다.</summary>
        protected bool isGroggy;
        public bool IsGroggy => isGroggy;


        [SerializeField]
        protected int place;
        protected List<Character> opponentTarget;
        protected List<Character> friendlyTarget;
        
        protected List<CharacterAction> actions;

        /// <summary>체력 바 오브젝트입니다. 체력 값을 수정할 때 반드시 함께 변경되어야 합니다.</summary>
        [SerializeField]
        protected GameObject healthBar;
        
        [SerializeField]
        protected TMP_Text levelTextObject;

        public string Name => characterName;
        public int Health => health;
        /// <summary>팀 진영 내에서의 위치를 의미합니다.</summary>
        public int Place => place;
        public List<Character> OpponentTarget => opponentTarget;
        /// <summary>도발, 디버프 등으로 변동된 수치가 한 턴 후에 원상 복귀되는 이벤트입니다.</summary>
        protected UnityEvent ReturnAfterAction = new UnityEvent();

        public int Level
        {
            get { return level; }
            set
            {
                if (value <= 0) level = 1;
                else level = value;
            }
        }

        /// <summary>행동 슬롯입니다. 행동을 최대 3개까지 등록 가능합니다.</summary>
        public List<CharacterAction> Actions => actions;

        protected virtual void Awake()
        {
            actions = new List<CharacterAction>(3);
            health = maxHealth;
            level = 1;
            isGroggy = false;
            levelTextObject.text = $"Level: {level}";
        }

        private void Start()
        {
        }

        public void Damage(CharacterType attackType, int damage)
        {
            float typeDamagePercent = TypeDamagePercent(attackType, this.characterType);
            int finalDamage = Mathf.FloorToInt(damage * (1 + typeDamagePercent));
            health -= finalDamage;

            if (health < 0) {
                health = 0;
                OnHealthZero();
            }

            float displayedHealth = (float) health / (float) maxHealth;
            healthBar.transform.localScale = new Vector3(displayedHealth, 1.0f, 1.0f);
            
            Debug.Log($"{characterName} got {finalDamage} damage!");
            Debug.Log($"{characterName}'s health: {displayedHealth}");
        }

        // 제안: 행동 슬롯을 배열로 설정하여 AddAction(action, slotNumber)으로 고치는 건 어떤지?
        public void AddAction(CharacterAction action)
        {            
            if (actions.Count >= 3) return;     // 3개까지만 수용
            else
            {
                action.SetUser(this);
                actions.Add(action);
            }
        }

        // 제안: AddAction에서와 마찬가지로 DeleteAction(slotNumber)로 고치는 것?
        public void DeleteAction(CharacterAction action)
        {
            actions.Remove(action);
        }

        public void ClearTarget()
        {
            this.opponentTarget.Clear();
        }

        public void AddTarget(Character target)
        {
            this.opponentTarget.Add(target);
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
        
        public void RestoreHealth()
        {

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
        
        protected abstract void OnHealthZero();
        public virtual void Die()
        {

        }
    }
}
