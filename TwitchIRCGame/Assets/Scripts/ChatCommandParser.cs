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
        // ���� �Ŵ����� ����Ʈ ����
        // ��Ʋ �Ŵ����� Ŀ�ǵ� ����


        private void Awake()
        {
            twitchIRC.newChatMessageEvent.AddListener(CommandParse);
        }

        // gamemanager.createservant


        // ���⼭ ���� �Լ��� �����Ű�� �� �ƴ�
        // �ݴ�� battlemanager���� ������ �����ϴ� ���·� ������
        // �� �Ⱦ� �ۺ����� �ٲܰž�

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
            // �� ��á���� !join ����
            Debug.Log($"Action {GameManager.Instance.servantIDs.Count}");
            if (GameManager.Instance.servantIDs.Count < GameManager.Battle.MaxServantNum)
            {
                if (chatter.message.Length == 5)
                {
                    // �����̶� ä���� �ɷ�����
                    string[] message = chatter.message.Split(' ');
                    if (message[0] == "!join")
                    {
                        Debug.Log("Create Servant");
                        GameManager.Instance.CreateServant(chatter.tags.userId);
                        GameManager.Battle.servants[GameManager.Instance.servantIDs.Count].Name = chatter.tags.displayName;
                    }
                }
                
            }

            // action chat
            if (IsPlayer(chatter.tags.userId))
            {
                string[] message = chatter.message.Split(' ');
                // command chat
                if (message[0][0] == '!')
                {
                    switch (message[0])
                    {
                        case "!attack":
                            Debug.Log("attack!");
                            GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, 0, 0);
                            GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, 1, 1, 1);
                            GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, 2, 1, 1);
                            break;

                    }
                }
                // idle chat
                else
                {

                }
            }


        }

        private bool IsPlayer(string playerID)
        {
            return GameManager.Instance.servantTeam.ContainsKey(playerID);
        }
    }

}
