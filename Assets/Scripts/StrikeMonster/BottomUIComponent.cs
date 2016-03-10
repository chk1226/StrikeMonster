using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

    public class BottomUIComponent : MonoBehaviour {

        public List<GameObject> HeroSlotList;



    	// Use this for initialization
    	void Start () {
    	
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}

        public void Initialize()
        {
            InitializeHeroSlot();
        }
        
        public void InitializeHeroSlot()
        {
            if (HeroSlotList == null)
            {
                return;
            }


            if(TeamComponent.Instance.Team.Count > HeroSlotList.Count)
            {
                return;
            }

            for (int i = 0; i < TeamComponent.Instance.Team.Count; i ++)
            {
                var hero = TeamComponent.Instance.Team[i] as HeroComponent;
                var icon = HeroSlotList[i].GetComponent<UnityEngine.UI.Image>();
                icon.sprite = hero.Icon.sprite;
            }

        }


    }

}