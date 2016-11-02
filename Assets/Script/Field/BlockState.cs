using UnityEngine;
using System.Collections;

public class BlockState : MonoBehaviour
{
    bool isCrashed = false;

    public BlockManager blockManager;

    public void Start()
    {
        blockManager = GetComponentInParent<BlockManager>();
    }

    public bool IsCrash()
    {
        return isCrashed;
    }

    public void Hit(GameObject bombEffect)
    {
        blockManager.Crash();
        GameObject effect = Instantiate(bombEffect, blockManager.transform.position, blockManager.transform.rotation) as GameObject;
        Destroy(effect, 1.0f);
    }

    public void Crash()
    {
        if (!isCrashed)
        {
            isCrashed = true;
        }

        GetComponent<Rigidbody>().isKinematic = false;

        transform.tag = "Item";

        //StartCoroutine(WaitForCrash());
    }

    IEnumerator WaitForCrash()
    {
        yield return new WaitForSeconds(0f);
        iTween.ScaleTo(gameObject, transform.localScale * 0.75f, 0.5f);
    }
}
