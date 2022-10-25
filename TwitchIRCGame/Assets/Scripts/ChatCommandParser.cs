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
            if (IsPlayer(chatter.tags.userId))
            {
                string[] command = chatter.message.Split(' ');
                if (command[0][0] == '!')
                {
                    switch (command[0])
                    {
                        case "!attack":
                            break;

                    }
                }
            }
            

        }

        private bool IsPlayer(string playerID)
        {
            return GameManager.Instance.playerTeam.ContainsKey(playerID);
        }
    }

}
