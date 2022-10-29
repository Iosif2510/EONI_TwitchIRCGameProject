using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRCGame
{
    public class TauntAction : CharacterAction
    {
        public override void SetAction(Character user)
        {
            base.SetAction(user);
            SetTaunt();
        }

        private void SetTaunt()
        {
            actionEvent.AddListener(user.Taunt);
        }
    }
}
