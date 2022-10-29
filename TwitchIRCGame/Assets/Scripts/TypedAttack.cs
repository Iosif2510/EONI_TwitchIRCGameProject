using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRCGame
{
    public class TypedAttack : CharacterAction
    {
        public override void SetAction(Character user)
        {
            base.SetAction(user);
            SetAttack();
        }
        private void SetAttack()
        {
            actionEvent.AddListener(() => user.Attack(true));
        }
    }
}
