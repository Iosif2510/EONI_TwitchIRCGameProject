using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRCGame
{
    public class NonTypeAttack : CharacterAction
    {
        public override void SetUser(Character user)
        {
            base.SetUser(user);
            SetAttack();
        }

        private void SetAttack()
        {
            actionEvent.AddListener(() => user.Attack(false));
        }
    }
}
