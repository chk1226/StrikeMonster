using UnityEngine;
using System.Collections;
//using UnityEngine.EventSystems;


namespace StrikeMonster
{
    public class ActiveSkillComponent : MonoBehaviour {

        public UnityEngine.UI.Text SkillReady;
        public UnityEngine.UI.Text ActiveSkillCD;
        public bool CanSkill = false;
        public HeroComponent Hero;
 
        private CDPropertyComponent m_HeroCD;
        private RectTransform m_rectTransform;

    	// Use this for initialization
    	void Start () {
            m_rectTransform = this.gameObject.GetComponent<RectTransform>();
    	}

    	// Update is called once per frame
    	void Update () {
            if (m_HeroCD && m_HeroCD.Value <= 0 && 
                m_HeroCD.TextIndicatorComponent && m_HeroCD.TextIndicatorComponent.IsActive())
            {
                if (SkillReady)
                {
                    SkillReady.gameObject.SetActive(true);
                }
            } else
            {
                if(SkillReady)
                {
                    SkillReady.gameObject.SetActive(false);
                }
            }


    	}

        public void Initialize(HeroComponent hero)
        {
            Hero = hero;
            if (hero.ActiveSkill && hero.ActiveSkill.CDProperty.Max > 0)
            {
                m_HeroCD = hero.ActiveSkill.CDProperty;

                if(ActiveSkillCD)
                {
                    ActiveSkillCD.gameObject.SetActive(true);
                    m_HeroCD.TextIndicatorComponent = ActiveSkillCD;
                    m_HeroCD.UpdateIndicator();
                }

            } else
            {
                if(ActiveSkillCD)
                {
                    ActiveSkillCD.gameObject.SetActive(false);
                }
            }

        }


        private bool IsInsideTouchZone(Vector3 touchPos)
        {
          
            if (!m_rectTransform)
            {
              return false;
            }
          
            var center = Camera.main.WorldToScreenPoint(m_rectTransform.position);   

            float x = center.x - touchPos.x;
            float y = center.y - touchPos.y;
            float length = x * x + y * y;

            x = m_rectTransform.offsetMax.x - center.x;


            return length <= (x * x);
          
          
        }
       

        public void OnPointerClick ()
        {
            if (SkillReady && SkillReady.IsActive())
            {

                if(IsInsideTouchZone(Input.mousePosition) && GamePlaySettings.Instance.CanStrike && TeamComponent.Instance.CurrentHero == Hero)
                {
                    GameFlowComponent.Instance.GameFlowFSM.SendEvent(GameFlowComponent.Instance.CastActiveSkillEvent);
                    
                    SpineControlComponent.Instance.LoadSpineData(Hero.SpineData, delegate(){
                        Hero.CastActiveSkill();
                        GameFlowComponent.Instance.GameFlowFSM.SendEvent(GameFlowComponent.Instance.DoneEvent);
                    });

                }
                        

            } 
 
            
        }


//        IEnumerator AnimationOutlineColor()
//        {
//            float indexOffset = 0.024f;
//            float index = 0;
//            bool inverse = false;
//            Color color = Color.white;
//
//            while (CanSkill)
//            {
//                if(!inverse)
//                {
//                    index = Mathf.Min(index + indexOffset, 3.0f);
//                }
//                else
//                {
//                    index = Mathf.Max(index - indexOffset, 0.0f);
//                }
//
//                color.r = (index <= 1 || index >= 0) ? Mathf.Lerp(0.0f, 0.55f, index) : (inverse) ? 0 : 1;
//                color.g = (index <= 2 || index >= 1) ? Mathf.Lerp(0.2f, 0.8f, index - 1) : (inverse) ? 0 : 1;
//                color.b = (index <= 3 || index >= 2) ? Mathf.Lerp(0.25f, 1.0f, index - 2) : (inverse) ? 0 : 1;
//                
//                if(Outline)
//                {
//                    if(Outline.Image)
//                    {
//                        Outline.Image.color = color;
//
//                    }
//
//                    Outline.TexOffset = 0.03f;
//                }
//
//
//                if(index >= 1)
//                {
//                    inverse = true;
//                }
//                else if(index <= 0)
//                {
//                    inverse = false;
//                }
//
//                yield return null;
//
//            }
//        }
        
        
        
    }

}