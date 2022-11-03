using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField]
        private int maxTeamNum = 3; // 아군 사역마는 3명까지, 적군은 4명까지라 걍 하드코딩함
        public int MaxTeamNum => maxTeamNum;

        [SerializeField]
        public Summoner summoner;      // 임시 에디터 직렬화
        [SerializeField]
        public List<Servant> servants;
        [SerializeField]
        public List<Enemy> enemies;

        private CharacterAction[] phaseActionList;

        private bool OnTurn = false;

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
            phaseActionList = new CharacterAction[8];
            for (int i = 0; i < 8; i++)
            {
                phaseActionList[i] = null;
            }
        }

        private void Start()
        {
            SetActionLists(); // 액션 리스트 설정: 이건 사실 배틀에서 구현할 부분이 아님
                                // 임의로 만들어 두고 나중에 다른 씬에서 구현
        }

        private void Update()
        {
            if(!OnTurn) ActionChoiceTime();
        }

        public void OnButtonClick(GameObject button) // 턴 종료 버튼
        {   
            if(!OnTurn)
            {
                OnTurn = true;
                Debug.Log("Test");
                TestScenario(); // 소환사,사역마,적의 액션 대상 지정 
                StartActions();
            }

        }
        public void Action1(GameObject button) // 액션1 버튼
        {
            if (!OnTurn)
            {
                Debug.Log("button1");
                summoner.ChoiceA = 0;
            }
        }
        public void Action2(GameObject button) // 액션2 버튼
        {
            if (!OnTurn)
            {
                Debug.Log("button2");
                summoner.ChoiceA = 1;
            }
        }
        public void Action3(GameObject button) // 액션3 버튼
        {
            if (!OnTurn)
            {
                Debug.Log("button3");
                summoner.ChoiceA = 2;
            }
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
            
        }
        private void TestScenario()
        {
            Debug.Log("Test Scenario");            
            // 소환사 행동 지정 (characterIndex = 10은 소환사를 의미)
            SelectAction(true, 10, summoner.ChoiceA, summoner.ChoiceE);

            // 사역마 행동 지정, 행동을 선택하지 않은 경우 SelectAction을 안함
            SelectAction(true, 0, 0, 0);
            SelectAction(true, 1, 1, 1);
            SelectAction(true, 2, 1, 2);

            // 적 행동 지정 (opponentIndex = 10은 소환사를 의미)
            SelectAction(false, 0, 1, 0);
            SelectAction(false, 1, 0, 1);
            SelectAction(false, 2, 1, 10);
        }
        private void SelectAction(bool isServant, int characterIndex, int actionIndex, int opponentIndex)
        {
            // 순서 구현은 하기나름
            // phaseActionList 순서대로 실행됨
            if (isServant)
            {
                if (characterIndex == 10) // 소환사의 행동
                {
                    if (summoner.Actions.Count <= actionIndex) return;   //TODO error
                    phaseActionList[0] = summoner.Actions[actionIndex];
                    summoner.SetSingleTarget(enemies[opponentIndex]);
                }
                else // 사역마의 행동
                {
                    if (servants[characterIndex].Actions.Count <= actionIndex) return;   //TODO error
                    phaseActionList[characterIndex + 1] = servants[characterIndex].Actions[actionIndex];
                    servants[characterIndex].SetSingleTarget(enemies[opponentIndex]);
                }
            }
            else
            {
                if (enemies[characterIndex].Actions.Count <= actionIndex) return;   //TODO error
                if (opponentIndex == 10) // 소환사 대상
                {
                    phaseActionList[characterIndex + 4] = enemies[characterIndex].Actions[actionIndex];
                    enemies[characterIndex].SetSingleTarget(summoner);
                }
                else // 사역마 대상
                {
                    phaseActionList[characterIndex + 4] = enemies[characterIndex].Actions[actionIndex];
                    enemies[characterIndex].SetSingleTarget(servants[opponentIndex]);
                }

            }
        }

        private void StartActions()
        {
            for (int i = 0; i < 8; i++)
            {
                // 도발, 방어 등 선행동
                if (phaseActionList[i] == null) continue;
                else if (phaseActionList[i].GetType() == typeof(TauntAction)) phaseActionList[i].DoAction();
            }
            for (int i = 0; i < 8; i++)
            {
                // 공격 등 후행동
                if (phaseActionList[i] == null) continue;
                else if (phaseActionList[i].GetType() != typeof(TauntAction)) phaseActionList[i].DoAction();
                if (summoner.Showhealth() == 0) break; // 소환사 사망 시 break
            }
            OnTurn = false;
        }
        

        private void Mission()
        {

        }
    }
}

