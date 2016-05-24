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
//                        break;
                    case 1:
                        return RightWall;
//                        break;
                    case 2:
                        return TopWall;
//                        break;
                    case 3:
                        return BottomWall;
//                        break;

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

//        public void EnableDamageWall(int turn)
//        {
//
//        }



    }

}