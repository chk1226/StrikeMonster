using UnityEngine;
using System.Collections;


namespace StrikeMonster
{
    public class ActionSkillComponent : MonoBehaviour {

        public CDPropertyComponent HeroCD;




    	// Use this for initialization
    	void Start () {
    	
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
            if(HeroCD && HeroCD.Value <= 0)
            {

            }

    	}

    }

}