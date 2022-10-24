using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [SerializeField]
    Collectable[] collectables;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    [ContextMenu("Init")]
    public void Init()
    {
        if(collectables == null)
            collectables = FindObjectsOfType<Collectable>();

        for (int i = 0; i < collectables.Length; i++)
        {
            collectables[i].Activate();
        }
    }
}
