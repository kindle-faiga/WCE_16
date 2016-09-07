using UnityEngine;
using System.Collections;

namespace WCE_16
{
    public class CameraManager : MonoBehaviour
    {
        private float moveRange = 5.0f;
        private float rangeAngle = 0.01f;
        private float maxAngle = 0.25f;
        private Vector3 offset;
        private Quaternion quaternion;
        private GameObject player;

        void Start()
        {
            quaternion = gameObject.transform.rotation;
            player = GameObject.FindGameObjectWithTag("Player");
            offset = transform.position - player.transform.position;
        }

        void Update()
        {
            //カメラを上下にも動かせるといいかもね

            float h = Input.GetAxis("CameraHorizontal");
            //float v = Input.GetAxis("CameraVertical");

            quaternion = gameObject.transform.rotation;
            float AngleY = quaternion.y;
            //float AngleX = quaternion.x;

            if ((h < 0 && -maxAngle < AngleY) || (0 < h && AngleY < maxAngle))
            {
                gameObject.transform.rotation = new Quaternion(quaternion.x, quaternion.y + h * rangeAngle, quaternion.z, quaternion.w);
            }
            /*
            if ((v < 0 && -maxAngle < AngleX) || (0 < v && AngleX < maxAngle))
            {
                gameObject.transform.rotation = new Quaternion(quaternion.x + v * rangeAngle, quaternion.y, quaternion.z, quaternion.w);
            }
            */

            if(Input.GetButtonDown("Reset"))
            {
                gameObject.transform.rotation = new Quaternion(0,0,0,0);
            }
        }

        void FixedUpdate()
        {
            Quaternion newQuaternion = transform.rotation;
            newQuaternion.y = player.transform.rotation.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, newQuaternion, moveRange * Time.deltaTime);

            Vector3 newPosition = transform.position;
            newPosition.x = player.transform.position.x + offset.x;
            newPosition.y = player.transform.position.y + offset.y;
            newPosition.z = player.transform.position.z + offset.z;
            transform.position = Vector3.Lerp(transform.position, newPosition, moveRange * Time.deltaTime);
            
            //transform.RotateAround(player.transform.position, transform.up, moveRange * Time.deltaTime);          
        }
    }
}
