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
        private string actionName;
        private UnityEvent characterEvent;
        private string actionDescription;

        public string ActionName => actionName;
        public string Description => actionDescription;

        public void DoAction()
        {
            characterEvent.Invoke();
        }
    }
}
