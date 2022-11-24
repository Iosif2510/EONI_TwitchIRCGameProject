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

        // ���� �Ŵ����� ����Ʈ ����
        // ��Ʋ �Ŵ����� Ŀ�ǵ� ����

        private void Awake()
        {
            twitchIRC.newChatMessageEvent.AddListener(CommandParse);
        }

        // ���⼭ ���� �Լ��� �����Ű�� �� �ƴ�
        // �ݴ�� battlemanager���� ������ �����ϴ� ���·� ������
        // �� �Ⱦ� �ۺ����� �ٲܰž�
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
                            if (message.Length == 2)
                            {
                                // ����� �򰥷��� ��� ��
                                Debug.Log($"{chatter.tags.displayName} selects a to {message[1]} action");
                                GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 0, int.Parse(message[1]) - 1);
                            }
                            else
                            {
                                Debug.Log($"{chatter.tags.displayName} selects a action");
                                GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 0);
                            }
                            break;
                        case "!b":
                            if (message.Length == 2)
                            {
                                Debug.Log($"{chatter.tags.displayName} selects b to {message[1]} action");
                                GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 1, int.Parse(message[1]) - 1);
                            }
                            else
                            {
                                Debug.Log($"{chatter.tags.displayName} selects b action");
                                GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 1);
                            }
                            break;
                        case "!c":
                            if (message.Length == 2)
                            {
                                Debug.Log($"{chatter.tags.displayName} selects c to {message[1]} action");
                                GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 2, int.Parse(message[1]) - 1);
                            }
                            else
                            {
                                Debug.Log($"{chatter.tags.displayName} selects c action");
                                GameManager.Battle.SelectAction<Servant>(GameManager.Battle.servants, GameManager.Instance.servantIDs.IndexOf(chatter.tags.userId), 2);
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
                // �� ��á���� !join ����
                Debug.Log($"Current team {GameManager.Instance.servantIDs.Count}");
                if (GameManager.Instance.servantIDs.Count < GameManager.Battle.MaxServantNum)
                {
                    if (chatter.message.Length == 5)
                    {
                        // �����̶� ä���� �ɷ�����
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
