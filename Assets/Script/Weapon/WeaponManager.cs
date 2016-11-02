using UnityEngine;
using System.Collections;

namespace WCE_16
{
	public class WeaponManager : MonoBehaviour 
	{
		[SerializeField]
		private float speed = 300.0f;
		[SerializeField]
		private float eraseTime = 2.0f;

		void Start ()
		{
			StartCoroutine(WaitForDestroy());
		}

		void FixedUpdate()
		{
			Vector3 t = transform.forward;
			t.y = 0;
			GetComponent<Rigidbody>().velocity = t * speed;
		}

		IEnumerator WaitForDestroy()
        {
            yield return new WaitForSeconds(eraseTime);
            Destroy(gameObject);
        }
	}
}