using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour 
{
	[SerializeField]
	private float maxSize = 100f;
	[SerializeField]
	private GameObject sparkEffect;
	[SerializeField]
	private GameObject setEffect;
	[SerializeField]
	private GameObject cube;
	[SerializeField]
	private Transform cubePos;
	
	void Start () 
	{
		iTween.ScaleTo(gameObject, 
			iTween.Hash
			(
				"x", maxSize, 
				"y", maxSize, 
				"z", maxSize, 
				"time", 7.5f,
				"oncomplete", "SetSpark", 
				"oncompletetarget", gameObject
			));
	}

	private void SetSpark()
	{
		GameObject spark =  Instantiate(sparkEffect, transform.position, transform.rotation) as GameObject;

		StartCoroutine(WaitForSpark(spark));
	}

	private IEnumerator WaitForSpark(GameObject spark)
	{
		yield return new WaitForSeconds(10.0f);
		Instantiate(setEffect, transform.position, transform.rotation);
		Instantiate(cube, cubePos.transform.position, transform.rotation);
		Destroy(spark);
		Destroy(gameObject);
	}
}
