using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{
    public class DirectionRaySkill : BaseSkill {



        private List<List<float>> hitTime;
        private System.Random rnd = new System.Random();
        private bool waitFire = false;

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);
            waveNumber = 1;

            if(rayEmitter != null && rayEmitter.Count != 0)
            {
                hitTime = new List<List<float>>();

                waveNumber = rayEmitter.Count;


                Quaternion rotation = Quaternion.Euler(90, 0, 0);
                for( int i = 0; i < rayEmitter.Count; i++)
                {
                    
                    rayEmitter[i].transform.rotation = rotation;
                    rayEmitter[i].startSize = size;
                }


                
            }


        }



        private void RestHitTime()
        {
//            hitTime.Clear();
//            
//            if (Targets != null)
//            {
//                for(int i = 0; i < Targets.Count; i++)
//                {
//                    hitTime.Add(0);
//                }
//                
//            }
            
        }




    }


}