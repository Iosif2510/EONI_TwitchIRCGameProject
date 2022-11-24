using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class TauntAction : CharacterAction
    {
        public override string ActionName => "Taunt";
        public override string Description => "Force all enemies to target you.";
        public override bool IsTargeted => false;
        public override bool IsTargetOpponent => false;
        public override int ActionOrder => ORDER_BEFORE_ATTACK;

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
