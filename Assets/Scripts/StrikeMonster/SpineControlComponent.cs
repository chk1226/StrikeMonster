﻿using UnityEngine;
using System.Collections;
using Spine.Unity;


namespace StrikeMonster
{

    public class SpineControlComponent : MonoBehaviour {

        private SkeletonAnimation m_skeletonAnimation;
        private MeshRenderer m_MeshRender;
        private static SpineControlComponent _instance;
        public static SpineControlComponent Instance {
            get
            {
                return _instance;
            }
        }

        private const string SPINE_PATH = "Spine/";


        public delegate void SpineEvent();
        public event SpineEvent PlayComplete;

        void Awake()
        {
            _instance = this;
        }

    	// Use this for initialization
    	void Start () {
            m_skeletonAnimation = this.gameObject.GetComponent<SkeletonAnimation>();

            if(m_skeletonAnimation)
            {
                m_MeshRender = m_skeletonAnimation.GetComponent<MeshRenderer>();
                m_MeshRender.sortingLayerName = GamePlaySettings.SortingLayer.UILayer.ToString();


            }


    	}
    	
    	// Update is called once per frame
    	void Update () {
    	   
    	}


        public bool LoadSpineData(string dataName, SpineEvent completeEvent = null)
        {
            var data = Instantiate(Resources.Load<SkeletonDataAsset>(SPINE_PATH + dataName));
            if (data)
            {
                m_MeshRender.enabled = true;

                m_skeletonAnimation.skeletonDataAsset = data;
                m_skeletonAnimation.Initialize(true);
                m_skeletonAnimation.AnimationName = "atk";
                m_skeletonAnimation.loop = false;

                m_skeletonAnimation.state.Complete += delegate(Spine.AnimationState state, int trackIndex, int loopCount) {
                    
                    if(PlayComplete != null)
                    {
                        PlayComplete();
                    }
                    
                    Debug.Log("-----------");
                    
                    m_MeshRender.enabled = false;
                };

                PlayComplete = completeEvent;


            } else
            {
                return false;
            }

            return true;
        }

//        public void Unload()
//        {
//            m_skeletonAnimation.skeletonDataAsset = null;
//
//        }
    }



}