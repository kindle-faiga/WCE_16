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
        public float maxSpeed = 200.0f;
        public float speedRange = 10.0f;
        public float rotateRange = 20.0f;
        private Rigidbody rigitbody;
        private float offset = 5.0f;
        private float speed = 0f;

        void Start()
        {
            speed = 0f;
            rigitbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (Input.GetButton("Brake"))
            {
                speed = 0;
            }
            else if(Input.GetButton("Accel"))
            {
                if (!Input.GetButtonDown("Brake"))
                {
                    if (speed < maxSpeed)
                    {
                        speed += speedRange;
                    }
                }
            }
            else if(0 < speed)
            {
                speed -= speedRange;
            }
        }

        void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            //float v = Input.GetAxis("Vertical");

            float turn = h * rotateRange * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rigitbody.MoveRotation(rigitbody.rotation * turnRotation);

            Vector3 movement = transform.forward  * speed * speedRange * Time.deltaTime;
            rigitbody.MovePosition(rigitbody.position + movement);
        }
    }
}
