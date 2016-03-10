using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class RaySkill : BaseSkill {
        
        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);
            waveNumber = 1;

            if(rayEmitter != null && rayEmitter.Count != 0)
            {
                float angle = 360 / rayEmitter.Count;
                Quaternion rotation = Quaternion.Euler(90, 0, 0);

                for( int i = 0; i < rayEmitter.Count; i++)
                {

                    rayEmitter[i].transform.rotation = Quaternion.Euler(0, 0, i * angle) * rotation;
//                    rayEmitter[i].enableEmission
                }

            }


            
        }


        IEnumerator EnableEmission()
        {
            foreach(var ray in rayEmitter)
            {

                ray.enableEmission = true;

            }


            yield return new WaitForSeconds(lifeTime);
            

            foreach(var ray in rayEmitter)
            {
                
                ray.enableEmission = false;
                
            }
        }


        protected override void AttackBehavior()
        {






        }





    	
    }


}