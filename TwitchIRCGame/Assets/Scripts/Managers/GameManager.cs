using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TwitchIRCGame.Define;

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

        //! 프로퍼티로만 접근할 것
        private static GameObject currentSceneManager;
        private UIManager uiManager;
        private GameSceneManager sceneManager;

        private BattleManager battleManager;

        //모든 씬에서 사용하는 매니저 모음
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

        public static GameSceneManager Scene
        {
            get
            {
                if (Instance.sceneManager == null)
                {
                    Instance.sceneManager = Instance.gameObject.GetComponent<GameSceneManager>();
                }
                return Instance.sceneManager;
            }
        }

        //현재 씬에서만 사용하는 매니저 모음
        public static GameObject CurrentSceneManager
        {
            get
            {
                if (currentSceneManager == null)
                {
                    currentSceneManager = GameObject.Find("@SceneManager");
                }
                return currentSceneManager;
            }
        }
        public static BattleManager Battle
        {
            get
            {
                {
                    if (Scene.CurrentScene == GameScene.InBattle)
                    {
                        if (Instance.battleManager == null)
                        {
                            Instance.battleManager = CurrentSceneManager.GetComponent<BattleManager>();
                        }
                        return Instance.battleManager;
                    }
                    else
                    {
                        throw new System.NotImplementedException();
                    }
                }
            }
        }



        // nth player: playerTeam[playerNames[n]]
        public Summoner summoner;
        public List<string> servantIDs;
        public Dictionary<string, Servant> servantTeam;

        private void Awake()
        {
            DataInit();
        }

        private static void SingletonInit()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@MasterManager");
                if (go == null)
                {
                    go = new GameObject { name = "@MasterManager" };
                    DontDestroyOnLoad(go);
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
        
        public void GameOver()
        {
            Debug.Log("Game Over!");
        }

        public void CreateServant(string newID)
        {
            servantIDs.Add(newID);
            //newServant = new Servant();
            //servantTeam.Add(newID, newServant);
        }

        public string ServantDelete(Servant servant)
        {
            string deleteID = servant.ChatterID;
            if (servantIDs.Contains(deleteID))
            {
                //servantTeam[deleteID].
                servantIDs.Remove(deleteID);
                servantTeam.Remove(deleteID);
            }
            return deleteID;
        }
        
    }

}
