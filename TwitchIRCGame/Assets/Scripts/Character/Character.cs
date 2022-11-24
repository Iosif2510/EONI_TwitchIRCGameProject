using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static TwitchIRCGame.Define;
using static TwitchIRCGame.Utils;
using TMPro;
using static UnityEngine.GraphicsBuffer;

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
        protected int Guardpoint;
        [SerializeField]
        protected int basicDamage = 10;
        protected int basicHealing = 5;
        protected int basicGuarding = 5;
        protected int basicBuff = 2;
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
        protected const string TARGET_NONE = "No target";
        protected const string TARGET_MULTIPLE = "Multiple target";
        
        protected List<CharacterAction> actions;

        /// <summary>체력 바 오브젝트입니다. 체력 값을 수정할 때 반드시 함께 변경되어야 합니다.</summary>
        [SerializeField]
        protected GameObject healthBar;
        
        [SerializeField]
        protected TMP_Text levelTextObject;
        [SerializeField]
        protected TMP_Text targetTextObject;

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
            targetTextObject.text = TARGET_NONE;
        }

        private void Start()
        {
        }

        public void Damage(CharacterType attackType, int damage)
        {
            float typeDamagePercent = TypeDamagePercent(attackType, this.characterType);
            int finalDamage = Mathf.FloorToInt(damage * (1 + typeDamagePercent));
            finalDamage -= Guardpoint;
            if(finalDamage > 0)
            {
                health -= finalDamage;
                Guardpoint = 0;
            }
            else
            {
                Guardpoint -= finalDamage;
            }
            if (health < 0) {
                health = 0;
                OnHealthZero();
            }

            float displayedHealth = (float) health / (float) maxHealth;
            healthBar.transform.localScale = new Vector3(displayedHealth, 1.0f, 1.0f);
            Debug.Log($"{characterName} got {finalDamage} damage!");
            Debug.Log($"{characterName}'s health: {health}/{maxHealth}");
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
            targetTextObject.text = TARGET_NONE;
        }

        public void AddTarget(Character target)
        {
            this.opponentTarget.Add(target);
            if (this.opponentTarget.Count == 1)
                targetTextObject.text = this.opponentTarget[0].Name;
            else
                targetTextObject.text = TARGET_MULTIPLE;
        }

        public void SetSingleTarget(Character target)
        {
            ClearTarget();
            AddTarget(target);
        }

        public void SetSingleSupport(Character target)
        {
            this.friendlyTarget.Clear();
            this.friendlyTarget.Add(target);
        }

        public void Attack(bool typedAttack) 
        {
            foreach (var target in opponentTarget)
            {
                //Servant Animation
                Debug.Log($"{characterName} attacked {target.Name}!");
                AttackTarget(target, typedAttack);
            }
            ReturnAfterAction.Invoke();
            ReturnAfterAction.RemoveAllListeners();
        }

        public void AttackAll(bool typedAttack)
        {
            foreach (var target in GameManager.Battle.enemies)
            {
                //Servant Animation
                Debug.Log($"{characterName} attacked {target.Name}!");
                AttackTarget(target, typedAttack);
            }
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

        public void Taunt() 
        {
            if (this.GetType() == typeof(Enemy))
            {
                foreach (var opponent in GameManager.Battle.servants)
                {
                    //Taunt Animation
                    TauntTarget(opponent);
                }
            }
            else
            {
                foreach (var opponent in GameManager.Battle.enemies)
                {
                    //Taunt Animation
                    TauntTarget(opponent);
                }
            }
            ReturnAfterAction.Invoke();
            ReturnAfterAction.RemoveAllListeners();
            Debug.Log($"{characterName} taunted!");
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
        public void Healing(int heal)
        {
            health += heal;
            //최대 체력 이상 회복 불가
            if (health > maxHealth) health = maxHealth;
        }

        protected void HealingTarget(Character target)
        {
            if (target == null) return;
            target.Healing(basicHealing);
        }

        public virtual void Heal()
        {
            foreach (var target in friendlyTarget)
            {
                Debug.Log($"{characterName} Healing {target.Name}!");
                HealingTarget(target);
            }
            ReturnAfterAction.Invoke();
            ReturnAfterAction.RemoveAllListeners();
        }

        public virtual void AllHeal()
        {
            foreach (var target in GameManager.Battle.servants)
            {
                Debug.Log($"{characterName} Healing {target.Name}!");
                HealingTarget(target);
            }
            ReturnAfterAction.Invoke();
            ReturnAfterAction.RemoveAllListeners();
        }
        public void Guarding(int guards)
        {
            Guardpoint += guards;
        }
        protected void GuardingTarget(Character target)
        {
            if (target == null) return;
            target.Guarding(basicGuarding);
        }

        public virtual void Guardreset()
        {
            Guardpoint = 0;
        }
        public virtual void Guard()
        {
            GuardingTarget(this);
            Debug.Log($"{characterName} Get Guard!");
            ReturnAfterAction.Invoke();
            ReturnAfterAction.RemoveAllListeners();
        }

        public virtual void Buff()
        {
            foreach (var target in friendlyTarget)
            {
                Debug.Log($"{characterName} Buff {target.Name}!");
                BuffTarget(target);
            }
            ReturnAfterAction.Invoke();
            ReturnAfterAction.RemoveAllListeners();
        }

        protected void BuffTarget(Character target)
        {
            if (target == null) return;
            target.BuffDamage(basicBuff);
        }

        public void BuffDamage(int buff)
        {
            basicDamage += buff;
        }

        public void resetBuff(int buff)
        {
            basicDamage -= buff;
        }
    }
}
