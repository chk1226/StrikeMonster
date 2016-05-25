using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    public class ThrustComponent : MonoBehaviour {

        public HeroComponent Hero;
        public float ForceScale = 1f;
        private Vector2 m_Force = Vector2.one; 

        public delegate void ThrustCallback(Collider2D coll, Vector2 thrustDir);
        public event ThrustCallback ThrustEvent;


    	// Use this for initialization
    	void Start () {
    	
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}


        void OnTriggerEnter2D(Collider2D coll)
        {
            var force = (this.transform.position - coll.transform.position).normalized;
            m_Force.x = force.x * ForceScale;
            m_Force.y = force.y * ForceScale;

            if(ThrustEvent != null)
            {
                ThrustEvent(coll, m_Force);
            }

            //Thrust hero
            var actionHero = coll.GetComponent<HeroComponent>();
            if (actionHero && actionHero == TeamComponent.Instance.CurrentHero &&
                GamePlaySettings.Instance.IsActionStrike)
            {
                Hero.Rigidbody_2D.AddForce(m_Force, ForceMode2D.Impulse);
                
            }


//            Debug.Log("[OnTriggerEnter2D] Trigger!" + Hero.name);
            
        }


    }


}