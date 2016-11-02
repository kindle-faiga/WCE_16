using UnityEngine;
using System.Collections;

namespace WCE_16
{
	public class SubCameraManager : MonoBehaviour
	{
		private Transform player;

		void Start()
		{
				player = GameObject.Find("Player1").transform;
		}

		void Update()
		{

		}

		void FixedUpdate()
		{
			Vector3 newPosition = transform.position;
			newPosition.x = player.position.x;
			newPosition.y = transform.position.y;
			newPosition.z = player.position.z;
			transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
		}
	}
}
