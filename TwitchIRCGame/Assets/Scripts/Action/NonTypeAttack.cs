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
        public override string ActionName => "Physical Attack";
        public override string Description => "Make a non-type attack on a target enemy.";

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
