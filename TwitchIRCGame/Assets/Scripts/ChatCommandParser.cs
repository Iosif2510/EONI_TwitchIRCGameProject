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

        // 게임 매니저로 서번트 생성
        // 배틀 매니저로 커맨드 생성

        private void Awake()
        {
            twitchIRC.newChatMessageEvent.AddListener(CommandParse);
        }

        // 여기서 직접 함수를 실행시키는 게 아닌
        // 반대로 battlemanager에게 정보를 전달하는 형태로 만들어보자
        // 응 싫어 퍼블릭으로 바꿀거야
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
                        case "!a":
                            Debug.Log($"{chatter.tags.displayName} selects a action");
                            GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 0);
                            break;
                        case "!b":
                            if (message.Length == 2 && int.Parse(message[1]) >= 1 && int.Parse(message[1]) <= 4)
                            {
                                // 디버깅 헷갈려서 길게 씀
                                Debug.Log($"{chatter.tags.displayName} selects b {message[1]} action");
                                GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 1, int.Parse(message[1]) - 1);
                            }
                            else
                            {
                                Debug.Log("Require target");
                            }
                            break;
                        case "!c":
                            if (message.Length == 2 && int.Parse(message[1]) >= 1 && int.Parse(message[1]) <= 4)
                            {
                                // 디버깅 헷갈려서 길게 씀
                                Debug.Log($"{chatter.tags.displayName} selects c {message[1]} action");
                                GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 2, int.Parse(message[1]) - 1);
                            }
                            else
                            {
                                Debug.Log("Require target");
                            }
                            break;
                        case "!leave":
                            Debug.Log($"{chatter.tags.displayName} leaves");
                            //GameManager.Instance.ServantDelete(chatter.tags.userId);
                            break;
                        default:
                            Debug.Log("Invalid command");
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
