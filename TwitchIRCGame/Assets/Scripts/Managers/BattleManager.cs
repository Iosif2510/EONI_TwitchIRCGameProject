using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField]
        private int maxTeamNum = 3;
        public int MaxTeamNum => maxTeamNum;

        [SerializeField]
        public Summoner summoner;      // �ӽ� ������ ����ȭ
        [SerializeField]
        public List<Servant> servants;
        [SerializeField]
        public List<Enemy> enemies;

        private CharacterAction[] phaseActionList;

        private void Awake()
        {
            //InitServants();
            InitBattle();
        }

        private void InitServants()
        {
            servants = new List<Servant>(maxTeamNum);
            for (int i = 0; i < GameManager.Instance.servantIDs.Count; i++)
            {
                servants.Add(GameManager.Instance.servantTeam[GameManager.Instance.servantIDs[i]]);
            }


        }

        private void InitBattle()
        {
            //enemies = new List<Enemy>(maxTeamNum);
            phaseActionList = new CharacterAction[maxTeamNum * 2 + 1];
            for (int i = 0; i < maxTeamNum * 2 + 1; i++)
            {
                phaseActionList[i] = null;
            }
        }

        private void Start()
        {
            SetActionLists(); // �׼� ����Ʈ ����
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

        private void TestScenario()
        {
            Debug.Log("Test Scenario");
            // �Ʊ� �ൿ ���� (characterIndex = 10�� ��ȯ�縦 �ǹ�)
            SelectAction(true, 10, 0, 0);
            SelectAction(true, 0, 0, 0);
            SelectAction(true, 1, 0, 1);
            SelectAction(true, 2, 0, 1);

            // �� �ൿ ���� (opponentIndex = 10�� ��ȯ�縦 �ǹ�)
            SelectAction(false, 0, 0, 0);
            SelectAction(false, 1, 0, 1);
            SelectAction(false, 2, 0, 10);
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
                    phaseActionList[characterIndex + maxTeamNum + 1] = enemies[characterIndex].Actions[actionIndex];
                    enemies[characterIndex].SetSingleTarget(summoner);
                }
                else // �翪�� ���
                {
                    phaseActionList[characterIndex + maxTeamNum + 1] = enemies[characterIndex].Actions[actionIndex];
                    enemies[characterIndex].SetSingleTarget(servants[opponentIndex]);
                }

            }
        }

        private void StartActions()
        {
            for (int i = 0; i < maxTeamNum * 2 + 1; i++)
            {
                // ����, ��� �� ���ൿ
                if (phaseActionList[i] == null) continue;
                else if (phaseActionList[i].GetType() == typeof(TauntAction)) phaseActionList[i].DoAction();
            }
            for (int i = 0; i < maxTeamNum * 2 + 1; i++)
            {
                // ���� �� ���ൿ
                if (phaseActionList[i] == null) continue;
                else if (phaseActionList[i].GetType() != typeof(TauntAction)) phaseActionList[i].DoAction();
            }
        }



        private void Mission()
        {

        }
    }
}

