using UnityEngine;
using System.Collections;

public class CamerControl : MonoBehaviour {

	public Transform TargetObj;


//	private const float MaxRotationAngle = 120.0f;
	private const float RotationOffset = 10.0f;
	private Rect screenRect;

	// Use this for initialization
	void Start () {
		screenRect = new Rect (0, 0, Screen.width, Screen.height);
	}

	// Update is called once per frame
	void Update () {
	
		screenRect.width = Screen.width;
		screenRect.height = Screen.height;

		if (!screenRect.Contains (Input.mousePosition))
			return;

		var scrollValue = Input.GetAxis ("Mouse ScrollWheel");
		if (scrollValue != 0) 
		{
			transform.RotateAround(TargetObj.position, Vector3.right, scrollValue * RotationOffset);
		
		}


	}
}
