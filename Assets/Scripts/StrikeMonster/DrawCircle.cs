using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    [ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
    public class DrawCircle : MonoBehaviour {
        public float Radius;
        public LineRenderer Line;
        public int PositionCount;
        public bool Reset = false;
        public bool IsKinematic = false;

        private int m_PositionCount;

    	// Use this for initialization
    	void Start () {
            Line = this.GetComponent<LineRenderer>();
            SetPositionCount();
            SetCirclePosition();
    	}
    	
    	// Update is called once per frame
    	void Update () {
            if(Reset)
            {
                SetPositionCount();
                Reset = false;
            }

            if(!IsKinematic)
            {
                SetCirclePosition();
            }
            
    	}


        private void SetPositionCount()
        {
            m_PositionCount = PositionCount;
            Line.SetVertexCount(PositionCount + 1);
        }


        private void SetCirclePosition()
        {

            if(m_PositionCount == 0)
            {
                return;
            }

            Vector3 nowPos = this.transform.position;
            Vector3 pos = nowPos;
            
            float perDeg = 360 / m_PositionCount;
            for (int i = 0; i < m_PositionCount; i++)
            {
                pos.x = Mathf.Cos( perDeg * i * Mathf.Deg2Rad) * Radius + nowPos.x;
                pos.y = Mathf.Sin( perDeg * i * Mathf.Deg2Rad) * Radius + nowPos.y;

                Line.SetPosition(i, pos);

            }

            pos.x = Radius + nowPos.x;
            pos.y = nowPos.y;
            Line.SetPosition(m_PositionCount, pos);
        }


    }


}