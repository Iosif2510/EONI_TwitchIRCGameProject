using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField]
        private int maxTeamNum = 4;
        public int MaxTeamNum => maxTeamNum;

        [SerializeField]
        public List<Servant> servants;      // 임시 에디터 직렬화
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
            phaseActionList = new CharacterAction[maxTeamNum * 2];
            for (int i = 0; i < maxTeamNum * 2; i++)
            {
                phaseActionList[i] = null;
            }
        }

        private void Start()
        {
            TestScenario();
            StartActions();
        }

        private void TestScenario()
        {
            Debug.Log("Test Scenario");
            // 액션 리스트 설정
            servants[0].AddAction(new TypedAttack());
            servants[0].AddAction(new NonTypeAttack());
            servants[1].AddAction(new TauntAction());
            servants[1].AddAction(new TypedAttack());
            servants[2].AddAction(new TypedAttack());
            servants[2].AddAction(new NonTypeAttack());
            servants[3].AddAction(new TauntAction());
            servants[3].AddAction(new NonTypeAttack());

            enemies[0].AddAction(new TypedAttack());
            enemies[0].AddAction(new NonTypeAttack());
            enemies[1].AddAction(new TauntAction());
            enemies[1].AddAction(new TypedAttack());
            enemies[2].AddAction(new TypedAttack());
            enemies[2].AddAction(new NonTypeAttack());
            enemies[3].AddAction(new TauntAction());
            enemies[3].AddAction(new NonTypeAttack());

            SelectAction(true, 0, 1, 3);
            SelectAction(true, 3, 0, 0);
            SelectAction(true, 2, 0, 2);
            SelectAction(true, 1, 1, 1);

            SelectAction(false, 0, 0, 2);
            SelectAction(false, 1, 1, 3);
            SelectAction(false, 2, 1, 0);
            SelectAction(false, 3, 0, 0);
        }
        private void StartActions()
        {
            for (int i = 0; i < maxTeamNum * 2; i++)
            {
                if (phaseActionList[i] == null) continue;
                else if (phaseActionList[i].GetType() == typeof(TauntAction)) phaseActionList[i].DoAction();
            }
            for (int i = 0; i < maxTeamNum * 2; i++)
            {
                if (phaseActionList[i] == null) continue;
                else if (phaseActionList[i].GetType() != typeof(TauntAction)) phaseActionList[i].DoAction();
            }
        }

        private void SelectAction(bool isServant, int characterIndex, int actionIndex, int opponentIndex)
        {
            // 순서 구현은 하기나름
            // phaseActionList 순서대로 실행됨
            if (isServant)
            {
                if (servants[characterIndex].Actions.Count <= actionIndex) return;   //TODO error
                phaseActionList[characterIndex] = servants[characterIndex].Actions[actionIndex];
                servants[characterIndex].SetSingleTarget(enemies[opponentIndex]);
            }
            else
            {
                if (enemies[characterIndex].Actions.Count <= actionIndex) return;   //TODO error
                phaseActionList[characterIndex + maxTeamNum] = enemies[characterIndex].Actions[actionIndex];
                enemies[characterIndex].SetSingleTarget(servants[opponentIndex]);
            }
        }

        private void Mission()
        {

        }
    }
}

