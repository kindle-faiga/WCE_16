using UnityEngine;
using System.Collections;

namespace WCE_16
{
    public enum WEAPON_NAME
    {
        Bullet,
        Bomb,
        Special,
        End,
    };

    public class PlayerManager : MonoBehaviour
    {
        private float offset = 5.0f;
        private float speed = 0f;
        private float maxSpeed = 50.0f;
        private float speedRange = 0.5f;

        void Start()
        {
            speed = 0f;
        }

        void Update()
        {
            if(Input.GetButton("Accel"))
            {
                if(speed < maxSpeed)
                {
                    speed += speedRange;
                }
            }
            else if(0 < speed)
            {
                speed -= speedRange;
            }

            if (Input.GetButtonDown("Brake"))
            {
                speed = 0;
            }
        }

        void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);
            //GetComponent<Rigidbody>().AddForce(h * speed, 0, speed * offset);
        }
    }
}
