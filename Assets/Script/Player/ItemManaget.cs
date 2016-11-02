using UnityEngine;
using System.Collections;

namespace WCE_16
{
	public class ItemManaget : MonoBehaviour 
	{
		private float itemState = 0;
		private float maxItemState = 50;

		public float GetItemState()
		{
			return itemState;
		}

		public float GetMaxItemState()
		{
			return maxItemState;
		}

		void OnTriggerEnter(Collider collider)
		{
			if(collider.transform.tag == "Item")
			{
				Debug.Log("Get!");

				if(itemState < maxItemState)
				{
					++itemState;
				}

				Destroy(collider.gameObject);
			}
		}

		void Start () 
		{
			
		}
	}
}