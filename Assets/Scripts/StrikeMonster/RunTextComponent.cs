using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class RunTextComponent : MonoBehaviour {

        public float InTime = 1;
        public float WaitTime = 0.15f;
        public float OutTime = 0.1f;

        public float HalfTextWidth
        {
            get;
            set;
        }

        public char Char
        {
            set
            {
                this.GetComponent<UnityEngine.UI.Text>().text = value.ToString();
            }
        }

        public Color CharColor
        {
            set
            {
                var text = this.GetComponent<UnityEngine.UI.Text>();
                value.a = text.color.a;
                text.color = value;
            }
        }

        private float OutOffset = -55f;

    	// Use this for initialization
    	void Start () {        
            var locPos = this.transform.localPosition;
            locPos.x = Screen.width/2;
            this.transform.localPosition = locPos;

            StartCoroutine(AdjustWidth());
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}

        private void AnimationComplete()
        {
            iTween.Stop(this.gameObject);
            Destroy(this.gameObject);
        }


        private IEnumerator AdjustWidth()
        {
            yield return null;

            var transformCmp = this.GetComponent<RectTransform>();
            float w;
            if (this.GetComponent<UnityEngine.UI.Text>().text != " ")
            {
                w = UnityEngine.UI.LayoutUtility.GetPreferredWidth(transformCmp);
            } else
            {
                w = 20;
            }
            transformCmp.sizeDelta = new Vector2(w, transformCmp.sizeDelta.y);

        }



        public void PlayTween()
        {
            float to = this.transform.localPosition.x - Screen.width/2 - HalfTextWidth;


            iTween.MoveTo(this.gameObject, iTween.Hash(
                "x", to,
                "time", InTime,
                "islocal", true
                ));
            iTween.ColorTo(this.gameObject, iTween.Hash(
                "a", 1.0f,
                "time", InTime,
                "easetype", iTween.EaseType.easeOutQuad
                ));

            iTween.MoveTo(this.gameObject, iTween.Hash(
                "x", to + OutOffset,
                "time", OutTime,
                "delay", InTime + WaitTime,
                "islocal", true,
                "oncomplete","AnimationComplete",
                "onupdatetarget", this.gameObject
                ));
            iTween.ColorTo(this.gameObject, iTween.Hash(
                "a", 0f,
                "time", OutTime,
                "delay", InTime + WaitTime,
                "easetype", iTween.EaseType.easeOutQuad
                ));


        }



    }


}