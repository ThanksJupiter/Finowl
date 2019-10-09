using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevardCode : MonoBehaviour
{
    bool doOnlyOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ApperAnimation()
    {
        if (doOnlyOnce)
        {
            doOnlyOnce = false;
        }
    }
}
