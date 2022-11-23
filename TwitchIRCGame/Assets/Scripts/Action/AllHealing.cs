using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class AllHealing : CharacterAction
    {
        public override bool IsTargeted => false;
        public override bool IsTargetOpponent => false;
        public override int ActionOrder => ORDER_BEFORE_ATTACK;

        public override string ActionName => throw new NotImplementedException();
        public override string Description => throw new NotImplementedException();

        public override void SetUser(Character user)
        {
            base.SetUser(user);
            SetHealing();

        }
        private void SetHealing()
        {
            actionEvent.AddListener(user.AllHeal);
        }
    }
}
