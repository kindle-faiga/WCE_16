using UnityEngine;
using System.Collections;

public class WeaponBomb : MonoBehaviour 
{
	[SerializeField]
	private float rotateRange = 30.0f;

	void Start () 
	{
	
	}
	
	void FixedUpdate () 
	{
		transform.Rotate(Vector3.forward, rotateRange * Time.deltaTime);
		//transform.rotation = Quaternion.AngleAxis( rotateRange * Time.deltaTime, Vector3.forward );
	}
}
