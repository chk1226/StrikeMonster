using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    public class UnitComponent : MonoBehaviour {

        public UnityEngine.UI.Image Icon;
        public UnityEngine.UI.Text HurtValue;
        public GameObject SkillGroup;
        
        private float totalHurtValue = 0;

        protected float m_HP;
        public float HP
        {
            get{
                return m_HP;
            }
        }

        protected float m_Attack;
        public float Attack
        {
            get{
                return m_Attack;
            }
        }


        protected const string HEAD_ICON_PATH = "Sprites/";
        protected const string SKILL_PATH = "Prefabs/Skill/";

        public virtual void Initialize(UnitInfo baseInfo)
        {
            m_HP = baseInfo.Hp;
            m_Attack = baseInfo.Attack;
            
            this.transform.localPosition = new Vector3(baseInfo.Position.x, baseInfo.Position.y, 0);
            
            var img = Resources.Load<Sprite>(HEAD_ICON_PATH + baseInfo.ImageSource);
            if (img)
            {
                Icon.sprite = img;
            }
            
        }


        private float intervalHurtTextTime = 0;
        void Update()
        {
            if (HurtValue && HurtValue.enabled)
            {
                if(HurtValue.GetComponent<iTween>())
                {

                    intervalHurtTextTime = 0;
                }
                else
                {
                    intervalHurtTextTime += GamePlaySettings.Instance.GetDeltaTime();
                    if(intervalHurtTextTime >= 1.5f)
                    {
                        HurtValue.enabled = false;
                        totalHurtValue = 0;
                    }


                }
            }
        }

        void FixedUpdate()
        {
            HandlePhysics();
        }

        private void HurtTextUpdate( float value )
        {
            HurtValue.text = value.ToString("0");

        }

        private void HurtTextStart()
        {
            if(HurtValue && HurtValue.enabled == false)
            {
                HurtValue.enabled = true;
            }
        }

        public virtual void OnHurt(float damage)
        {
            if(HurtValue)
            {

                iTween.ValueTo( HurtValue.gameObject, iTween.Hash(
                    "from", totalHurtValue,
                    "to", totalHurtValue + damage,
                    "time", 0.5f,
                    "onstarttarget", this.gameObject,
                    "onstart", "HurtTextStart",
                    "onupdatetarget", this.gameObject,
                    "onupdate", "HurtTextUpdate",
                    "easetype", iTween.EaseType.easeOutQuad));

                totalHurtValue += damage;
//                Debug.Log(totalHurtValue);

            }

        }

        public virtual void HandlePhysics()
        {
        }



    }

}