using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateNeurons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform child in gameObject.transform)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(true);
                return;
            }
        }
    }
}
