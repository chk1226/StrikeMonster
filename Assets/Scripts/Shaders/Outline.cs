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

        private Material m_Material = null;
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
                        m_Material.hideFlags = HideFlags.DontSave;
                    }
                }
                return m_Material;
            }
        }
        
//        public bool AlwaysUpdate = true;
        [Range(0.001f, 0.0035f)]
        public float TexOffset = 0.0025f;

        public bool _enable = false;
        public bool Enable
        {
            set{
                _enable = value;
                if (_enable)
                {
                    Image.type = UnityEngine.UI.Image.Type.Simple;
                    if (material == null || !material.shader.isSupported)
                    {
                        gameObject.SetActive(false);
                        return;
                    } else
                    {
                        Image.material = material;
                        Render();
                    }
                } else
                {
                    Image.material = null;
                    Image.color = Color.white;
                }   
            }

            get{ return _enable; }
        }

       // Use this for initialization
        void Start () {
            Enable = true;
        }
        
        // Update is called once per frame
        void Update () {
            if(Enable)
            {
                Render();
            }
        }


        public void Render()
        {
            material.SetColor("_Color", Image.color);
            material.SetFloat("_TexOffset", TexOffset);
            material.SetTexture("_MainTex", Image.sprite.texture);


#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }
            

            
    }





}


