using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRCGame
{
    public class TauntAction : CharacterAction
    {
        public override bool IsTargeted => false;
        public override bool IsTargetOpponent => false;
        public override int ActionOrder => 0;

        public override void SetUser(Character user)
        {
            base.SetUser(user);
            SetTaunt();
        }

        private void SetTaunt()
        {
            actionEvent.AddListener(user.Taunt);
        }
    }
}
