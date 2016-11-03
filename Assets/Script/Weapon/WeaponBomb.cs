using UnityEngine;
using System.Collections;

public class WeaponBomb : MonoBehaviour 
{
	[SerializeField]
	private float rotateRange = 30.0f;
	[SerializeField]
    private GameObject destroyEffect;

	void OnCollisionEnter(Collision collision)
	{
		if(collision.transform.tag != "Machine")
		{
			GameObject effect =  Instantiate(destroyEffect, transform.position, transform.rotation) as GameObject;
			Destroy(effect, 0.5f);
			Destroy(gameObject);
		}
	}

	void Start () 
	{
	
	}
	
	void FixedUpdate () 
	{
		transform.Rotate(Vector3.forward, rotateRange * Time.deltaTime);
		//transform.rotation = Quaternion.AngleAxis( rotateRange * Time.deltaTime, Vector3.forward );
	}
}
