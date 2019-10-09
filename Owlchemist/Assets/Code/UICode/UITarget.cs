using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITarget : MonoBehaviour
{
    public bool closed = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void  MyFuction()
    {

    }
    public virtual void CloseUI()
    {

    }
    public void Selected()
    {
        GetComponent<MeshRenderer>().material.SetColor("_FresnelCol", Color.cyan);
        GetComponent<MeshRenderer>().material.SetFloat("_FresnelScale",600);
    }
    public void UnSelected()
    {
       GetComponent<MeshRenderer>().material.SetColor("_FresnelCol", Color.white);
       GetComponent<MeshRenderer>().material.SetFloat("_FresnelScale", 4);
    }
}
