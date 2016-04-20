using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

    public class BottomUIComponent : MonoBehaviour {

        public List<GameObject> HeroSlotList;
        public UnityEngine.UI.Text OurTurnCounterText;

        private string m_OurTurnCounterFormat = string.Empty;

    	
    	// Update is called once per frame
    	void Update () {
    	
    	}

        public void Initialize()
        {
            InitializeHeroSlot();
            if(OurTurnCounterText)
            {
                m_OurTurnCounterFormat = OurTurnCounterText.text;
            }
        }
        
        private void InitializeHeroSlot()
        {
            if (HeroSlotList == null)
            {
                return;
            }


            if(TeamComponent.Instance.Team.Count > HeroSlotList.Count)
            {
                return;
            }


            for(int i = 0; i < HeroSlotList.Count; i ++)
            {
                var cd = HeroSlotList[i].GetComponent<CDPropertyComponent>();
                if(cd)
                {
                    cd.EnableCDText(false);
                }
                
            }



            for (int i = 0; i < TeamComponent.Instance.Team.Count; i ++)
            {
                var hero = TeamComponent.Instance.Team[i] as HeroComponent;

                // set icon
                var icon = HeroSlotList[i].GetComponent<UnityEngine.UI.Image>();
                icon.sprite = hero.Icon.sprite;

                // set active skill cd
                var cd = HeroSlotList[i].GetComponent<CDPropertyComponent>();
                if(cd)
                {
                    if(hero.MaxActiveSkillCD > 0)
                    {
                        cd.Initialize(hero.MaxActiveSkillCD, 0, hero.MaxActiveSkillCD);
                        cd.EnableCDText(true);
                    }
                    else
                    {
                        cd.EnableCDText(false);
                    }
                }
            }

        }

        public void UpdateOurActiveCounter()
        {
            if(m_OurTurnCounterFormat != string.Empty)
            {
                OurTurnCounterText.text = string.Format(m_OurTurnCounterFormat, GameFlowComponent.Instance.OurTurnCounter);

            }
        }


    }

}