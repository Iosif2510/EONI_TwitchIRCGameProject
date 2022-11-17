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
            for (int i = 0; i < GameManager.Instance.servantIDs.Count; i++)
            {
                servants.Add(GameManager.Instance.servantTeam[GameManager.Instance.servantIDs[i]]);
            }
        }
        
        private void InitBattle()
        {
            //enemies = new List<Enemy>(maxEnemyNum);
            
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
            // 선택 페이즈로 변경
            CurrentPhase = BattlePhase.SummonerSelectPhase;
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].SetPositionTextDisplay(true);

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
            
            TestScenario();
        }
        
        /*
        private void Update()
        {
            ActionChoiceTime();
        }
        */

        public void EndTurn()
        {
            StartActions();
            TestScenario();
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
            SelectAction<Servant>(servants, 0, 0);
            SelectAction<Servant>(servants, 1, 1, 1);
            SelectAction<Servant>(servants, 2, 1, 2);

            /// 적 행동 지정
            SelectAction<Enemy>(enemies, 0, 1, 0);
            SelectAction<Enemy>(enemies, 1, 0);
            SelectAction<Enemy>(enemies, 2, 1, -1);
        }
        
        /// <summary>현재 턴에서 사용될 행동을 지정합니다.</summary>
        /// <param name="characters">해당 캐릭터가 포함된 캐릭터 리스트</param>
        /// <param name="characterIndex">캐릭터 리스트에서 해당 캐릭터의 인덱스</param>
        /// <param name="actionIndex">행동 슬롯에서 해당 행동의 인덱스</param>
        /// <param name="targetIndex">대상 캐릭터의 인덱스 (-1이면 사역마를 의미)</param>
        private void SelectAction<T>(List<T> characters, int characterIndex, int actionIndex, int targetIndex = 0)
        where T : Character
        {
            if (characterIndex < 0)
                throw new System.Exception($"Invalid character index: {characterIndex}");
            if (actionIndex < 0)
                throw new System.Exception($"Invalid action index: {actionIndex}");
            if (targetIndex < -1)
                throw new System.Exception($"Invalid target index: {targetIndex}");

            CharacterAction[] actionList;
            string characterString;

            if (typeof(T) == typeof(Servant))
            {
                actionList = servantActionList;
                characterString = "Servant";
            }
            else if (typeof(T) == typeof(Enemy))
            {
                actionList = enemyActionList;
                characterString = "Enemy";
            }
            else
            {
                throw new System.Exception("Summoner action should be selected by UI buttons");
            }
            
            // 존재하지 않는 캐릭터
            if (characters.Count <= characterIndex)
            {
                throw new System.Exception($"{characterString} {characterIndex + 1} does not exist.");
            }
            
            // TODO: 다음과 같은 상황에서 사용자에게 오류 알림
            // 캐릭터 빈사
            if (characters[characterIndex].IsGroggy)
            {
                Debug.Log($"{characterString} {characterIndex + 1} is in groggy!");
            }

            // 존재하지 않는 행동
            else if (characters[characterIndex].Actions.Count <= actionIndex)
            {
                if (typeof(T) == typeof(Enemy))
                {
                    throw new System.Exception($"Enemy {(characterIndex + 1)} has selected an invalid action: Action {(actionIndex + 1)}");
                }
                Debug.Log($"Action {(actionIndex + 1)} is not in the slot!");
            }
            
            else
            {
                actionList[characterIndex] = characters[characterIndex].Actions[actionIndex]; // 행동 선택
                if (actionList[characterIndex].IsTargeted)
                {
                    Character target;
                    // 이 값이 true이면 대상은 적 진영, false이면 대상은 아군 진영임
                    bool isTargetEnemy = (typeof(T) == typeof(Servant)) ==
                                         (actionList[characterIndex].IsTargetOpponent);
                    
                    // TODO: 다음과 같은 상황에서 사용자에게 오류 알림
                    // 존재하지 않는 대상
                    if (isTargetEnemy && (targetIndex == -1 || enemies.Count <= targetIndex) ||
                        !isTargetEnemy && (targetIndex != -1 && servants.Count <= targetIndex))
                    {
                        if (typeof(T) == typeof(Enemy))
                        {
                            throw new System.Exception($"Enemy {(characterIndex + 1)} has selected an invalid target.");                                                
                        }
                        Debug.Log($"Target does not exist!");
                        return; // 강제 종료
                    }
                    
                    if (isTargetEnemy)
                        target = enemies[targetIndex]; 
                    else if (targetIndex != -1)
                        target = servants[targetIndex];
                    else
                        target = summoner;
                    
                    // TODO: 다음과 같은 상황에서 사용자에게 오류 알림
                    // 대상 사망
                    if (targetIndex != -1 && !target.gameObject.activeSelf)
                    {
                        Debug.Log($"Target {target.Name} is dead!");
                        return; // 강제 종료
                    }
                    
                    characters[characterIndex].SetSingleTarget(target); // 대상 선택
                }
            }
        } 

        /// <summary>지정한 행동들을 실행합니다.</summary>
        private void StartActions()
        {
            // 전투 페이즈로 변경
            CurrentPhase = BattlePhase.FightPhase;
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].SetPositionTextDisplay(false);
            
            if (summonerAction != null)
                summonerAction.DoAction();
            
            for (int order = 0; order < ORDER_MAX; order++)
            {
                for (int i = 0; i < servants.Count; i++)
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
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemyActionList[i] == null) continue;
                    
                    if (enemyActionList[i].ActionOrder == order)
                        enemyActionList[i].DoAction();
                }
                
            }

            summonerAction = null;
            summoner.ClearTarget();
            for (int i = 0; i < servants.Count; i++)
            {
                servantActionList[i] = null;
                servants[i].ClearTarget();
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemyActionList[i] = null;
                enemies[i].ClearTarget();
            }

            if (CheckClear()) StageClear();
            
            // 선택 페이즈로 변경
            CurrentPhase = BattlePhase.SummonerSelectPhase;
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].SetPositionTextDisplay(true);
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
            // TODO: 씬 전환, 성장, 보상 등
            Debug.Log("Stage Clear!");
        }

    }
}

