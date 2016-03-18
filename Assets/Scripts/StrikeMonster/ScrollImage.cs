using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace StrikeMonster
{
    [RequireComponent(typeof(Graphic))]
    public class ScrollImage : BaseVertexEffect {

        public float ScrollSpeed;
        public Image BackgroundImage;

        private float offset = 0;


        public override void ModifyVertices(System.Collections.Generic.List<UIVertex> verts)
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
