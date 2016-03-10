using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{
	public class HpPropertyComponent : IndicatorPropertyComponent
	{


		//--------------------------------------------------------------------------------
//		protected override void OnValueChanged(float amount)
//		{
//			if (amount < 0)
//			{
//				gameObject.SendMessage("OnHurt", -amount,
//									   SendMessageOptions.DontRequireReceiver);
//			}
//			else if (amount > 0) // got heal
//			{
//				gameObject.SendMessage("OnHeal", amount,
//									   SendMessageOptions.DontRequireReceiver);
//			}
//		}

		//--------------------------------------------------------------------------------
//		public void SetRatio(float ratio)
//		{
//			ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);
//			m_CurrentValue = ratio * m_MaxValue;
//			UpdateIndicator();
//		}

		//--------------------------------------------------------------------------------
//		public void IncreaseHPAmountWhenHPGreaterZero(float amount)
//		{
//			if (Value > 0)
//			{
//				Value += amount;
//			}
//		}


	} 
}