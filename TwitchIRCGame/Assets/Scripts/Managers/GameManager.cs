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

        public static UIManager UI 
        {
            get
            {
                if (Instance.uiManager == null) 
                {
                    Instance.uiManager = Instance.gameObject.GetComponent<UIManager>();
                }
                return Instance.uiManager;
            }
        }
        public static BattleManager Battle
        {
            get
            {
                {
                    if (Instance.battleManager == null)
                    {
                        Instance.battleManager = Instance.gameObject.GetComponent<BattleManager>();
                    }
                    return Instance.battleManager;
                }
            }
        }

        // nth player: playerTeam[playerNames[n]]
        public List<string> servantIDs;
        public Dictionary<string, Servant> servantTeam;

        private void Awake()
        {
            
            DataInit();
            //uiManager = gameObject.GetComponent<UIManager>();
            //battleManager = gameObject.GetComponent<BattleManager>();
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
            servantIDs = new List<string>(4);
            servantTeam = new Dictionary<string, Servant>(4);
        }

        
    }

}
