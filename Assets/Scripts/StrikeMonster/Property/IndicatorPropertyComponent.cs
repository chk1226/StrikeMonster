
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

	public class IndicatorPropertyComponent : MonoBehaviour
	{
		[SerializeField]
		protected float m_CurrentValue = 0.0f;

		[SerializeField]
		protected float m_MinValue = 0.0f;

		[SerializeField]
		protected float m_MaxValue = 100.0f;

		[SerializeField]
        protected IndicatorBarComponent m_IndicatorComponent = null;

		[SerializeField]
		private IndicatorBarComponent m_ShadowIndicatorComponent = null;

		[SerializeField]
        protected UnityEngine.UI.Text m_TextIndicatorComponent = null;

		[SerializeField]
		private bool m_IsAnimate = true;

		[SerializeField]
		private bool m_IsRenderInvert = false;


        public IndicatorBarComponent IndicatorComponent
        {
            set
            {
                this.m_IndicatorComponent = value;
            }
        }
        
        public UnityEngine.UI.Text TextIndicatorComponent
        {
            set
            {
                this.m_TextIndicatorComponent = value;
            }
        }

		//--------------------------------------------------------------------------------
		public void Initialize(float currentValue, float minValue, float maxValue)
		{
			m_CurrentValue = currentValue;
			m_MinValue = minValue;
			m_MaxValue = maxValue;
			UpdateIndicator();
		}

		//--------------------------------------------------------------------------------
		public float Value
		{
			get
			{
				return ProcessGetValue();
			}
			set
			{
				ProcessSetValue(value);
			}
		}

		//--------------------------------------------------------------------------------
		protected virtual float ProcessGetValue()
		{
			return m_CurrentValue;
		}

		//--------------------------------------------------------------------------------
		protected virtual void ProcessSetValue(float newValue)
		{
			float previous_value = m_CurrentValue;
			m_CurrentValue = Mathf.Clamp(newValue, m_MinValue, m_MaxValue);
			OnValueChanged(newValue - previous_value);
			UpdateIndicator();
		}

		//--------------------------------------------------------------------------------
		public float Ratio
		{
			get
			{
				return (m_CurrentValue - m_MinValue) / (m_MaxValue - m_MinValue);
			}
		}

		//--------------------------------------------------------------------------------
		public float Max
		{
			get
			{
				return m_MaxValue;
			}
		}

		//--------------------------------------------------------------------------------
		public float Min
		{
			get
			{
				return m_MinValue;
			}
		}

        protected virtual string TextOutput()
        {
            return string.Format("{0} / {1}", Mathf.CeilToInt(m_CurrentValue), Mathf.CeilToInt(m_MaxValue));
        }

		//--------------------------------------------------------------------------------
		protected void UpdateIndicator()
		{
			if (null != m_IndicatorComponent)
			{
				float ratio = Mathf.Clamp(Ratio, 0.0001f, m_MaxValue);

				if (m_IsRenderInvert)
				{
					ratio = 1.0f - ratio;
				}

				if (m_IsAnimate)
				{
					m_IndicatorComponent.UpdateBar(ratio);
				}
				else
				{
					m_IndicatorComponent.SetValue(ratio);
				}
				if (m_ShadowIndicatorComponent != null)
				{
					m_ShadowIndicatorComponent.UpdateBar(Ratio);
				}
			}

			if (m_TextIndicatorComponent != null)
			{
                m_TextIndicatorComponent.text = TextOutput();
			}
		}

		//--------------------------------------------------------------------------------
		protected virtual void OnValueChanged(float amount)
		{
		}
	}

}