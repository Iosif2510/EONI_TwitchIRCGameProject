using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField]
        private int maxTeamNum = 3; // �Ʊ� �翪���� 3�����, ������ 4������� �� �ϵ��ڵ���
        public int MaxTeamNum => maxTeamNum;

        [SerializeField]
        public Summoner summoner;      // �ӽ� ������ ����ȭ
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
            SetActionLists(); // �׼� ����Ʈ ����: �̰� ��� ��Ʋ���� ������ �κ��� �ƴ�
                                // ���Ƿ� ����� �ΰ� ���߿� �ٸ� ������ ����
        }

        private void Update()
        {
            if(!OnTurn) ActionChoiceTime();
        }

        public void OnButtonClick(GameObject button) // �� ���� ��ư
        {   
            if(!OnTurn)
            {
                OnTurn = true;
                Debug.Log("Test");
                TestScenario(); // ��ȯ��,�翪��,���� �׼� ��� ���� 
                StartActions();
            }

        }
        public void Action1(GameObject button) // �׼�1 ��ư
        {
            if (!OnTurn)
            {
                Debug.Log("button1");
                summoner.ChoiceA = 0;
            }
        }
        public void Action2(GameObject button) // �׼�2 ��ư
        {
            if (!OnTurn)
            {
                Debug.Log("button2");
                summoner.ChoiceA = 1;
            }
        }
        public void Action3(GameObject button) // �׼�3 ��ư
        {
            if (!OnTurn)
            {
                Debug.Log("button3");
                summoner.ChoiceA = 2;
            }
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
            
        }
        private void TestScenario()
        {
            Debug.Log("Test Scenario");            
            // ��ȯ�� �ൿ ���� (characterIndex = 10�� ��ȯ�縦 �ǹ�)
            SelectAction(true, 10, summoner.ChoiceA, summoner.ChoiceE);

            // �翪�� �ൿ ����, �ൿ�� �������� ���� ��� SelectAction�� ����
            SelectAction(true, 0, 0, 0);
            SelectAction(true, 1, 1, 1);
            SelectAction(true, 2, 1, 2);

            // �� �ൿ ���� (opponentIndex = 10�� ��ȯ�縦 �ǹ�)
            SelectAction(false, 0, 1, 0);
            SelectAction(false, 1, 0, 1);
            SelectAction(false, 2, 1, 10);
        }
        private void SelectAction(bool isServant, int characterIndex, int actionIndex, int opponentIndex)
        {
            // ���� ������ �ϱ⳪��
            // phaseActionList ������� �����
            if (isServant)
            {
                if (characterIndex == 10) // ��ȯ���� �ൿ
                {
                    if (summoner.Actions.Count <= actionIndex) return;   //TODO error
                    phaseActionList[0] = summoner.Actions[actionIndex];
                    summoner.SetSingleTarget(enemies[opponentIndex]);
                }
                else // �翪���� �ൿ
                {
                    if (servants[characterIndex].Actions.Count <= actionIndex) return;   //TODO error
                    phaseActionList[characterIndex + 1] = servants[characterIndex].Actions[actionIndex];
                    servants[characterIndex].SetSingleTarget(enemies[opponentIndex]);
                }
            }
            else
            {
                if (enemies[characterIndex].Actions.Count <= actionIndex) return;   //TODO error
                if (opponentIndex == 10) // ��ȯ�� ���
                {
                    phaseActionList[characterIndex + 4] = enemies[characterIndex].Actions[actionIndex];
                    enemies[characterIndex].SetSingleTarget(summoner);
                }
                else // �翪�� ���
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
                // ����, ��� �� ���ൿ
                if (phaseActionList[i] == null) continue;
                else if (phaseActionList[i].GetType() == typeof(TauntAction)) phaseActionList[i].DoAction();
            }
            for (int i = 0; i < 8; i++)
            {
                // ���� �� ���ൿ
                if (phaseActionList[i] == null) continue;
                else if (phaseActionList[i].GetType() != typeof(TauntAction)) phaseActionList[i].DoAction();
                if (summoner.Showhealth() == 0) break; // ��ȯ�� ��� �� break
            }
            OnTurn = false;
        }
        

        private void Mission()
        {

        }
    }
}

