using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class ChatCommandParser : MonoBehaviour
    {
        [SerializeField]
        private TwitchIRC twitchIRC;

        private void Awake()
        {
            twitchIRC.newChatMessageEvent.AddListener(CommandParse);
        }

        private void CommandParse(Chatter chatter)
        {
            string[] command = chatter.message.Split(' ');
            
        }
    }

}
