using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class NonTypeAttack : CharacterAction
    {
        public override string ActionName => "Non-type Attack";
        public override string Description => "Non-type Attack";
        public override bool IsTargeted => true;
        public override bool IsTargetOpponent => true;
        public override int ActionOrder => ORDER_ATTACK;

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
