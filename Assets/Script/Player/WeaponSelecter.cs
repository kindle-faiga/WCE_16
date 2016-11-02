using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace WCE_16
{
    public class WeaponSelecter : MonoBehaviour
    {
        [SerializeField]
        private GameObject Bullet;
        private Transform shotPos;
        private float shotRange = 5.0f;
        private float shotState;
        private float maxShotState = 100.0f;
        private float shotRate = 0.25f;
        private float nextShot = 0.0f;

        public float GetShotState()
        {
            return shotState;
        }

        public float GetMaxShotState()
        {
            return maxShotState;
        }

        void Start()
        {
            shotPos = transform.FindChild("ShotPos").transform;
            shotState = maxShotState;
        }

        void Update()
        {
            if(Input.GetButton("UseBullet"))
            {
                if(nextShot < Time.time && shotRange < shotState)
                {
                    nextShot = Time.time + shotRate;

                    shotState -= shotRange;

                    UseBullet();
                }
            }
            else
            {
                if(shotState < maxShotState)
                {
                    ++shotState;
                }
            }
        }

        private void UseBullet()
        {
            Instantiate(Bullet, shotPos.position, transform.rotation);
        }
    }
}
