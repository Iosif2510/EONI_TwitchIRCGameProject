using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchIRCGame
{
    public class GameManager : MonoBehaviour
    {

        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                SingletonInit();
                return instance;
            }
        }
        
        private BattleManager battleManager;
        private UIManager uiManager;

        public static UIManager UI { get => Instance.uiManager; }
        public static BattleManager Battle { get => Instance.battleManager; }

        // nth player: playerTeam[playerNames[n]]
        public List<string> playerIDs;
        public Dictionary<string, PlayerCharacter> playerTeam;

        private void Awake()
        {
            SingletonInit();
            uiManager = gameObject.GetComponent<UIManager>();
            battleManager = gameObject.GetComponent<BattleManager>();
        }

        private static void SingletonInit()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<GameManager>();
                }
                instance = go.GetComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
        }

        private void DataInit()
        {
            playerIDs = new List<string>(4);
            playerTeam = new Dictionary<string, PlayerCharacter>(4);
        }

        
    }

}
