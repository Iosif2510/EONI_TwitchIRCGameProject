using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class BattleUI : MonoBehaviour
    {

        // for summoner action/enemy button
        private int selectedActionIndex = NOT_SELECTED;
        private int selectedTargetIndex = NOT_SELECTED;

        private const int SUMMONER = -1;
        private const int NOT_SELECTED = -2;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ToggleSummonerAction(int actionIndex)
        {
            if (GameManager.Battle.summoner.Actions.Count < actionIndex)
            {
                Debug.Log("Action " + actionIndex + " is not in the slot");
                return;
            }

            if (selectedActionIndex != actionIndex - 1) // ����
            {
                selectedActionIndex = actionIndex - 1; // �������̽��� index�� 1-indexed, ���� ������ index�� 0-indexed
                Debug.Log("Action " + actionIndex + " selected");
            }
            else // ���� ����
            {
                selectedActionIndex = NOT_SELECTED;
                Debug.Log("Action " + actionIndex + " deselected");
            }
        }

        /// <param name="targetIndex">
        /// ����̸� �Ʊ� (1, 2, 3 -> �翪�� 1, 2, 3),
        /// �����̸� ���� (-1, -2, -3 -> �� 1, 2, 3),
        /// 0�̸� ��ȯ�� �ڽ��� �ǹ��մϴ�.
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
            /* ���� �ܰ迡�� enemies�� servants�� �� ����Ʈ�� ��� ����
            if (isInputTargetEnemy)
                isInputTargetAlive = GameManager.Battle.enemies.Count < -targetIndex;
            else if (targetIndex != SUMMONER)
                isInputTargetAlive = GameManager.Battle.servants.Count < targetIndex;
            */

            // ����� ���ų� ��ü�� ����� �ൿ�� ��� ��ȿ
            if (isTargetless)
                return;

            // �߸��� ����� ������ ��� ��ȿ
            if (isInputTargetEnemy != isRequiredTargetEnemy || !isInputTargetAlive)
                return;

            // �������̽��� index�� 1-indexed, ���� ������ index�� 0-indexed
            // ��ȯ��� -1�� ǥ����
            selectedTargetIndex = (isInputTargetEnemy ? -targetIndex : targetIndex) - 1;

            if (isInputTargetEnemy)
                Debug.Log("Target: Enemy " + (-targetIndex));
            else if (selectedTargetIndex == SUMMONER)
                Debug.Log("Target: Summoner");
            else
                Debug.Log("Target: Servant " + targetIndex);
        }

        public void EndTurn() // �� ���� ��ư
        {
            if (SelectAction())
            {
                Debug.Log("Turn ended");
                GameManager.Battle.EndTurn();

                // �ʱ�ȭ
                selectedActionIndex = NOT_SELECTED;
                selectedTargetIndex = NOT_SELECTED;
            }
            else
            {
                Debug.Log("Turn not ended; please select the target");
            }
        }

        /// <returns>
        /// �ൿ�� �����ߴµ� ����� ����� ���õ��� ���� ��� false�� ��ȯ�մϴ�.
        /// �� �ܿ��� true�� ��ȯ�մϴ�.
        /// </returns>
        private bool SelectAction()
        {
            if (selectedActionIndex == NOT_SELECTED)
            {
                GameManager.Battle.summonerAction = null;
                return true;
            }

            if (selectedTargetIndex == NOT_SELECTED)
                return false;

            // �ൿ ����
            CharacterAction selectedAction = GameManager.Battle.summoner.Actions[selectedActionIndex];
            GameManager.Battle.summonerAction = selectedAction;

            // ��� ����
            if (selectedAction.IsTargeted)
            {
                if (selectedAction.IsTargetOpponent)
                {
                    // ���� ����
                    GameManager.Battle.summoner.SetSingleTarget(GameManager.Battle.enemies[selectedTargetIndex]);
                }
                else
                {
                    // �Ʊ� ����
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
