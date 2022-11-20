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
                switch (command[0])
                {
                    case "!attack":
                        // non typed attack
                        // command[1] command [2]
                        if (command[1] == null)
                        {
                            Debug.Log("Invalid commands. Target is not selected");
                        }
                        else 
                        {
                            Debug.Log("Player " + chatter.tags.displayName + " attack to " + command[1]);
                        }
                        
                        break;
                    // 명령어 임시
                    case "!skill":
                        if (command[1] == null)
                        {
                            Debug.Log("Invalid commands. Target is not selected");
                        } 
                        else
                        {
                            // typed attack
                            Debug.Log("Player " + chatter.tags.displayName + " activate typed attack to " + command[1]);
                        }
                        
                        break;
                    case "!taunt":
                        // non target action
                        Debug.Log("Player " + chatter.tags.displayName + " taunt");
                        break;
                }
            }
            // idle chatting
            else
            {

            }


        }

        private bool IsPlayer(string playerID)
        {
            return GameManager.Instance.servantTeam.ContainsKey(playerID);
        }
    }

}
