using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class TargetHealing : CharacterAction
    {
        public override string ActionName => "Target Healing";
        public override string Description => "Target Healing";

        public override bool IsTargeted => true;
        public override bool IsTargetOpponent => false;
        public override int ActionOrder => ORDER_BEFORE_ATTACK;

        public override void SetUser(Character user)
        {
            base.SetUser(user);
            SetHealing();

        }
        private void SetHealing()
        {
            actionEvent.AddListener(user.Heal);
        }
    }
}
