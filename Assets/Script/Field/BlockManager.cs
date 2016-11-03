using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{
    public List<BlockState> blocks;

    public void Crash()
    {
        foreach (BlockState block in blocks)
        {
            block.Crash();
        }

        Destroy(gameObject);
    }

    List<GameObject> GetAll(GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(obj, ref allChildren);
        return allChildren;
    }

    void GetChildren(GameObject obj, ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform>();

        if (children.childCount == 0)
        {
            return;
        }
        foreach (Transform ob in children)
        {
            allChildren.Add(ob.gameObject);
            GetChildren(ob.gameObject, ref allChildren);
        }
    }

    void Start ()
    {
        List<GameObject> list = GetAll(gameObject);

        foreach (GameObject obj in list)
        {
            if(obj.tag == "Block")
            {
                BlockState blockState = obj.GetComponent<BlockState>();
                blocks.Add(blockState);
            }
        }

        if(transform.position.y < 0)
        {
            iTween.MoveTo(gameObject, 
			iTween.Hash
			(
				"y", 0f, 
				"time", 1.0f
			));
        }
    }
	
	void Update ()
    {
	
	}
}
