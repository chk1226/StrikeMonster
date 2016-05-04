using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    public class ThrustComponent : MonoBehaviour {

        public HeroComponent Hero;
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
            var force = (this.transform.position - coll.transform.position).normalized;
            m_Force.x = force.x;
            m_Force.y = force.y;

            Hero.Rigidbody_2D.AddForce(m_Force, ForceMode2D.Impulse);
        }


    }


}