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
        public Summoner summoner;      // 임시 에디터 직렬화
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
            SetActionLists(); // 액션 리스트 설정: 이건 사실 배틀에서 구현할 부분이 아님
                                // 임의로 만들어 두고 나중에 다른 씬에서 구현
        }
        
        /*
        private void Update()
        {
            ActionChoiceTime();
        }
        */

        public void DoTurnEnd() // 턴 종료 버튼
        {   
            TestScenario(); // 소환사,사역마,적의 액션 대상 지정 
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
            // twitch chat 연동 기능 필요            
            // 해당 사역마가 선택 완료 시에 이를 나타내는 Interface 필요
        }
        private void TestScenario()
        {
            Debug.Log("Test Scenario");            
            // 소환사 행동 지정은 버튼으로 선택(UIManager)
            // 사역마 행동 지정, 행동을 선택하지 않은 경우 ActionList에 null
            SelectAction(CharacterClass.Servant, 0, 0, 0);
            SelectAction(CharacterClass.Servant, 1, 1, 1);
            SelectAction(CharacterClass.Servant, 2, 1, 2);

            // 적 행동 지정 (opponentIndex = -1은 소환사를 의미)
            SelectAction(CharacterClass.Enemy, 0, 1, 0);
            SelectAction(CharacterClass.Enemy, 1, 0);
            SelectAction(CharacterClass.Enemy, 2, 1, -1);
        }
        private void SelectAction(CharacterClass characterClass, int characterIndex, int actionIndex, int targetIndex = 0)
        {
            // 순서 구현은 하기나름
            // targeted
            // phaseActionList 순서대로 실행됨
            switch (characterClass)
            {   
                /*  UIManager에서 구현
                case CharacterClass.Summoner:
                    if (summoner.Actions.Count <= actionIndex)
                    {
                        summonerAction = null;  //null을 넣어줘야 마지막으로 한 행동 반복안함
                        return;     //TODO error
                    }
                    summonerAction = summoner.Actions[actionIndex];
                    if (summoner.Actions[actionIndex].IsTargeted)
                    {
                        if (summoner.Actions[actionIndex].IsTargetOpponent)
                        {
                            summoner.SetSingleTarget(enemies[targetIndex]);
                        }
                        else
                        {
                            if (targetIndex == -1) summoner.SetSingleTarget(summoner);
                            else summoner.SetSingleTarget(servants[targetIndex]);
                        }
                    }
                    
                    break;
                */
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

            summonerAction = null;  // 액션리스트 초기화
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

