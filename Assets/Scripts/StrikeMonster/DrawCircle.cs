using UnityEngine;
using System.Collections;

namespace StrikeMonster
{
    [ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
    public class DrawCircle : MonoBehaviour {
        public float Radius;
        private LineRenderer Line;
        public int PositionCount;
        public bool Reset = false;
        public bool IsKinematic = false;
//        public Color CircleColor = Color.white;

        private int m_PositionCount;

//        public float Alpha
//        {
//            get
//            { 
//                return CircleColor.a;
//            }
//            set
//            {
//                CircleColor.a = value;
//                Line.SetColors(CircleColor, CircleColor);
//            }
//        }

        public bool EnableCircle
        {
            get
            {
                return Line.enabled;
            }
            set
            {
                Line.enabled = value;
            }
        }


    	// Use this for initialization
    	void Start () {
            Line = this.GetComponent<LineRenderer>();
//            Line.SetColors(CircleColor, CircleColor);

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
//
            if(!IsKinematic)
            {
                SetCirclePosition();
            }
            
    	}


        private void SetPositionCount()
        {
            m_PositionCount = PositionCount;
            Line.SetVertexCount(PositionCount + 2);
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
            for (int i = 0; i < m_PositionCount + 2; i++)
            {
                pos.x = Mathf.Cos( perDeg * i * Mathf.Deg2Rad) * Radius + nowPos.x;
                pos.y = Mathf.Sin( perDeg * i * Mathf.Deg2Rad) * Radius + nowPos.y;

                Line.SetPosition(i, pos);

            }
//
//            pos.x = Radius + nowPos.x;
//            pos.y = nowPos.y;
//            Line.SetPosition(m_PositionCount, pos);
        }




    }


}