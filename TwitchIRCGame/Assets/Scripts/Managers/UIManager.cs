using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwitchIRCGame 
{

    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        public Camera mainCamera;

        // for summoner action/enemy button
        private int actionIndex;
        private int targetIndex;

        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null) return null;
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (instance != this) Destroy(gameObject);
            }
        }

        public void SummonerAction(int getactionIndex) // action 1, 2, 3: parameter 0, 1 ,2
        {
            actionIndex = getactionIndex;
            Debug.Log("Action: " + (actionIndex+1));            
        }
        public void SummonerTarget(int gettargetIndex) // enemy 1, 2, 3, 4 : parameter 0, 1, 2, 3 / summoner : parameter -1 / servant 1, 2, 3 : parameter -2, -3, -4
        {            
            if (gettargetIndex < 0 && GameManager.Battle.summoner.Actions[actionIndex].IsTargetOpponent) // 잘못된 진영 타겟으로 선택 (적 스킬인데 아군 클릭)
            {
                GameManager.Battle.summonerAction = null;
                return;
            } 
            else if (gettargetIndex >= 0 && !GameManager.Battle.summoner.Actions[actionIndex].IsTargetOpponent) // 잘못된 진영 타겟으로 선택 (아군 스킬인데 적 클릭)
            {
                GameManager.Battle.summonerAction = null;
                return;
            }

            if(gettargetIndex >= 0 ) targetIndex = gettargetIndex;
            else targetIndex = (-1)*gettargetIndex - 2;

            if (GameManager.Battle.summoner.Actions[actionIndex].IsTargetOpponent)
            {
                Debug.Log("Target: Enemy " + (targetIndex + 1));
            }
            else
            {
                if (targetIndex == -1) Debug.Log("Target: Summoner");
                else Debug.Log("Target: Servant " + (targetIndex + 1));
            }

        }

        public void TurnEndButton() // 턴 종료 버튼
        {
            Debug.Log("Turn End Button On");

            //summonerAction에 행동 추가
            if (GameManager.Battle.summoner.Actions.Count <= actionIndex)
            {
                GameManager.Battle.summonerAction = null;  //null을 넣어줘야 마지막으로 한 행동 반복안함
            }
            else if(GameManager.Battle.summoner.Actions[actionIndex].IsTargetOpponent && GameManager.Battle.enemies.Count <= targetIndex)
            {
                GameManager.Battle.summonerAction = null;  
            }
            else if (!GameManager.Battle.summoner.Actions[actionIndex].IsTargetOpponent && GameManager.Battle.servants.Count <= targetIndex)
            {
                GameManager.Battle.summonerAction = null;
            }
            else
            {
                GameManager.Battle.summonerAction = GameManager.Battle.summoner.Actions[actionIndex];
                if (GameManager.Battle.summoner.Actions[actionIndex].IsTargeted)
                {
                    if (GameManager.Battle.summoner.Actions[actionIndex].IsTargetOpponent)
                    {
                        GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.enemies[targetIndex]);
                    }
                    else
                    {
                        if (targetIndex == -1) GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.summoner);
                        else GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.servants[targetIndex]);
                    }
                }
            }
            
            GameManager.Battle.DoTurnEnd();

        }


    }
}
