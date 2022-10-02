using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TwitchIRCGame
{
    public class Player : MonoBehaviour
    {
        //private IRCTags ircTag;
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


        //public IRCTags Tag
        //{
        //    get { return ircTag; }
        //    set
        //    {
        //        ircTag = value;
        //        this.Name = ircTag.displayName;
        //    }
        //}

        // Start is called before the first frame update
        void Awake()
        {
            GetComponentInChildren<Canvas>().worldCamera = UIManager.Instance.mainCamera;
            nameTextUI = GetComponentInChildren<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
