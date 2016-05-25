using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{
    public class WallComponent : MonoBehaviour {

        public GameObject LeftWall;
        public GameObject RightWall;
        public GameObject TopWall;
        public GameObject BottomWall;

        private System.Random rnd = new System.Random();
        public GameObject RandomWall
        {
            get
            {
                var index = rnd.Next() % 4;

                switch(index)
                {
                    case 0:
                        return LeftWall;
                    case 1:
                        return RightWall;
                    case 2:
                        return TopWall;
                    case 3:
                        return BottomWall;
                    default:
                        return LeftWall;
                }

            }
        }

        private static WallComponent m_Wall;
        public static WallComponent Instance
        {
            get{
                return m_Wall;
            }
        }

        void Awake()
        {
            m_Wall = this;
        }

    	// Use this for initialization
    	void Start () {
    	
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	}

        public void DamageWallReduceTurn()
        {
            LeftWall.BroadcastMessage(InterfaceMehodName.ReduceCD, SendMessageOptions.DontRequireReceiver);
            RightWall.BroadcastMessage(InterfaceMehodName.ReduceCD, SendMessageOptions.DontRequireReceiver);
            TopWall.BroadcastMessage(InterfaceMehodName.ReduceCD, SendMessageOptions.DontRequireReceiver);
            BottomWall.BroadcastMessage(InterfaceMehodName.ReduceCD, SendMessageOptions.DontRequireReceiver);
            
        }


        public List<GameObject> GetAllWall()
        {
            var all = new List<GameObject>();

            all.Add(LeftWall);
            all.Add(RightWall);
            all.Add(TopWall);
            all.Add(BottomWall);
            return all;
        }



        public Vector3 GetNormal(GameObject wall)
        {

            if (wall == LeftWall)
            {
                return Vector3.right;
            } else if (wall == RightWall)
            {
                return Vector3.left;
            } else if (wall == TopWall)
            {
                return Vector3.down;
            }
            else if(wall == BottomWall)
            {
                return Vector3.up;
            }

            Debug.LogWarning("GetNormal is not found");
            return Vector3.up;

        }
            


    }

}