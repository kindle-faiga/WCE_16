using UnityEngine;
using System.Collections;

namespace WCE_16
{
    public class Bullet : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag != "Machine")
            {
                Destroy(gameObject);
            }
        }
    }
}
