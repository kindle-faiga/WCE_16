using UnityEngine;
using System.Collections;

namespace WCE_16
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private float maxSpeed = 200.0f;
        [SerializeField]
        private float speedRange = 10.0f;
        [SerializeField]
        private float rotateRange = 45.0f;
        private float defaultRange;
        private float turnRange;
        private Rigidbody rigitbody;
        private Transform cameraAngle;
        private ParticleSystem smoke;
        private ParticleSystem brake;
        private float cameraRange = 15.0f;
        private float cameraRate = 1.0f;
        private float offset = 5.0f;
        private float speed = 0f;
        private bool isActive = false;

        void Start()
        {
            speed = 0f;
            defaultRange = rotateRange;
            turnRange = rotateRange * 2f;
            rigitbody = GetComponent<Rigidbody>();
            cameraAngle = gameObject.transform.FindChild("Machine Camera").transform;
            smoke = transform.FindChild("Smoke").GetComponent<ParticleSystem>();
            brake = transform.FindChild("Brake").GetComponent<ParticleSystem>();
            smoke.Stop();
            brake.Stop();
        }

        public bool GetActive()
        {
            return isActive;
        }

        public void SetActive()
        {
            isActive = true;
        }

        public void ResetActive()
        {
            isActive = false;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public float GetMaxSpeed()
        {
            return maxSpeed;
        }

        void Update()
        {
            if(isActive)
            {
                if (Input.GetButton("Brake"))
                {
                    speed = 0;
                    rotateRange = turnRange;
                }
                else if(Input.GetButton("Accel"))
                {
                    if(!smoke.IsAlive())
                    {
                        smoke.Play();
                    }

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

                if(Input.GetButtonUp("Brake"))
                {
                    rotateRange = defaultRange;
                    brake.Stop();
                }
                    
                else if(Input.GetButtonDown("Brake"))
                {
                    smoke.Stop();
                    brake.Play();
                }  
            }
        }

        void FixedUpdate()
        {
            if(isActive)
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
}
