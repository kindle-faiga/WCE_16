using UnityEngine;
using System.Collections;

namespace WCE_16
{
    public class Smoke : MonoBehaviour
    {
        public float exhaustRate;

        [SerializeField]
        private float engineRevs;
        [SerializeField]
        private ParticleSystem exhaust;


        void Start() 
        {
            exhaust = GetComponent<ParticleSystem>();
        }


        void Update()
        {
            exhaust.emissionRate = engineRevs * exhaustRate;
        }
        /*
        void FixedUpdate()
        {
            engineRevs = Mathf.Abs(transform.parent.GetComponent<CharacterController>().getH());
        }
        */
    }
}
