using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour 
{
	[SerializeField]
	private float maxSize = 100f;
	
	void Start () 
	{
		iTween.ScaleTo(gameObject, new Vector3(maxSize,maxSize,maxSize), 20.0f);
	}
}
