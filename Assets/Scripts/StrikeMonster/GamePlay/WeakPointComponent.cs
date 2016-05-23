using UnityEngine;
using System.Collections;

namespace StrikeMonster
{

    public class WeakPointComponent : UnitComponent {

        public EnemyComponent Self;


        void OnCollisionEnter2D(Collision2D coll) {

            if(Self)
            {
                var hero = coll.gameObject.GetComponent<HeroComponent>();
                if (hero == TeamComponent.Instance.CurrentHero && GamePlaySettings.Instance.IsPlayerRound)
                {
                    Self.OnHurt(hero.Attack * 2);
                }

            }


        }

        void OnTriggerEnter2D(Collider2D coll)
        {
//            Debug.Log("[OnTriggerEnter2D] WeakPointComponent!");
        }


        public override void OnHurt(float damage)
        {
            Self.OnHurt(damage);
//            Debug.Log("[OnTriggerEnter2D] WeakPointComponent!");
            
        }

    }


}