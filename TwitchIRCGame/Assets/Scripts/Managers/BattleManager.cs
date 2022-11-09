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
        public Summoner summoner;      // �ӽ� ������ ����ȭ
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
            SetActionLists(); // �׼� ����Ʈ ����: �̰� ��� ��Ʋ���� ������ �κ��� �ƴ�
                                // ���Ƿ� ����� �ΰ� ���߿� �ٸ� ������ ����
        }
        
        /*
        private void Update()
        {
            ActionChoiceTime();
        }
        */

        public void DoTurnEnd() // �� ���� ��ư
        {   
            TestScenario(); // ��ȯ��,�翪��,���� �׼� ��� ���� 
            StartActions(); 

        }

        private void SetActionLists()
        {
            // �Ʊ� �ൿ ����
            summoner.AddAction(new TauntAction());
            summoner.AddAction(new NonTypeAttack());
            servants[0].AddAction(new TauntAction());
            servants[0].AddAction(new NonTypeAttack());
            servants[1].AddAction(new TauntAction());
            servants[1].AddAction(new TypedAttack());
            servants[2].AddAction(new TauntAction());
            servants[2].AddAction(new TypedAttack());

            // ���� �ൿ ����
            enemies[0].AddAction(new TauntAction());
            enemies[0].AddAction(new NonTypeAttack());
            enemies[1].AddAction(new TauntAction());
            enemies[1].AddAction(new TypedAttack());
            enemies[2].AddAction(new TauntAction());
            enemies[2].AddAction(new TypedAttack());
        }

        private void ActionChoiceTime()
        {
            // summoner�� Ŭ���� ���ؼ� �ൿ�� ��� ����            

            // servant�� ä���� ���ؼ� �ൿ�� ��� ����
            // twitch chat ���� ��� �ʿ�            
            // �ش� �翪���� ���� �Ϸ� �ÿ� �̸� ��Ÿ���� Interface �ʿ�
        }
        private void TestScenario()
        {
            Debug.Log("Test Scenario");            
            // ��ȯ�� �ൿ ������ ��ư���� ����(UIManager)
            // �翪�� �ൿ ����, �ൿ�� �������� ���� ��� ActionList�� null
            SelectAction(CharacterClass.Servant, 0, 0, 0);
            SelectAction(CharacterClass.Servant, 1, 1, 1);
            SelectAction(CharacterClass.Servant, 2, 1, 2);

            // �� �ൿ ���� (opponentIndex = -1�� ��ȯ�縦 �ǹ�)
            SelectAction(CharacterClass.Enemy, 0, 1, 0);
            SelectAction(CharacterClass.Enemy, 1, 0);
            SelectAction(CharacterClass.Enemy, 2, 1, -1);
        }
        private void SelectAction(CharacterClass characterClass, int characterIndex, int actionIndex, int targetIndex = 0)
        {
            // ���� ������ �ϱ⳪��
            // targeted
            // phaseActionList ������� �����
            switch (characterClass)
            {   
                /*  UIManager���� ����
                case CharacterClass.Summoner:
                    if (summoner.Actions.Count <= actionIndex)
                    {
                        summonerAction = null;  //null�� �־���� ���������� �� �ൿ �ݺ�����
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
            if(summonerAction != null) summonerAction.DoAction(); // ���� �ڵ鸵
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

