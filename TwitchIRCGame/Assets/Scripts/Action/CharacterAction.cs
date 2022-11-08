using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TwitchIRCGame
{
    public abstract class CharacterAction
    {
        protected string actionName;
        protected UnityEvent actionEvent = new UnityEvent();
        protected string actionDescription;

        public Character user;

        public string ActionName => actionName;
        public string Description => actionDescription;

        /// <summary>
        /// 해당 행동의 대상 범위를 의미합니다.
        /// <c>true</c>이면 특정 대상 행동,
        /// <c>false</c>이면 대상이 없는 행동이거나 전체 대상 행동입니다.
        /// <example>
        /// 적 1명을 공격: <c>true</c><br/>
        /// 아군 전체를 치유: <c>false</c><br/>
        /// 도발: <c>false</c><br/>
        /// </example>
        /// </summary>
        public abstract bool IsTargeted { get; }
        
        /// <summary>
        /// 해당 행동이 어느 진영을 대상으로 하는지를 의미합니다.
        /// <c>true</c>이면 적군 대상,
        /// <c>false</c>이면 아군 대상 행동입니다.
        /// </summary>
        public abstract bool IsTargetOpponent { get; }
        
        /// <summary>
        /// BattleManager에서 실행될 순서입니다.
        /// </summary>
        public abstract int ActionOrder { get; }

        public void DoAction()
        {
            actionEvent.Invoke();
        }

        public virtual void SetUser(Character user) 
        {
            this.user = user;
        }
    }
}
