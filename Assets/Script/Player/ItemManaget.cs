using UnityEngine;
using System.Collections;

namespace WCE_16
{
	public class ItemManaget : MonoBehaviour 
	{
		[SerializeField]
        private GameObject ItemB;
		[SerializeField]
        private GameObject ItemA;
		[SerializeField]
        private GameObject ItemX;
		[SerializeField]
        private GameObject GetEffect;
        private Transform shotPos;
		private Transform circlePos;
		private ParticleSystem spark;
		private float itemState = 0;
		private float maxItemState = 50;
		private bool isModeY = false;
		private bool isModeX = false;
		private float shotRate = 0.25f;
        private float nextShot = 0.0f;
		private PlayerManager playerManager;

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
					SetEffect();
				}

				Destroy(collider.gameObject);
			}
		}

		void Start () 
		{
			playerManager = GetComponent<PlayerManager>();
			shotPos = transform.FindChild("ShotPos").transform;
			circlePos = transform.FindChild("CirclePos").transform;
			spark = transform.FindChild("Spark").GetComponent<ParticleSystem>();
			spark.Stop();
		}

		void Update()
        {
			if(playerManager.GetActive())
            {
				if(Input.GetButtonDown("UseItemB"))
				{
					if(!isModeY)
					{
						UseItemB();
					}
				}
				
				if(Input.GetButtonDown("UseItemA"))
				{
					UseItemA();
				}

				if(Input.GetButtonDown("UseItemX"))
				{
					if(!isModeX)
					{
						isModeX = true;
						UseItemX();
					}
				}

				if(Input.GetButtonDown("UseItemY"))
				{
					if(!isModeY)
					{
						isModeY = true;

						if(!spark.IsAlive())
						{
							spark.Play();
						}
					}
				}

				if(isModeY)
				{
					if(nextShot < Time.time)
					{
						nextShot = Time.time + shotRate;

						UseItemB();
					}
				}
			}
		}
        
		private void UseItemB()
		{
			Instantiate(ItemB, shotPos.position, transform.rotation);
		}

		private void UseItemA()
		{
			GameObject trap =  Instantiate(ItemA, circlePos.position, transform.rotation) as GameObject;

			trap.transform.parent = transform;

			StartCoroutine(WaitForTrap(trap));
		}

		private void UseItemX()
		{
			GameObject circle =  Instantiate(ItemX, shotPos.position, transform.rotation) as GameObject;

			circle.transform.parent = transform;			
		}

		private void SetEffect()
		{
			GameObject effect =  Instantiate(GetEffect, circlePos.position, transform.rotation) as GameObject;

			effect.transform.parent = transform;

			Destroy(effect, 0.5f);
		}

		IEnumerator WaitForTrap(GameObject trap)
        {
            yield return new WaitForSeconds(0.5f);
        	trap.transform.parent = null;
        }
	}
}