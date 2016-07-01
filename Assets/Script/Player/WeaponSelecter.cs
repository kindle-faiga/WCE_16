using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace WCE_16
{
    public class WeaponSelecter : MonoBehaviour
    {
        public GameObject Bullet;
        public GameObject Bomb;
        private int weaponBoxNum;
        private bool isSelect = false;
        private float moveTime = 0.5f;
        private WEAPON_NAME weaponName;
        private GameObject weaponBox;
        private GameObject[] weaponSlots = new GameObject[3];
        private Image[] weaponFlameImage = new Image[3];
        private Image[] weaponImage = new Image[3];
        private Vector3 defaultSlotPos;
        private Vector3[] defaultSlotPoses = new Vector3[3];

        void Start()
        {
            //武器スロット取得
            weaponName = WEAPON_NAME.Bullet;

            weaponBox = GameObject.Find("UI/Panel/WeaponBox");
            weaponBoxNum = weaponBox.transform.childCount;

            for(int i=0; i<weaponBoxNum; ++i)
            {
                weaponSlots[i] = weaponBox.transform.GetChild(i).gameObject;
                defaultSlotPoses[i] = weaponSlots[i].transform.position;
                defaultSlotPos = defaultSlotPoses[0];

                weaponSlots[i].transform.position = defaultSlotPos;

                weaponFlameImage[i] = weaponSlots[i].GetComponent<Image>();
                weaponFlameImage[i].enabled = false;
                weaponImage[i] = weaponSlots[i].transform.FindChild("Sprite").GetComponent<Image>();
                weaponImage[i].enabled = false;
            }

            weaponFlameImage[0].enabled = true;
            weaponImage[0].enabled = true;
        }

        void Update()
        {
            if(Input.GetButtonDown("SelectWeapon"))
            {
                if (!isSelect)
                {
                    SelectWeapon();
                }
            }

            if (Input.GetButtonUp("SelectWeapon"))
            {
                ResetWeapon();
            }

            if (Input.GetButtonDown("UseWeapon"))
            {
                if(isSelect)
                {
                    SetWeapon();
                }
                else
                {
                    UseWeapon();
                }
            }
        }

        //武器変更呼び出し
        private void SelectWeapon()
        {
            isSelect = true;

            for (int i = 0; i < weaponBoxNum; ++i)
            {
                weaponFlameImage[i].enabled = true;
                weaponImage[i].enabled = true;

                iTween.MoveTo(weaponSlots[i], iTween.Hash("position", defaultSlotPoses[i], "time", moveTime));
            }
        }

        //武器使用
        private void UseWeapon()
        {
            switch(weaponName)
            {
                case WEAPON_NAME.Bullet:
                    GameObject bullet = Instantiate(Bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation) as GameObject;
                    Destroy(bullet, 2.0f);
                    break;
                case WEAPON_NAME.Bomb:
                    GameObject bomb = Instantiate(Bomb, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation) as GameObject;
                    Destroy(bomb, 5.0f);
                    break;
                case WEAPON_NAME.Special:

                    break;
            }
        }

        //武器変更
        private void SetWeapon()
        {
            ++weaponName;

            if (weaponName == WEAPON_NAME.End)
            {
                weaponName = 0;
            }

            //そのうちなおそう
            Vector3 pos = defaultSlotPoses[0];
            Vector3 pos1 = defaultSlotPoses[1];
            defaultSlotPoses[0] = defaultSlotPoses[2];
            defaultSlotPoses[1] = pos;
            defaultSlotPoses[2] = pos1;

            for (int i = 0; i < weaponBoxNum; ++i)
            {
                iTween.MoveTo(weaponSlots[i], iTween.Hash("position", defaultSlotPoses[i], "time", moveTime));
            }
        }

        //武器変更終了
        private void ResetWeapon()
        {
            isSelect = false;

            for (int i = 0; i < weaponBoxNum; ++i)
            {
                weaponImage[i].enabled = false;

                iTween.MoveTo(weaponSlots[i], iTween.Hash("position", defaultSlotPos, "time", moveTime, "oncomplete", "ResetImage", "oncompletetarget", gameObject));
            }

            weaponImage[(int)weaponName].enabled = true;
        }

        private void ResetImage()
        {
            for (int i = 0; i < weaponBoxNum; ++i)
            {
                weaponFlameImage[i].enabled = false;
            }

            weaponFlameImage[(int)weaponName].enabled = true;
        }
    }
}
