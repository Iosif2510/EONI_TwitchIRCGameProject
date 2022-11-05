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

        public abstract bool IsTargeted { get; }        // 무대상 or 전체대상: false / 특정대상: true
        public abstract bool IsTargetOpponent { get; }  // 아군대상: false / 적군대상: true
        public abstract int ActionOrder { get; }        // ActionOrder 순으로 BattleManager에서 액션 시행

        //public abstract bool IsTargeted();
        //public abstract bool IsTargetOpponent();
        //public abstract int ActionOrder();

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
