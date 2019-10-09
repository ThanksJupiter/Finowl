using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICartLeave : UITarget
{
    public GameObject cart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void MyFuction()
    {
        cart.GetComponent<CartInteraction>().LeaveCart();
    }
}
