using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class NonTypeAttackAll : CharacterAction
    {
        public override string ActionName => "Physical Attack: All";
        public override string Description => "Make a non-type attack on all enemies.";

        public override bool IsTargeted => false;
        public override bool IsTargetOpponent => true;
        public override int ActionOrder => ORDER_BEFORE_ATTACK;

        public override void SetUser(Character user)
        {
            base.SetUser(user);
            SetAttack();
        }

        private void SetAttack()
        {
            actionEvent.AddListener(() => user.AttackAll(false));
        }
    }
}
