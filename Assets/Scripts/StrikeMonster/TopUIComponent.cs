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



        public void EnableBossHp(IndicatorPropertyComponent ipc)
        {
            BossHp.SetActive(true);
            var bar = BossHp.GetComponentInChildren<IndicatorBarComponent>();

            if (ipc)
            {
                ipc.IndicatorComponent = bar;
            }

        }


        public void DisableBossHp()
        {
            BossHp.SetActive(false);
        }



    }


}