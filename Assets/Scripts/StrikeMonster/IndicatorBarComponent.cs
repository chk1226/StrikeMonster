using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
//		[AddComponentMenu("Inverse/GamePlay/IndicatorBarComponent")]
	public class IndicatorBarComponent : MonoBehaviour
	{
		public float MaxAmount = 1.0f;
		public float MinAmount = 0;
		public float Scale = 1.0f;
		public float Time = 0.25f;
		public float DefaultValue = 0.0f;
		protected UnityEngine.UI.Image m_Image = null;

		protected bool m_TweenMode = false;
		protected float m_DeltaTime = 0;
		protected float m_FromValue = 0;
		protected float m_ToValue = 0;
		protected bool m_UpdateBarFinished = true;

		//--------------------------------------------------------------------------------
		void Start()
		{
			m_Image = gameObject.GetComponent<UnityEngine.UI.Image>();
			if (m_Image != null)
			{
				m_Image.fillAmount = Mathf.Clamp(DefaultValue, MinAmount, MaxAmount) * Scale;
			}
		}

		//--------------------------------------------------------------------------------
		public void UpdateBar(float value)
		{
			if (m_Image != null)
			{
				m_TweenMode = true;

				if (m_UpdateBarFinished)
				{
					m_FromValue = m_Image.fillAmount;
				}
				else
				{
					m_Image.fillAmount = UpdateFillAmount();
					m_FromValue = m_Image.fillAmount;
				}
				m_DeltaTime = 0;
				m_ToValue = Mathf.Clamp(value, MinAmount, MaxAmount) * Scale;
				m_Image.SetAllDirty();
			}
			else
			{
				if (gameObject.activeInHierarchy)
				{
					StartCoroutine(WaitForInitialized(value));
				}
			}
		}

		//--------------------------------------------------------------------------------
		public IEnumerator WaitForInitialized(float value)
		{
			yield return new WaitForEndOfFrame();

			if (m_Image != null)
			{
				UpdateBar(value);
			}
		}

		//--------------------------------------------------------------------------------
		public void SetValue(float value)
		{
			if (m_Image != null)
			{
				m_UpdateBarFinished = false;
				m_Image.fillAmount = Mathf.Clamp(value, MinAmount, MaxAmount) * Scale;
				m_Image.SetAllDirty();
			}
		}

		//--------------------------------------------------------------------------------
		void Update()
		{
			if (m_TweenMode && m_Image != null)
			{
				m_Image.fillAmount = UpdateFillAmount();
				m_DeltaTime += GamePlaySettings.Instance.GetDeltaTime();
			}
		}

		//--------------------------------------------------------------------------------
		float UpdateFillAmount()
		{
			float value = m_Image.fillAmount;
			if (m_DeltaTime < Time)
			{
				m_UpdateBarFinished = false;
				value = m_FromValue + (m_ToValue - m_FromValue) * m_DeltaTime / Time;
			}
			else if (m_DeltaTime >= Time)
			{
				value = m_ToValue;
				m_UpdateBarFinished = true;
			}

			return value;
		}
	} // class IndicatorBarComponent
}
