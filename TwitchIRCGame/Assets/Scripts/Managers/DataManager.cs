using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static TwitchIRCGame.Define;

namespace TwitchIRCGame
{
    public class DataManager : MonoBehaviour
    {
        // (to-be-updated) nth servant: servantTeam[servantIDs[n]]
        [Obsolete]
        public Summoner summoner;
        public List<string> servantIDs;
        [Obsolete]
        public Dictionary<string, Servant> servantTeam;

        public SummonerData summonerData;
        public List<ChatterData> servantChatterDatas;
        public Dictionary<ChatterData, CharacterData> servantTeamData;

        private void Awake()
        {
            DataInit();
        }

        private void DataInit()
        {
            servantIDs = new List<string>(4);
            //servantTeam = new Dictionary<string, Servant>(4);

            summonerData = new SummonerData();      // IRC로부터 id 받아올 것
            servantChatterDatas = new List<ChatterData>();
            servantTeamData = new Dictionary<ChatterData, CharacterData>();
        }

        public void CreateServantChatter(string newID)
        {
            servantIDs.Add(newID);
            ChatterData newData = new ChatterData(newID);
            servantChatterDatas.Add(newData);

        }

        public void ServantDataDelete(ChatterData chatterData)
        {
            if (servantChatterDatas.Contains(chatterData))
            {
                servantChatterDatas.Remove(chatterData);
                servantTeamData.Remove(chatterData);
            }
        }

        [Obsolete]
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
