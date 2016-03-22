using UnityEngine;
using System.Collections;


namespace StrikeMonster
{

    public class DamageWallSkill : BaseSkill {

        public LineRenderer DamaeRay;
        private bool waitFire = false;
        
        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);
        }


        IEnumerator FrieRay()
        {

            GameObject wall = WallComponent.Instance.RandomWall;
            var wallCollider = wall.GetComponent<BoxCollider2D>();

            if (wallCollider)
            {
                Bounds wallBounds;
                wallBounds = wallCollider.bounds;

                float currentTime = lifeTime;
                waitFire = true;

                DamaeRay.gameObject.SetActive(true);
                DamaeRay.SetPosition(0, this.transform.position);
                DamaeRay.SetColors(normalColor * 0.5f, normalColor);


                DamageWallComponent dw = wall.GetComponent<DamageWallComponent>();
                if(dw)
                {
                    dw.Reset(1);
                }
                else
                {
                    dw = wall.AddComponent<DamageWallComponent>();
                    dw.Initialize(1, damage, lifeTime);
                }


                while(currentTime >= 0)
                {
                    
                    float rate = currentTime / lifeTime;
                    var lineEnd = Vector3.Lerp(wallBounds.max, wallBounds.min, rate);
                    DamaeRay.SetPosition(1, lineEnd);
                    
                    
                    currentTime -= GamePlaySettings.Instance.GetDeltaTime();
                    
                    
                    yield return null;
                    
                }
                DamaeRay.SetPosition(1, wallBounds.max);
                
                yield return null;

                DamaeRay.gameObject.SetActive(false);
                waitFire = false;
      
            }

            

        }
        

        protected override void RecoveryReady()
        {

            if (waitFire)
            {
                return;
            }

            base.RecoveryReady();

        }


        protected override void AttackBehavior()
        {
            StartCoroutine(FrieRay());
        }



    }



}