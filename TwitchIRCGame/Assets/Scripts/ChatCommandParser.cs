using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TwitchIRCGame.Define;


namespace TwitchIRCGame
{
    public class ChatCommandParser : MonoBehaviour
    {
        [SerializeField]
        private TwitchIRC twitchIRC;

        //[SerializeField]
        //private BattleManager battleManager = gameManager.Battle;
        // 게임 매니저로 서번트 생성
        // 배틀 매니저로 커맨드 생성


        private void Awake()
        {
            twitchIRC.newChatMessageEvent.AddListener(CommandParse);
        }

        // gamemanager.createservant


        // 여기서 직접 함수를 실행시키는 게 아닌
        // 반대로 battlemanager에게 정보를 전달하는 형태로 만들어보자
        // 응 싫어 퍼블릭으로 바꿀거야

        //public void NewMessageParse(Chatter chatter)
        //{
        //    string[] message = chatter.message.Split(' ');
        //    if (playerTeams[0].ContainsKey(chatter.tags.userId) || playerTeams[1].ContainsKey(chatter.tags.userId))
        //    {
        //        //TODO action
        //        if (message[0] == "!leave")
        //        {
        //            PlayerCharacter deletePlayer;
        //            string deleteId = chatter.tags.userId;
        //            int containTeam = playerTeams[0].ContainsKey(deleteId) ? 0 : 1;
        //            deletePlayer = playerTeams[containTeam][deleteId];
        //            playerTeams[containTeam].Remove(deleteId);
        //            Destroy(deletePlayer.gameObject);
        //        }
        //    }
        //    else if (spawnCount < maxTeamPlayerCount * 2)
        //    {
        //        if ((message.Length == 2) && (message[0] == "!join"))
        //        {
        //            PlayerCharacter newPlayer;
        //            spawnCount++;
        //            if ((teamIndex == 0) || (teamIndex == 1))
        //            {
        //                if (playerTeams[teamIndex].Count < maxTeamPlayerCount)
        //                {
        //                    newPlayer = Instantiate(playerPrefab);
        //                    playerTeams[teamIndex].Add(chatter.tags.userId, newPlayer);
        //                    newPlayer.Name = chatter.tags.displayName;
        //                    newPlayer.teamNumber = teamIndex + 1;
        //                }
        //                else
        //                {
        //                    Debug.Log($"Team {teamIndex + 1} full!");
        //                }
        //            }

        //        }
        //    }



        private void CommandParse(Chatter chatter)
        {
            // action chat
            if (IsPlayer(chatter.tags.userId))
            {
                Debug.Log("Is player");
                string[] message = chatter.message.Split(' ');
                // command chat
                if (message[0][0] == '!')
                {
                    switch (message[0])
                    {
                        case "!attack":
                            if (message.Length == 2 && int.Parse(message[1]) >= 1 && int.Parse(message[1]) <= 4)
                            {
                                // 디버깅 헷갈려서 길게 씀
                                Debug.Log($"{chatter.tags.displayName} selects attack {message[1]} action");
                                GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 1, int.Parse(message[1]) - 1);
                            }
                            else
                            {
                                Debug.Log("Invalid command. Require target");
                            }
                            break;
                        case "!taunt":
                            Debug.Log($"{chatter.tags.displayName} selects taunt action");
                            GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 0);
                            break;
                        case "!leave":
                            Debug.Log($"{chatter.tags.displayName} leaves");
                            //GameManager.Instance.ServantDelete(chatter.tags.userId);
                            break;
                    }
                }
                // idle chat
                else
                {

                }
            }
            else
            {
                // 꽉 안찼으면 !join 가능
                Debug.Log($"Current team {GameManager.Instance.servantIDs.Count}");
                if (GameManager.Instance.servantIDs.Count < GameManager.Battle.MaxServantNum)
                {
                    if (chatter.message.Length == 5)
                    {
                        // 조금이라도 채팅을 걸러보자
                        string[] message = chatter.message.Split(' ');
                        if (message[0] == "!join")
                        {
                            Debug.Log($"Servant {chatter.tags.displayName} summoned");
                            GameManager.Instance.CreateServant(chatter.tags.userId);
                            GameManager.Battle.servants[GameManager.Instance.servantIDs.Count].Name = chatter.tags.displayName;
                        }
                    }

                }
            }


        }

        private bool IsPlayer(string playerID)
        {
            return GameManager.Instance.servantIDs.Contains(playerID);
        }
    }

}
