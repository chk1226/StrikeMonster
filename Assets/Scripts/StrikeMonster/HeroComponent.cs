using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    public class HeroComponent : UnitComponent {

        public string Name;
        public Rigidbody2D Rigidbody_2D;
        public CircleCollider2D CircleCollider_2D;


//        [SerializeField]
        private float m_Speed;


        public override void Initialize (UnitInfo baseInfo)
        {
            var heroInfo = baseInfo as HeroInfo;
            if (heroInfo != null)
            {
                m_Speed = heroInfo.Speed;
                Name = heroInfo.Name;

                base.Initialize (baseInfo);
            }

        }


        public void GiveVelocity(Vector2 velocity)
        {
            if (Rigidbody_2D)
            {
                Rigidbody_2D.velocity = velocity * m_Speed;
            }
        }

        public void GiveForce(Vector2 f)
        {
            if (Rigidbody_2D)
            {
                Rigidbody_2D.AddForce(f* m_Speed);
            }
        }


        public override void OnHurt(float damage)
        {
            if(TeamComponent.Instance.HPProperty)
            {
                TeamComponent.Instance.HPProperty.Value -= damage;

            }

            base.OnHurt(damage);
        }


        public override void HandlePhysics()
        {
            if(Rigidbody_2D)
            {
                if(GamePlaySettings.Instance.IsPlayerRound && Rigidbody_2D.velocity.magnitude < GamePlaySettings.Instance.ThresholdVelocity)
                {
                    Rigidbody_2D.velocity = Vector2.zero;
                }

            }
        }


    	void OnCollisionEnter2D(Collision2D coll) {

            Debug.Log("[OnCollisionEnter2D] hit!" + coll.gameObject.name);

    	}

        void OnTriggerEnter2D(Collider2D coll)
        {
            Debug.Log("[OnTriggerEnter2D] Trigger!" + coll.gameObject.name);
            
        }


    }


}