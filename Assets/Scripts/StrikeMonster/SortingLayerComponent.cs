using UnityEngine;

namespace StrikeMonster
{
    [ExecuteInEditMode]
    public class SortingLayerComponent : MonoBehaviour
	{
        public bool OverrideCanvas;


        [SerializeField]
        private GamePlaySettings.SortingLayer m_LayerName;

        private Canvas m_Canvas;

        public GamePlaySettings.SortingLayer LayerName
		{
			get
			{
				return m_LayerName;
			}
			set
			{
				m_LayerName = value;
				SetLayer();
			}
		}

		//--------------------------------------------------------------------------------
		void Start()
		{
			SetLayer();
		}

		//--------------------------------------------------------------------------------
		protected void SetLayer()
		{
			Renderer[] renderer_list = gameObject.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < renderer_list.Length; ++i)
			{
				renderer_list[i].sortingLayerName = LayerName.ToString();
			}
			
            if(OverrideCanvas)
            {
                if(!m_Canvas)
                {
                    m_Canvas = gameObject.GetComponent<Canvas>();

                    if(!m_Canvas)
                    {
                        m_Canvas = gameObject.AddComponent<Canvas>();
                        m_Canvas.overrideSorting = true;
                    }

                }

                if(m_Canvas)
                {
                    m_Canvas.sortingLayerName = LayerName.ToString();
                }

            }

		}


        void Update()
        {
            if(!Application.isPlaying)
            {
                if(OverrideCanvas)
                {
                    SetLayer();
                }

            }

        }


	} 

} 
