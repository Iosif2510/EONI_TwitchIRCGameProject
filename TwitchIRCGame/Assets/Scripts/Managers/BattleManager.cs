using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField]
        private int maxServantNum = 3;
        [SerializeField]
        private int maxEnemyNum = 4;
        public int MaxServantNum => maxServantNum;
        public int MaxEnemyNum => maxEnemyNum;

        [SerializeField]
        public Summoner summoner;      // 임시 에디터 직렬화 <- 무슨 뜻?
        [SerializeField]
        public List<Servant> servants;
        [SerializeField]
        public List<Enemy> enemies;

        public CharacterAction summonerAction;
        private CharacterAction[] servantActionList;
        private CharacterAction[] enemyActionList;
        
        private void Awake()
        {
            //InitServants();
            InitBattle();
        }
        /*
        private void InitServants()
        {
            servants = new List<Servant>(maxTeamNum);
            for (int i = 0; i < GameManager.Instance.servantIDs.Count; i++)
            {
                servants.Add(GameManager.Instance.servantTeam[GameManager.Instance.servantIDs[i]]);
            }
        }
        */

        private void InitBattle()
        {
            //enemies = new List<Enemy>(maxTeamNum);
            summonerAction = null;
            servantActionList = new CharacterAction[maxServantNum];
            for (int i = 0; i < maxServantNum; i++)
            {
                servantActionList[i] = null;
            }
            enemyActionList = new CharacterAction[maxEnemyNum];
            for (int i = 0; i < maxEnemyNum; i++)
            {
                enemyActionList[i] = null;
            }
        }

        private void Start()
        {
            // BattleManager에서 구현할 부분이 아니므로, 임시로 쓰고 나중에 다른 씬에서 구현
            SetActionLists();
        }
        
        /*
        private void Update()
        {
            ActionChoiceTime();
        }
        */

        /// <summary>턴을 종료합니다.</summary>
        public void DoTurnEnd()
        {   
            TestScenario();
            StartActions();
        }

        private void SetActionLists()
        {
            // 아군 행동 설정
            summoner.AddAction(new TauntAction());
            summoner.AddAction(new NonTypeAttack());
            servants[0].AddAction(new TauntAction());
            servants[0].AddAction(new NonTypeAttack());
            servants[1].AddAction(new TauntAction());
            servants[1].AddAction(new TypedAttack());
            servants[2].AddAction(new TauntAction());
            servants[2].AddAction(new TypedAttack());

            // 적군 행동 설정
            enemies[0].AddAction(new TauntAction());
            enemies[0].AddAction(new NonTypeAttack());
            enemies[1].AddAction(new TauntAction());
            enemies[1].AddAction(new TypedAttack());
            enemies[2].AddAction(new TauntAction());
            enemies[2].AddAction(new TypedAttack());
        }

        private void ActionChoiceTime()
        {
            // summoner는 클릭을 통해서 행동과 대상 선택            

            // servant는 채팅을 통해서 행동과 대상 선택
            // Twitch chat 연동 기능 필요            
            // 해당 사역마가 선택 완료 시에 이를 나타내는 interface 필요
        }
        private void TestScenario()
        {
            Debug.Log("Test Scenario");            
            // 소환사 행동 지정은 버튼으로 선택(UIManager)
            // 사역마 행동 지정, 행동을 선택하지 않은 경우 ActionList에 null
            SelectAction(CharacterClass.Servant, 0, 0, 0);
            SelectAction(CharacterClass.Servant, 1, 1, 1);
            SelectAction(CharacterClass.Servant, 2, 1, 2);

            /// 적 행동 지정
            SelectAction(CharacterClass.Enemy, 0, 1, 0);
            SelectAction(CharacterClass.Enemy, 1, 0);
            SelectAction(CharacterClass.Enemy, 2, 1, -1);
        }
        
        /// <summary>현재 턴에서 사용될 행동을 지정합니다.</summary>
        /// <param name="characterIndex">캐릭터 리스트에서 해당 캐릭터의 인덱스</param>
        /// <param name="actionIndex">행동 슬롯에서 해당 행동의 인덱스</param>
        /// <param name="targetIndex">대상 캐릭터의 인덱스 (-1이면 사역마를 의미)</param>
        /// <returns></returns>
        private void SelectAction(CharacterClass characterClass, int characterIndex, int actionIndex, int targetIndex = 0)
        {
            // 순서 구현은 하기나름
            // targeted
            // phaseActionList 순서대로 실행됨
            switch (characterClass)
            {
                case CharacterClass.Servant:
                    if (servants[characterIndex].Actions.Count <= actionIndex) {
                        servantActionList[characterIndex] = null;
                        return;   //TODO error
                    }
                    else if (enemies.Count <= targetIndex)
                    {
                        servantActionList[characterIndex] = null;
                        return;
                    }
                    servantActionList[characterIndex] = servants[characterIndex].Actions[actionIndex];
                    if (servants[characterIndex].Actions[actionIndex].IsTargeted)
                    {
                        if (servants[characterIndex].Actions[actionIndex].IsTargetOpponent)
                        {
                            servants[characterIndex].SetSingleTarget(enemies[targetIndex]);
                        }
                        else
                        {
                            if (targetIndex == -1) servants[characterIndex].SetSingleTarget(summoner);
                            else servants[characterIndex].SetSingleTarget(servants[targetIndex]);
                        }
                    }
                    break;
                case CharacterClass.Enemy:
                    if (enemies[characterIndex].Actions.Count <= actionIndex)
                    {
                        enemyActionList[characterIndex] = null;
                        return;   //TODO error
                    }
                    else if ((servants.Count + 1) <= targetIndex)
                    {
                        enemyActionList[characterIndex] = null;
                        return;
                    }
                    enemyActionList[characterIndex] = enemies[characterIndex].Actions[actionIndex];
                    if (enemies[characterIndex].Actions[actionIndex].IsTargeted)
                    {
                        if (enemies[characterIndex].Actions[actionIndex].IsTargetOpponent)
                        {
                            if (targetIndex == -1) enemies[characterIndex].SetSingleTarget(summoner);
                            else enemies[characterIndex].SetSingleTarget(servants[targetIndex]);
                        }
                        else
                        {
                            enemies[characterIndex].SetSingleTarget(enemies[targetIndex]);
                        }
                    }
                    break;
            }
        } 

        /// <summary>지정한 행동들을 실행합니다.</summary>
        private void StartActions()
        {
            if(summonerAction != null) summonerAction.DoAction(); // 에러 핸들링
            for (int order = 0; order < 3; order++)
            {
                for (int i = 0; i < maxServantNum; i++)
                {
                    if (servantActionList[i] == null) continue;
                    else if (servantActionList[i].ActionOrder == order) servantActionList[i].DoAction();
                }
                for (int i = 0; i < maxEnemyNum; i++)
                {
                    if (enemyActionList[i] == null) continue;
                    else if (enemyActionList[i].ActionOrder == order) enemyActionList[i].DoAction();
                }
            }

            summonerAction = null;  // �׼Ǹ���Ʈ �ʱ�ȭ
            for (int i = 0; i < maxServantNum; i++)
            {
                servantActionList[i] = null;
            }
            for (int i = 0; i < maxEnemyNum; i++)
            {
                enemyActionList[i] = null;
            }

        }
        

        private void Mission()
        {

        }
    }
}

