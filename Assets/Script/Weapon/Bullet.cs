using UnityEngine;
using System.Collections;

namespace WCE_16
{
    public class Bullet : MonoBehaviour
    {
        private float speed = 100.0f;

        void FixedUpdate()
        {
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);
            Vector3 t = transform.forward;
            t.y = 0;
            GetComponent<Rigidbody>().velocity = t * speed;
        }
    }
}
