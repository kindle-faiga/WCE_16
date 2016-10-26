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
        private Transform cameraAngle;
        private float cameraRange = 15.0f;
        private float cameraRate = 1.0f;
        private float offset = 5.0f;
        private float speed = 0f;

        void Start()
        {
            speed = 0f;
            rigitbody = GetComponent<Rigidbody>();
            cameraAngle = gameObject.transform.FindChild("Main Camera").transform;
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

            float angle_c = cameraAngle.eulerAngles.z;

            if(0 < h)
            {
                if(360 - (cameraRange + cameraRate) < angle_c || angle_c < cameraRange)
                {
                    cameraAngle.Rotate(new Vector3(0,0,1), cameraRate);
                }
            }
            else if(h < 0)
            { 
                if(360 - cameraRange < angle_c || angle_c < cameraRange + cameraRate)
                {
                    cameraAngle.Rotate(new Vector3(0,0,1), -cameraRate);
                }
            }
            else
            {
               //iTween.RotateTo(cameraAngle.gameObject, iTween.Hash("z", 0,"time", 2.0f));
            }
        }
    }
}
