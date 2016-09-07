using UnityEngine;
using System.Collections;

namespace WCE_16
{
    public class Bomb : MonoBehaviour
    {
        public GameObject BombEffect;
        private float speed = 100.0f;

        void OnCollisionEnter(Collision c)
        {
            if (c.transform.tag == "Block")
            {
                if (!c.transform.GetComponent<BlockState>().IsCrash())
                {
                    c.transform.GetComponent<BlockState>().Hit(BombEffect);
                    Destroy(gameObject);
                }
            }
        }

        void Start()
        {
            //StartCoroutine(WaitForEffect());
        }

        void FixedUpdate()
        {
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);
            Vector3 t = transform.forward;
            t.y = 0;
            GetComponent<Rigidbody>().velocity = t * speed;
        }
        /*
        IEnumerator WaitForEffect()
        {
            yield return new WaitForSeconds(3.0f);
            GameObject effect = Instantiate(BombEffect, transform.position, transform.rotation) as GameObject;
            Destroy(effect, 5.0f);
            Destroy(gameObject);
        }
        */
    }
}
