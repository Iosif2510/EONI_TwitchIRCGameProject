using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TwitchIRCGame
{
    public class CharacterAction
    {
        protected string actionName;
        protected UnityEvent actionEvent = new UnityEvent();
        protected string actionDescription;

        public Character user;

        public string ActionName => actionName;
        public string Description => actionDescription;

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
