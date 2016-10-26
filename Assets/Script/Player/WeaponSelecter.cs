using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace WCE_16
{
    public class WeaponSelecter : MonoBehaviour
    {
        public GameObject Bullet;
        public GameObject Bomb;
        private Transform shotPos;

        void Start()
        {
            shotPos = GameObject.Find("Player/ShotPos").transform;
        }

        void Update()
        {
            if(Input.GetButtonDown("UseBullet"))
            {
                UseBullet();
            }
            else if(Input.GetButtonDown("UseBomb"))
            {
                UseBomb();
            }
        }

        private void UseBullet()
        {
            Instantiate(Bullet, shotPos.position, transform.rotation);
        }

        private void UseBomb()
        {
            Instantiate(Bomb, shotPos.position, transform.rotation);
        }
    }
}
