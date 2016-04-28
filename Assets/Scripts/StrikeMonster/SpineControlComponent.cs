using UnityEngine;
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
        private readonly int MASK_FADEIN = Animator.StringToHash("MaskFadeIn");

        public delegate void SpineEvent();
        public event SpineEvent PlayComplete;
        public Animator MaskAnimator;

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
                m_MeshRender.sortingLayerName = GamePlaySettings.SortingLayer.FrontUILayer.ToString();
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


                // mask animator
                if(MaskAnimator)
                {
                    MaskAnimator.Play(MASK_FADEIN, -1, 0f);
                    MaskAnimator.SetBool("switch", false);
                }

                PlayComplete = completeEvent;
                m_skeletonAnimation.state.Complete += delegate(Spine.AnimationState state, int trackIndex, int loopCount) {
                    
                    if(PlayComplete != null)
                    {
                        PlayComplete();
                    }

                    MaskAnimator.SetBool("switch", true);
                    m_MeshRender.enabled = false;
                };

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