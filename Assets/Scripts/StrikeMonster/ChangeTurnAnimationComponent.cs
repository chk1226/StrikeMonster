using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

    public class ChangeTurnAnimationComponent : MonoBehaviour {


        public string ShowText
        {
            get;
            set;
        }

        public Color CharColor
        {
            get;
            set;
        }

        [Range(0.0f, 0.3f)]
        public float PerCharTime = 0.1f;
        public GameObject PrefabText;

        private List<RunTextComponent> m_CharList = new List<RunTextComponent>();

        // Use this for initialization
        void Start () {
//            CreateChar();



            StartCoroutine(PlayAnimation());
        }
        
        // Update is called once per frame
        void Update () {
        
        }

        public void Play(string str, Color color)
        {
            CharColor = color;
            ShowText = str;


            StartCoroutine(PlayAnimation());
            
        }
       

        private void CreateChar()
        {

            char[] charArray = ShowText.ToCharArray();
            char c;
            m_CharList.Clear();

            for (int i = 0; i < charArray.Length; i++)
            {
                c = charArray [i];
                
                var runTextGo = Instantiate(PrefabText) as GameObject;
                if (runTextGo)
                {
                    var runText = runTextGo.GetComponent<RunTextComponent>();
                    if (runText)
                    {
                        runText.transform.SetParent(this.transform, false);
                        runText.transform.localScale = Vector3.one;
                        runText.Char = c;
                        runText.CharColor = CharColor;

                        m_CharList.Add(runText);

                    } else
                    {
                        Destroy(runText);
                        Debug.LogWarning("No has RunTextComponent");
                        
                    }
                    
                }
            }



        }


        private IEnumerator PlayAnimation()
        {
            CreateChar();

            yield return null;
            yield return null;
            

            Vector3 offsetPos = Vector3.one;
            float totalHalf = 0;

            if(m_CharList.Count > 0)
            {
                offsetPos = m_CharList[0].transform.localPosition;
            }

            for(int i = 0; i < m_CharList.Count; i++)
            {
                float halfW = m_CharList[i].GetComponent<RectTransform>().sizeDelta.x / 2;

                offsetPos.x += halfW;
                m_CharList[i].transform.localPosition = offsetPos;
                offsetPos.x += halfW;

                totalHalf += halfW;

//                m_CharList[i].PlayTween();
//                yield return new WaitForSeconds(PerCharTime);
            }

            for(int i = 0; i < m_CharList.Count; i++)
            {
                m_CharList[i].HalfTextWidth = totalHalf;
                m_CharList[i].PlayTween();
                yield return new WaitForSeconds(PerCharTime);
            }


        }




    }


}