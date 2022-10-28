using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    [Obsolete]
    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField]
        private TwitchIRC twitchIRC;
        [SerializeField]
        private int maxTeamPlayerCount;
        [SerializeField]
        private PlayerCharacter playerPrefab;

        private int spawnCount = 0;
        private List<Dictionary<string, PlayerCharacter>> playerTeams;
        [SerializeField]
        private Transform playerPositionParent;
        private List<Transform> playerPositions;

        // Start is called before the first frame update
        void Awake()
        {
            playerTeams = new List<Dictionary<string, PlayerCharacter>>(2);
            playerTeams.Add(new Dictionary<string, PlayerCharacter>(maxTeamPlayerCount));
            playerTeams.Add(new Dictionary<string, PlayerCharacter>(maxTeamPlayerCount));
            playerPositions = new List<Transform>();
            foreach (Transform position in playerPositionParent)
            {
                playerPositions.Add(position);
            }
            twitchIRC.newChatMessageEvent.AddListener(NewMessageParse);
        }

        public void NewMessageParse(Chatter chatter)
        {
            string[] message = chatter.message.Split(' ');
            if (playerTeams[0].ContainsKey(chatter.tags.userId) || playerTeams[1].ContainsKey(chatter.tags.userId))
            {
                //TODO action
                if (message[0] == "!leave")
                {
                    PlayerCharacter deletePlayer;
                    string deleteId = chatter.tags.userId;
                    int containTeam = playerTeams[0].ContainsKey(deleteId) ? 0 : 1;
                    deletePlayer = playerTeams[containTeam][deleteId];
                    playerTeams[containTeam].Remove(deleteId);
                    Destroy(deletePlayer.gameObject);
                }
            }
            else if (spawnCount < maxTeamPlayerCount * 2)
            {
                if ((message.Length == 2) && (message[0] == "!join"))
                {
                    PlayerCharacter newPlayer;
                    spawnCount++;
                    int teamIndex = int.Parse(message[1]) - 1;
                    if ((teamIndex == 0) || (teamIndex == 1))
                    {
                        if (playerTeams[teamIndex].Count < maxTeamPlayerCount)
                        {
                            newPlayer = Instantiate(playerPrefab);
                            playerTeams[teamIndex].Add(chatter.tags.userId, newPlayer);
                            newPlayer.Name = chatter.tags.displayName;
                            newPlayer.teamNumber = teamIndex + 1;
                            newPlayer.transform.position = playerPositions[teamIndex * 4 + playerTeams[teamIndex].Count - 1].position;
                        }
                        else
                        {
                            Debug.Log($"Team {teamIndex + 1} full!");
                        }
                    }

                }
            }
                
            
        }
    }
}

