using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

    public class BottomUIComponent : MonoBehaviour {

        public List<GameObject> HeroSlotList;
        public UnityEngine.UI.Text OurTurnCounterText;

        private string m_OurTurnCounterFormat = string.Empty;
//        private List<ActiveSkillComponent> ActiveSkillUI = new List<ActiveSkillComponent>();
    	
//    	// Update is called once per frame
//    	void Update () {
//    	
//    	}

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
                var _as = HeroSlotList[i].GetComponent<ActiveSkillComponent>();
                if(_as && _as.ActiveSkillCD)
                {
//                    ActiveSkillUI.Add(_as);
                    _as.ActiveSkillCD.gameObject.SetActive(false);
                }
                
            }

            for (int i = 0; i < TeamComponent.Instance.Team.Count; i ++)
            {
                var hero = TeamComponent.Instance.Team[i] as HeroComponent;

                // set icon
                var icon = HeroSlotList[i].GetComponent<UnityEngine.UI.Image>();
                icon.sprite = hero.Icon.sprite;

                var _as = HeroSlotList[i].GetComponent<ActiveSkillComponent>();
                if(_as)
                {
                    _as.Initialize(hero);
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

//
//        public void AdjustSkillReadyUI()
//        {
//            for (int i = 0; i < ActiveSkillUI.Count; i++)
//            {
//                ActiveSkillUI [i].AdjustSkillReadyUI();
//            }
//        }
//

    }

}