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

        // Start is called before the first frame update
        void Start()
        {
            int numAllies = GameManager.Battle.servants.Count + 1;
            for (int i = 0; i < numAllies; i++)
            {
                List<CharacterAction> actions;
                if (i == 0)
                    actions = GameManager.Battle.summoner.Actions;
                else
                    actions = GameManager.Battle.servants[i-1].Actions;
                
                for (int j = 0; j < actions.Count; j++)
                {
                    actionNameTextObjects[i*3 + j].text = actions[j].ActionName;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ToggleSummonerAction(int actionIndex)
        {
            if (GameManager.Battle.summoner.Actions.Count < actionIndex)
            {
                Debug.Log($"Action {actionIndex} is not in the slot");
                return;
            }

            // 기존 행동 선택 해제 (하얀색)
            if (selectedActionIndex != NOT_SELECTED)
                actionNameTextObjects[selectedActionIndex].color = Color.white;

            if (selectedActionIndex != actionIndex - 1) // 선택
            {
                selectedActionIndex = actionIndex - 1; // 인터페이스상 index는 1-indexed, 실제 구현된 index는 0-indexed
                Debug.Log($"Action {actionIndex} selected");
            }
            else // 선택 해제
            {
                selectedActionIndex = NOT_SELECTED;
                Debug.Log($"Action {actionIndex} deselected");
            }
            
            // 기존 행동 선택 (빨간색)
            if (selectedActionIndex != NOT_SELECTED)
                actionNameTextObjects[selectedActionIndex].color = Color.red;
        }

        /// <param name="targetIndex">
        /// 양수이면 아군 (1, 2, 3 -> 사역마 1, 2, 3),
        /// 음수이면 적군 (-1, -2, -3 -> 적 1, 2, 3),
        /// 0이면 소환사 자신을 의미합니다.
        /// </param>
        public void SetSummonerTarget(int targetIndex)
        {
            if (selectedActionIndex == NOT_SELECTED)
                return;

            CharacterAction selectedAction = GameManager.Battle.summoner.Actions[selectedActionIndex];
            bool isTargetless = !selectedAction.IsTargeted;
            bool isInputTargetEnemy = targetIndex < 0;
            bool isRequiredTargetEnemy = selectedAction.IsTargetOpponent;

            bool isInputTargetAlive = true;
            /* 현재 단계에서 enemies와 servants가 빈 리스트라서 잠시 꺼둠
            if (isInputTargetEnemy)
                isInputTargetAlive = GameManager.Battle.enemies.Count < -targetIndex;
            else if (targetIndex != SUMMONER)
                isInputTargetAlive = GameManager.Battle.servants.Count < targetIndex;
            */

            // 대상이 없거나 전체가 대상인 행동일 경우 무효
            if (isTargetless)
                return;

            // 잘못된 대상을 선택한 경우 무효
            if (isInputTargetEnemy != isRequiredTargetEnemy || !isInputTargetAlive)
                return;

            // 인터페이스상 index는 1-indexed, 실제 구현된 index는 0-indexed
            // 소환사는 -1로 표현됨
            selectedTargetIndex = (isInputTargetEnemy ? -targetIndex : targetIndex) - 1;

            if (isInputTargetEnemy)
                Debug.Log($"Target: Enemy {-targetIndex}");
            else if (selectedTargetIndex == SUMMONER)
                Debug.Log($"Target: Summoner");
            else
                Debug.Log($"Target: Servant {targetIndex}");
        }

        public void EndTurn()
        {
            if (SelectAction())
            {
                Debug.Log("Turn ended");
                GameManager.Battle.EndTurn();

                // 초기화
                if (selectedActionIndex != NOT_SELECTED)
                    actionNameTextObjects[selectedActionIndex].color = Color.white;
                selectedActionIndex = NOT_SELECTED;
                selectedTargetIndex = NOT_SELECTED;
            }
            else
            {
                Debug.Log("Turn not ended; please select the target");
            }
        }

        /// <returns>
        /// 행동은 선택했는데 대상이 제대로 선택되지 않은 경우 false를 반환합니다.
        /// 그 외에는 true를 반환합니다.
        /// </returns>
        private bool SelectAction()
        {
            if (selectedActionIndex == NOT_SELECTED)
            {
                GameManager.Battle.summonerAction = null;
                return true;
            }

            // 행동 선택
            CharacterAction selectedAction = GameManager.Battle.summoner.Actions[selectedActionIndex];
            GameManager.Battle.summonerAction = selectedAction;

            // 대상 선택
            if (selectedAction.IsTargeted)
            {
                if (selectedTargetIndex == NOT_SELECTED) return false;      // 타겟 공격일때만 타겟을 결정
                else if (selectedAction.IsTargetOpponent)
                {
                    // 적군 선택
                    GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.enemies[selectedTargetIndex]);
                }
                else
                {
                    // 아군 선택
                    if (selectedTargetIndex == SUMMONER)
                        GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.summoner);
                    else
                        GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.servants[selectedTargetIndex]);
                }
            }
            
            return true;
        }
    }

}
