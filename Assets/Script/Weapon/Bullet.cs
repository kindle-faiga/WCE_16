using UnityEngine;
using System.Collections;

namespace WCE_16
{
    public class Bullet : MonoBehaviour
    {
        private float speed = 200.0f;

        void Update()
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);
        }
    }
}
