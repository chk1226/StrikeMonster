using UnityEngine;
using System.Collections;

namespace StrikeMonster
{

    public class TopUIComponent : MonoBehaviour {

        public GameObject BossHp;


        public void EnableBossHp(bool value)
        {
            if (value)
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

            } else
            {
                BossHp.SetActive(false);
            }

           
        }
       


    }


}