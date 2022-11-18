using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class BattleManager : MonoBehaviour
    {
        public enum BattlePhase
        {
            SummonerSelectPhase,
            WaitServantSelectPhase,
            FightPhase,
            CheckPhase,
            BattleFinish
        }

        [SerializeField]
        private BattlePhase currentPhase;
        public BattlePhase CurrentPhase
        {
            get { return currentPhase; }
            private set { currentPhase = value; }
        }

        [SerializeField]
        private int maxServantNum = 3;
        [SerializeField]
        private int maxEnemyNum = 4;
        public int MaxServantNum => maxServantNum;
        public int MaxEnemyNum => maxEnemyNum;

        [SerializeField]
        private Summoner summonerPrefab;
        [SerializeField]
        private Servant servantPrefab;
        [SerializeField]
        private Enemy enemyPrefab;

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

        private void InitServants()
        {
            servants = new List<Servant>(maxServantNum);
            for (int i = 0; i < GameManager.Data.servantChatterDatas.Count; i++)
            {
                var newServant = Instantiate(servantPrefab);
                newServant.SetCharacterData(GameManager.Data.servantChatterDatas[i]);
                servants.Add(newServant);
            }
        }

        private void InitEnemies()
        {
            enemies = new List<Enemy>(maxEnemyNum);
        }


        private void InitBattle()
        {
            CurrentPhase = BattlePhase.SummonerSelectPhase;
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
        
        /*
        private void Update()
        {
            ActionChoiceTime();
        }
        */

        public void EndTurn()
        {   
            TestScenario();
            StartActions();
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
            SelectAction(CharacterClass.Enemy, 1, 0, 0);
            SelectAction(CharacterClass.Enemy, 2, 1, 1);
        }
        
        /// <summary>현재 턴에서 사용될 행동을 지정합니다.</summary>
        /// <param name="characterIndex">캐릭터 리스트에서 해당 캐릭터의 인덱스</param>
        /// <param name="actionIndex">행동 슬롯에서 해당 행동의 인덱스</param>
        /// <param name="targetIndex">대상 캐릭터의 인덱스 (-1이면 사역마를 의미)</param>
        private void SelectAction(CharacterClass characterClass, int characterIndex, int actionIndex, int targetIndex = 0)
        {
            if (characterIndex < 0)
                throw new System.Exception("Invalid character index: " + characterIndex);
            if (actionIndex < 0)
                throw new System.Exception("Invalid action index: " + actionIndex);
            if (targetIndex < -1)
                throw new System.Exception("Invalid target index: " + targetIndex);
            
            switch (characterClass)
            {
                case CharacterClass.Servant:
                    // 존재하지 않는 캐릭터 (?)
                    if (servants.Count <= characterIndex) {
                        throw new System.Exception($"Servant {characterIndex + 1} does not exist");
                    }
                    // 캐릭터 사망
                    else if (servants[characterIndex].IsGroggy)
                    {
                        Debug.Log($"Servant {characterIndex + 1} is in groggy!");
                        return;
                    }

                    // 존재하지 않는 행동
                    if (servants[characterIndex].Actions.Count <= actionIndex) {
                        servantActionList[characterIndex] = null;
                        // TODO: 오류 알림
                        Debug.Log("Action " + (actionIndex + 1) + " is not in the slot");
                    }
                    
                    // 존재하지 않는 대상
                    if (targetIndex == -1 || enemies.Count <= targetIndex)
                    {
                        servantActionList[characterIndex] = null;
                        // TODO: 오류 알림
                        Debug.Log("Enemy " + (targetIndex + 1) + " does not exist");
                    }
                    //대상 사망
                    else if (!enemies[targetIndex].gameObject.activeSelf)
                    {
                        Debug.Log($"Enemy {enemies[targetIndex].Name} is dead!");
                        return;
                    }
                    
                    // 행동 선택
                    servantActionList[characterIndex] = servants[characterIndex].Actions[actionIndex];
                    if (servants[characterIndex].Actions[actionIndex].IsTargeted)
                    {
                        if (servants[characterIndex].Actions[actionIndex].IsTargetOpponent)
                        {
                            // 적군 선택
                            servants[characterIndex].SetSingleTarget(enemies[targetIndex]);
                        }
                        else
                        {
                            // 아군 선택
                            if (targetIndex == -1) servants[characterIndex].SetSingleTarget(summoner);
                            else servants[characterIndex].SetSingleTarget(servants[targetIndex]);
                        }
                    }
                    break;
                case CharacterClass.Enemy:
                    // 존재하지 않는 캐릭터 (?)
                    if (characterIndex < 0 || enemies.Count <= characterIndex) {
                        throw new System.Exception("Enemy " + (characterIndex + 1) + "does not exist");
                    }
                    // 캐릭터 사망
                    else if (enemies[characterIndex].gameObject.activeSelf == false)    
                    {
                        Debug.Log($"Enemy {characterIndex + 1} is dead!");
                        return;
                    }

                    // 존재하지 않는 행동
                    if (enemies[characterIndex].Actions.Count <= actionIndex)
                    {
                        enemyActionList[characterIndex] = null;
                        throw new System.Exception("Enemy " + (characterIndex + 1) + " has selected an invalid action: Action " + (actionIndex + 1));
                    }
                    
                    // 존재하지 않는 대상
                    if (servants.Count <= targetIndex)
                    {
                        enemyActionList[characterIndex] = null;
                        throw new System.Exception("Enemy " + (characterIndex + 1) + " has selected an invalid target: Target " + (targetIndex + 1));
                    }
                    // 대상 사망
                    else if (servants[targetIndex].IsGroggy)
                    {
                        Debug.Log($"Servant {servants[targetIndex].Name} is in groggy!");
                        return;
                    }
                    
                    // 행동 선택
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
                default:
                    throw new System.Exception("Summoner action should be selected by UI buttons");
            }
        } 

        /// <summary>지정한 행동들을 실행합니다.</summary>
        private void StartActions()
        {
            CurrentPhase = BattlePhase.FightPhase;
            if (summonerAction != null)
                summonerAction.DoAction();
            
            for (int order = 0; order < ORDER_MAX; order++)
            {
                for (int i = 0; i < maxServantNum; i++)
                {
                    if (servantActionList[i] == null) continue;
                    
                    if (servantActionList[i].ActionOrder == order)
                    {
                        // 그로기에 빠진 사역마
                        if (servants[i].IsGroggy)
                        {
                            //TODO 그로기 애니메이션(행동을 할 수 없음)
                            continue;
                        }
                        else
                        {
                            servantActionList[i].DoAction();
                        }
                    }
                }
                for (int i = 0; i < maxEnemyNum; i++)
                {
                    if (enemyActionList[i] == null) continue;
                    
                    if (enemyActionList[i].ActionOrder == order)
                        enemyActionList[i].DoAction();
                }
            }

            summonerAction = null;
            for (int i = 0; i < maxServantNum; i++)
            {
                servantActionList[i] = null;
            }
            for (int i = 0; i < maxEnemyNum; i++)
            {
                enemyActionList[i] = null;
            }

            if (CheckClear()) StageClear();
        }

        private bool CheckClear()
        {
            bool allEnemyDied = true;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    allEnemyDied = false;
                }
            }

            return allEnemyDied;
        }

        private void StageClear()
        {
            //TODO 씬 전환, 성장, 보상 등
            Debug.Log("Stage Clear!");
        }

    }
}

