using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    public class ThrustComponent : MonoBehaviour {

        public HeroComponent Hero;
        public float ForceScale = 1f;
        private Vector2 m_Force = Vector2.one; 

        public delegate void ThrustCallback(Collider2D coll);
        public event ThrustCallback ThrustEvent;


    	// Use this for initialization
    	void Start () {
    	
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}


        void OnTriggerEnter2D(Collider2D coll)
        {
            if(ThrustEvent != null)
            {
                ThrustEvent(coll);
            }


            //Thrust hero
            var actionHero = coll.GetComponent<HeroComponent>();
            if (actionHero && actionHero == TeamComponent.Instance.CurrentHero &&
                GamePlaySettings.Instance.IsActionStrike)
            {

                var force = (this.transform.position - coll.transform.position).normalized;
                m_Force.x = force.x * ForceScale;
                m_Force.y = force.y * ForceScale;
                
                Hero.Rigidbody_2D.AddForce(m_Force, ForceMode2D.Impulse);

                
            }


//            Debug.Log("[OnTriggerEnter2D] Trigger!" + Hero.name);
            
        }


    }


}