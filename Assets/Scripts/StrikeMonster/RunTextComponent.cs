using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class RunTextComponent : MonoBehaviour {

        public float To;
        public float Out;
        public float InTime = 1;
        public float WaitTime = 0.15f;
        public float OutTime = 0.1f;




    	// Use this for initialization
    	void Start () {
           
            SetupiTween();
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}

        private void AnimationComplete()
        {
            iTween.Stop(this.gameObject);
            Destroy(this.gameObject);
        }

        private void SetupiTween()
        {

            iTween.MoveTo(this.gameObject, iTween.Hash(
                "x", To,
                "time", InTime,
                "islocal", true
                ));
            iTween.ColorTo(this.gameObject, iTween.Hash(
                "a", 1.0f,
                "time", InTime,
                "easetype", iTween.EaseType.easeOutQuad
                ));

            iTween.MoveTo(this.gameObject, iTween.Hash(
                "x", Out,
                "time", OutTime,
                "delay", InTime + 0.15f,
                "islocal", true,
                "oncomplete","AnimationComplete",
                "onupdatetarget", this.gameObject
                ));
            iTween.ColorTo(this.gameObject, iTween.Hash(
                "a", 0f,
                "time", OutTime,
                "delay", InTime + 0.15f,
                "easetype", iTween.EaseType.easeOutQuad
                ));


        }



    }


}