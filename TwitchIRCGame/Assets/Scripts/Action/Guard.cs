using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class Guard : CharacterAction
    {
        public override bool IsTargeted => false;
        public override bool IsTargetOpponent => false;
        public override int ActionOrder => ORDER_BEFORE_ATTACK;

        public override void SetUser(Character user)
        {
            base.SetUser(user);
            SetGuard();
        }
        private void SetGuard()
        {
            actionEvent.AddListener(user.Guard);
        }

    }
}
