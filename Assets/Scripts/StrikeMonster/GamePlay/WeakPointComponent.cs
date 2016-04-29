using UnityEngine;
using System.Collections;

namespace StrikeMonster
{

    public class WeakPointComponent : MonoBehaviour {

        public EnemyComponent Self;


    	// Use this for initialization
    	void Start () {
    	
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}

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
            
            Debug.Log("[OnTriggerEnter2D] WeakPointComponent!");
            
        }
    }


}