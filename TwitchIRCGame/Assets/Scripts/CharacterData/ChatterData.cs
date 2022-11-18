using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRCGame
{
    [Serializable]
    public class ChatterData    // 트위치 사용자에 종속된 데이터
    {
        protected string chatterID;
        public string ChatterID => chatterID;

        public ChatterData(string chatterID)
        {
            this.chatterID = chatterID;
        }
    }
}
