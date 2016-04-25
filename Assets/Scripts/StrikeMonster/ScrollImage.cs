using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace StrikeMonster
{
    [RequireComponent(typeof(Graphic))]
#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
	public class ScrollImage : BaseVertexEffect{
#else
	public class ScrollImage : BaseMeshEffect {
#endif

        public float ScrollSpeed;
        public Image BackgroundImage;

        private float offset = 0;


#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
		public override void ModifyVertices(List<UIVertex> verts)
#else
		public override void ModifyMesh(VertexHelper vh)
		{
			if (IsActive() == false)
			{
				return;
			}

			var vList = new List<UIVertex>();
			vh.GetUIVertexStream(vList);

			ModifyVertices(vList);

			vh.Clear();
			vh.AddUIVertexTriangleStream(vList);
		}

		public void ModifyVertices(List<UIVertex> verts)
#endif
        {

            if (IsActive() == false || verts == null || verts.Count == 0)
            {
                return;
            }

            if (BackgroundImage)
            {
                if (BackgroundImage.sprite.texture.wrapMode != TextureWrapMode.Repeat)
                {
                    BackgroundImage.sprite.texture.wrapMode = TextureWrapMode.Repeat;
                }
            } 
            else
            {
                return;
            }
        
            offset += ScrollSpeed * Time.deltaTime;


            for (int index = 0; index < verts.Count; index ++)
            {
                var vertex = verts[index];
                vertex.uv0.y += offset;

                verts[index] = vertex;

            }

        
        }

        void Update()
        {
            if (ScrollSpeed > 0)
            {
                graphic.SetVerticesDirty();
            }
        }




    }




}
