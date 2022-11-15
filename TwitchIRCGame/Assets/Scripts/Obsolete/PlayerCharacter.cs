using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TwitchIRCGame
{
    public class PlayerCharacter : MonoBehaviour
    {
        private string playerName;
        private TextMeshProUGUI nameTextUI;
        public int teamNumber;

        public string Name
        {
            get { return playerName; }
            set
            {
                nameTextUI.text = value;
                playerName = value;
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            GetComponentInChildren<Canvas>().worldCamera = GameManager.UI.mainCamera;
            nameTextUI = GetComponentInChildren<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
