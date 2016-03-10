using UnityEngine;
using System.Collections;

public class CollisionOnPlane : MonoBehaviour {

	public float AmpCoefficient = 0.5f;

	private Material rippleWaterMaterial;

	// Use this for initialization
	void Start () {
		rippleWaterMaterial = this.GetComponent<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
	
		float amp = rippleWaterMaterial.GetFloat ("_Amplitude");
		if (amp < 0.001f) {
			rippleWaterMaterial.SetFloat ("_Amplitude", 0);
		} else 
		{
			rippleWaterMaterial.SetFloat ("_Amplitude", amp * 0.98f);
		}


	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.rigidbody != null) {

			var contact = collision.contacts[0];
			Vector2 offset = new Vector2(contact.point.x - transform.position.x, contact.point.z - transform.position.z);
			offset *= -1;
			rippleWaterMaterial.SetFloat("_ContactX", offset.x);
			rippleWaterMaterial.SetFloat("_ContactZ", offset.y);
			Debug.Log("_ContactX : " + offset.x.ToString() + ",_ContactZ : " + offset.y.ToString() );


			rippleWaterMaterial.SetFloat("_Amplitude", collision.relativeVelocity.magnitude * AmpCoefficient);


		}
	}
	

}
