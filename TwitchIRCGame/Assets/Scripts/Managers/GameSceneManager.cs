using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField]
        private GameScene currentScene;

        public GameScene CurrentScene
        {
            get { return currentScene; }
            private set { currentScene = value; }
        }

        public bool TryLoadScene(GameScene sceneType)
        {
            try
            {
                CurrentScene = sceneType;
                switch(sceneType)
                {
                    case GameScene.Main:
                        SceneManager.LoadScene("MainScene");
                        break;
                    case GameScene.InBattle:
                        SceneManager.LoadScene("InBattleScene");
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}