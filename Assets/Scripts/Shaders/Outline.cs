using UnityEngine;
using System.Collections;

namespace MyShader
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class Outline : MonoBehaviour {

        private UnityEngine.UI.Image m_Image = null;
        public UnityEngine.UI.Image Image
        {
            get
            {
                if (m_Image == null)
                {
                    m_Image = GetComponent<UnityEngine.UI.Image>();
                }
                return m_Image;
            }
        }

        private static Material m_Material = null;
        protected Material material
        {
            get
            {
                if (m_Material == null)
                {
                    Shader shader = null;
                    if ((shader = Shader.Find("Custom/Outline")) != null)
                    {
                        m_Material = new Material(shader);
                        m_Material.hideFlags = HideFlags.None;
                    }
                }
                return m_Material;
            }
        }
        
        public bool AlwaysUpdate = false;
        
        
        
        
        // Use this for initialization
        void Start () {
            Image.type = UnityEngine.UI.Image.Type.Simple;
            if (material == null || !material.shader.isSupported)
            {
                gameObject.SetActive(false);
                return;
            }
            else
            {
                Image.material = material;
            }
        }
        
        // Update is called once per frame
        void Update () {
            if(AlwaysUpdate)
            {
                Render();
            }
        }


        public void Render()
        {

            material.SetTexture("_MainTex", Image.sprite.texture);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }
            
            
            
            
    }





}


