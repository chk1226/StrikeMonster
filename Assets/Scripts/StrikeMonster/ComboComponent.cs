using UnityEngine;
using System.Collections;

namespace StrikeMonster
{

    public class ComboComponent : MonoBehaviour {

        private static ComboComponent _instance;
        public static ComboComponent Instance
        {
            get
            {
                return _instance;
            }
        }

        public UnityEngine.UI.Text Hits;
        public UnityEngine.UI.Text ComboValue;


        private int _Value = 0;

        void Awake()
        {
            _instance = this;
        }

        // Use this for initialization
        void Start () {
            
        }
        
        // Update is called once per frame
        void Update () {
            
        }


        public void EnableCombo(bool b)
        {
            if (Hits && Hits.IsActive() != b)
            {
                Hits.gameObject.SetActive(b);
            }

            if (ComboValue && ComboValue.IsActive() != b)
            {
                ComboValue.gameObject.SetActive(b);
            }

        }

        public void Increment(int amount)
        {
            if(amount <= 0)
            {
                return;
            }

            _Value += amount;
            ComboValue.text = _Value.ToString();

        }


        public void Rest()
        {
            _Value = 0;
            ComboValue.text = _Value.ToString();
        }



    }


}