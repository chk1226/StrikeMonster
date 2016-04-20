using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    public class CDPropertyComponent : IndicatorPropertyComponent {

        protected override string TextOutput()
        {
            return string.Format("{0}", Mathf.CeilToInt(m_CurrentValue));
        }

        public void SetCDTextLocalPosition(Vector2 locPos)
        {
            if (m_TextIndicatorComponent)
            {
                m_TextIndicatorComponent.transform.localPosition = new Vector3(locPos.x, locPos.y, m_TextIndicatorComponent.transform.localPosition.z);
            }
        }


        public void ReduceCD()
        {
            Value -= 1;

        }

        public void RecoveryCD()
        {
            Value = m_MaxValue;
        }

        public void EnableCDText(bool b)
        {
            if (m_TextIndicatorComponent)
            {
                m_TextIndicatorComponent.gameObject.SetActive(b);
            }
        }

    }
}