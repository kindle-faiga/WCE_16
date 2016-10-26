using UnityEngine;
using System.Collections;

namespace WCE_16
{
    public class Bomb : MonoBehaviour
    {
        public GameObject BombEffect;

        void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Block")
            {
                if (!collision.transform.GetComponent<BlockState>().IsCrash())
                {
                    collision.transform.GetComponent<BlockState>().Hit(BombEffect);
                    Destroy(gameObject);
                }
            }
            if(collision.transform.tag != "Machine")
            {
                Debug.Log(collision.gameObject.name);
                Destroy(gameObject);
            }
        }
    }
}
