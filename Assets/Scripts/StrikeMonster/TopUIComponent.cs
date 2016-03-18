using UnityEngine;
using System.Collections;

namespace StrikeMonster
{

    public class TopUIComponent : MonoBehaviour {

        public GameObject BossHp;


    	// Use this for initialization
    	void Start () {
    	
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}






        public void EnableBossHp()
        {

            var boss = WaveComponent.Instance.Boss;


            if (boss)
            {
                BossHp.SetActive(true);
                var bar = BossHp.GetComponentInChildren<IndicatorBarComponent>();

                var hpProperty = boss.GetComponent<HpPropertyComponent>();
                if (hpProperty)
                {
                    hpProperty.IndicatorComponent = bar;
                    bar.UpdateBar(hpProperty.Ratio);
                }

            }
           
        }


        public void DisableBossHp()
        {
            BossHp.SetActive(false);
        }



    }


}