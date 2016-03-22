using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class WaveInfo {

        public List<UnitInfo> EnemyInfo = new List<UnitInfo>();


        public WaveInfo(PrototypeSystem.PrototypeWaveInfo waveInfo)
        {

            foreach(var enemyInfo in waveInfo.EnemyList)
            {

                EnemyInfo.Add(new EnemyInfo(enemyInfo));

            }


        }

    }



}