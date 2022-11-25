using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TwitchIRCGame.Define;
using TMPro;

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

        // 임시; 행동 이펙트/애니메이션 구현 후 삭제할 것
        [SerializeField]
        private TMP_Text actionLogObject;
        
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
            
            //summoner.AddAction(new NonTypeAttackAll());
            summoner.AddAction(new TargetBuff());
            summoner.AddAction(new TargetHealing());
            summoner.AddAction(new AllHealing());

            servants[0].AddAction(new TauntAction());
            servants[0].AddAction(new NonTypeAttack());
            servants[0].AddAction(new Guard());

            servants[1].AddAction(new TauntAction());
            servants[1].AddAction(new TypedAttack());
            servants[1].AddAction(new Guard());

            servants[2].AddAction(new NonTypeAttack());
            servants[2].AddAction(new TypedAttack());
            servants[2].AddAction(new Guard());

            // 적군 행동 설정
            enemies[0].AddAction(new TauntAction());
            enemies[0].AddAction(new NonTypeAttack());

            enemies[1].AddAction(new TauntAction());
            enemies[1].AddAction(new TypedAttack());

            enemies[2].AddAction(new NonTypeAttack());
            enemies[2].AddAction(new TypedAttack());
            
            TestScenario();
        }
        

        public IEnumerator EndTurn()
        {
            // 전투 페이즈로 변경
            CurrentPhase = BattlePhase.FightPhase;
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].SetPositionTextDisplay(false);

            yield return StartActions();
            
            if (CheckClear()) StageClear();
            
            // 선택 페이즈로 변경
            CurrentPhase = BattlePhase.SummonerSelectPhase;
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].SetPositionTextDisplay(true);
            
            TestScenario();
        }


        private void TestScenario()
        {   
            // 소환사 행동 지정은 버튼으로 선택(UIManager)
            // 사역마 행동 지정, 행동을 선택하지 않은 경우 ActionList에 null
            
            /// 적 행동 지정
            for (int i = 0; i < enemies.Count; i++)
            {
                System.Random rand = new System.Random();
                int randAct = rand.Next(enemies[i].Actions.Count); //0,1 중 하나 선택                
                int randTarget = 0; // 1. 대상공격인 경우 대상을 랜덤으로 선택, 2. 공격이 아닌 경우 대상 없음
                if (enemies[i].Actions[randAct].IsTargeted)
                {
                    randTarget = rand.Next(-1, servants.Count); // 소환사, 사역마 중 하나 선택
                }
                SelectAction<Enemy>(enemies, i, randAct, randTarget);
            }
        }

        /// <summary>현재 턴에서 사용될 행동을 지정합니다.</summary>
        /// <param name="characters">해당 캐릭터가 포함된 캐릭터 리스트</param>
        /// <param name="characterIndex">캐릭터 리스트에서 해당 캐릭터의 인덱스</param>
        /// <param name="actionIndex">행동 슬롯에서 해당 행동의 인덱스</param>
        /// <param name="targetIndex">대상 캐릭터의 인덱스 (-1이면 사역마를 의미)</param>
        // 파서 연동 위해 public으로 바꿈 (용)
        public void SelectAction<T>(List<T> characters, int characterIndex, int actionIndex, int targetIndex = 0)
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
                    // 이 값이 true이면 대상은 해당 캐릭터의 적임
                    bool isTargetOpponent = actionList[characterIndex].IsTargetOpponent;
                    // 이 값이 true이면 대상은 적 진영, false이면 대상은 소환사 진영임
                    bool isTargetEnemy = (typeof(T) == typeof(Servant)) == isTargetOpponent;

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
                    {
                        // 적 진영
                        target = enemies[targetIndex];
                    }   
                    else
                    {
                        // 소환사 진영
                        if (targetIndex == -1) target = summoner;
                        else target = servants[targetIndex];
                    }
                    
                    // TODO: 다음과 같은 상황에서 사용자에게 오류 알림
                    // 대상 사망
                    if (targetIndex != -1 && !target.gameObject.activeSelf)
                    {
                        Debug.Log($"Target {target.Name} is dead!");
                        return; // 강제 종료
                    }

                    // 실제로 대상 지정

                    characters[characterIndex].SetSingleTarget(target);
                }
                else
                {
                    characters[characterIndex].ClearTarget();
                }
            }
        } 

        /// <summary>지정한 행동들을 실행합니다.</summary>
        private IEnumerator StartActions()
        {
            if (summonerAction != null)
            {
                summonerAction.DoAction();
                if (summonerAction.IsTargeted)
                {
                    actionLogObject.text =
                        $"{summoner.Name} performed {summonerAction.ActionName} to {summoner.Targets[0].Name}";
                }
                else
                {
                    actionLogObject.text =
                        $"{summoner.Name} performed {summonerAction.ActionName}";
                }
                yield return new WaitForSeconds(2);
            }
            
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
                            if (servantActionList[i].IsTargeted)
                            {
                                actionLogObject.text =
                                    $"{servants[i].Name} performed {servantActionList[i].ActionName} to {servants[i].Targets[0].Name}";
                            }
                            else
                            {
                                actionLogObject.text =
                                    $"{servants[i].Name} performed {servantActionList[i].ActionName}";
                            }
                            yield return new WaitForSeconds(2);
                        }
                    }
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemyActionList[i] == null) continue;

                    if (enemyActionList[i].ActionOrder == order)
                    {
                        enemyActionList[i].DoAction();
                        if (enemyActionList[i].IsTargeted)
                        {
                            actionLogObject.text =
                                $"{enemies[i].Name} performed {enemyActionList[i].ActionName} to {enemies[i].Targets[0].Name}";
                        }
                        else
                        {
                            actionLogObject.text =
                                $"{enemies[i].Name} performed {enemyActionList[i].ActionName}";
                        }
                                
                        yield return new WaitForSeconds(2);
                    }
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
            actionLogObject.text = "";
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

