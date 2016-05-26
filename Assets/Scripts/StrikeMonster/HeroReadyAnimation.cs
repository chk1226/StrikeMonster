using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    public class HeroReadyAnimation : MonoBehaviour {
        public DrawCircle Circle;

        [HideInInspector]
        public float StartRadius;


//    	// Use this for initialization
    	void Start () {
            StartRadius = 10;
    	}
//    	
//    	// Update is called once per frame
//    	void Update () {
//    	
//    	}

        private void CircleStart()
        {
            Circle.Radius = StartRadius;
            Circle.EnableCircle = true;
        }

        private void CircleUpdate( float value )
        {
            Circle.Radius = value;
        }

        private void CircleEnd()
        {
            Circle.IsKinematic = true;
        }


        public void StartAnimation()
        {

            iTween.ValueTo( this.gameObject, iTween.Hash(
                "from", StartRadius,
                "to", 0.6f,
                "time", 0.15f,
                "onstart", "CircleStart",
                "onupdate", "CircleUpdate"
                ));

        }

        public void StopAnimation()
        {
            iTween.Stop(this.gameObject);
            Circle.EnableCircle = false; 
            Circle.IsKinematic = false;
			Circle.Radius = 0;
        }


    }

}