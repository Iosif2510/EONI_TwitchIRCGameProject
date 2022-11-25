using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TwitchIRCGame
{
    public class BattleUI : MonoBehaviour
    {

        // for summoner action/enemy button
        private int selectedActionIndex = NOT_SELECTED;
        private int selectedTargetIndex = NOT_SELECTED;

        private const int SUMMONER = -1;
        private const int NOT_SELECTED = -2;

        [SerializeField]
        private List<TMP_Text> actionNameTextObjects;
        [SerializeField]
        private List<TMP_Text> characterNameTextObjects;

        // Start is called before the first frame update
        void Start()
        {
            int numAllies = GameManager.Battle.servants.Count + 1;
            for (int i = 0; i < numAllies; i++)
            {
                List<CharacterAction> actions;
                if (i == 0)
                {
                    actions = GameManager.Battle.summoner.Actions;
                    characterNameTextObjects[i].text = GameManager.Battle.summoner.Name;
                }
                else
                {
                    actions = GameManager.Battle.servants[i-1].Actions;
                    characterNameTextObjects[i].text = GameManager.Battle.servants[i-1].Name;                    
                }

                for (int j = 0; j < actions.Count; j++)
                {
                    actionNameTextObjects[i*3 + j].text = actions[j].ActionName;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < GameManager.Battle.servants.Count; i++)
            {
                characterNameTextObjects[i + 1].text = GameManager.Battle.servants[i].Name;

                if (GameManager.Battle.servantActionList[i] != null)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // 준내 임시로 짬
                        // 같은 행동 두개 이상이 슬롯에 들어가있는 경우는 고려 안함
                        string selectedAction = GameManager.Battle.servantActionList[i].ActionName;
                        string currentAction = GameManager.Battle.servants[i].Actions[j].ActionName;

                        if (selectedAction == currentAction)
                            actionNameTextObjects[selectedActionIndex].color = Color.red;
                        else
                            actionNameTextObjects[selectedActionIndex].color = Color.white;
                    }
                }
            }
        }

        private void ToggleSummonerAction(int actionIndex)
        {
            if (GameManager.Battle.summoner.Actions.Count < actionIndex)
            {
                return;
            }

            // 기존 행동 선택 해제 표시 (하얀색)
            if (selectedActionIndex != NOT_SELECTED)
                actionNameTextObjects[selectedActionIndex].color = Color.white;

            // 행동 지정
            if (selectedActionIndex != actionIndex - 1) // 선택
            {
                selectedActionIndex = actionIndex - 1; // 인터페이스상 index는 1-indexed, 실제 구현된 index는 0-indexed
                GameManager.Battle.summonerAction = GameManager.Battle.summoner.Actions[selectedActionIndex];
            }
            else // 선택 해제
            {
                selectedActionIndex = NOT_SELECTED;
                GameManager.Battle.summonerAction = null;
            }
            
            // 기존 행동 선택 표시 (빨간색)
            if (selectedActionIndex != NOT_SELECTED)
                actionNameTextObjects[selectedActionIndex].color = Color.red;
        }

        /// <param name="targetIndex">
        /// 양수이면 아군 (1, 2, 3 -> 사역마 1, 2, 3),
        /// 음수이면 적군 (-1, -2, -3 -> 적 1, 2, 3),
        /// 0이면 소환사 자신을 의미합니다.
        /// </param>
        private void SetSummonerTarget(int targetIndex)
        {
            if (selectedActionIndex == NOT_SELECTED)
                return;

            CharacterAction selectedAction = GameManager.Battle.summoner.Actions[selectedActionIndex];
            bool isTargetless = !selectedAction.IsTargeted;
            bool isInputTargetEnemy = targetIndex < 0;
            bool isRequiredTargetEnemy = selectedAction.IsTargetOpponent;

            bool doesInputTargetExist = true;
            if (isInputTargetEnemy)
                doesInputTargetExist = GameManager.Battle.enemies.Count >= -targetIndex;
            else if (targetIndex != SUMMONER)
                doesInputTargetExist = GameManager.Battle.servants.Count >= targetIndex;

            // 대상이 없거나 전체가 대상인 행동일 경우 무효
            if (isTargetless)
            {
                GameManager.Battle.summoner.ClearTarget();
                return;
            }

            // 잘못된 대상을 선택한 경우 무효
            if (isInputTargetEnemy != isRequiredTargetEnemy || !doesInputTargetExist)
                return;

            // 인터페이스상 index는 1-indexed, 실제 구현된 index는 0-indexed
            // 소환사는 -1로 표현됨
            selectedTargetIndex = (isInputTargetEnemy ? -targetIndex : targetIndex) - 1;
            
            // 대상 지정
            if (isInputTargetEnemy)
            {
                GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.enemies[selectedTargetIndex]);
            }
            else if (selectedTargetIndex == SUMMONER)
            {
                GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.summoner);
            }
            else
            {
                GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.servants[selectedTargetIndex]);
            }
        }

        private void EndTurn()
        {
            bool isActionSelected = selectedActionIndex != NOT_SELECTED;
            bool isActionTargeted = isActionSelected && GameManager.Battle.summonerAction.IsTargeted;
            bool isTargetSelected = selectedTargetIndex != NOT_SELECTED; 
            if (isActionTargeted && !isTargetSelected)
            {
                Debug.Log("Turn not ended; please select the target");
            }
            else
            {
                StartCoroutine(_EndTurn());
            }
        }

        private IEnumerator _EndTurn()
        {
            yield return GameManager.Battle.EndTurn();
                
            // 초기화
            if (selectedActionIndex != NOT_SELECTED)
                actionNameTextObjects[selectedActionIndex].color = Color.white;
            selectedActionIndex = NOT_SELECTED;
            selectedTargetIndex = NOT_SELECTED;
        }
    }

}
