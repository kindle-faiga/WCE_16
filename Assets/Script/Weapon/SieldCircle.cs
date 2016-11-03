using UnityEngine;
using System.Collections;

public class SieldCircle : MonoBehaviour 
{
	[SerializeField]
	private float rotateRange = 30.0f;
	private Vector3 defaultSize;
	
	void Start () 
	{
		defaultSize = transform.localScale;
		transform.localScale = Vector3.zero;
		iTween.ScaleTo(gameObject, defaultSize, 3.0f);
	}
	
	void FixedUpdate () 
	{
		transform.Rotate(Vector3.forward, rotateRange * Time.deltaTime);
	}
}
