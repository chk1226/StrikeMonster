using UnityEngine;
using System.Collections;

public class RaycastMouseInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1")) 
		{

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var hit = Physics.RaycastAll(ray);

			if(hit != null)
			{

				for(int i=0; i< hit.Length; i++)
				{
					Debug.Log("hit!!! "+ i.ToString()+ " " + hit[i].transform.name);
					Debug.Log(hit[i].textureCoord);
				}

			}

		}
	}
}
