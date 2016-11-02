using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace WCE_16
{
	public class FillAmountManager : MonoBehaviour 
	{
		[SerializeField]
		private Image speedImage;
		[SerializeField]
		private Image weaponImage;
		[SerializeField]
		private Image itemImage; 
		private PlayerManager playerManager;
		private WeaponSelecter weaponSelecter;
		private ItemManaget itemManager;
		private float maxSpeed;
		private float maxWeapon;
		private float maxItem;

		void Start () 
		{	
			playerManager = GetComponent<PlayerManager>();
			weaponSelecter = GetComponent<WeaponSelecter>();
			itemManager = GetComponent<ItemManaget>();

			maxSpeed = playerManager.GetMaxSpeed();
			maxWeapon = weaponSelecter.GetMaxShotState();
			maxItem = itemManager.GetMaxItemState();
		}
		
		private void UpdateState(float state, float maxState, Image image)
		{
			float range = state/maxState;

			Debug.Log(range);

			image.fillAmount = range;
		}

		void Update () 
		{
			float speed = playerManager.GetSpeed();

			float weapon = weaponSelecter.GetShotState();

			float item = itemManager.GetItemState();

			UpdateState(speed, maxSpeed, speedImage);

			UpdateState(weapon, maxWeapon, weaponImage);

			UpdateState(item, maxItem, itemImage);
		}
	}
}
