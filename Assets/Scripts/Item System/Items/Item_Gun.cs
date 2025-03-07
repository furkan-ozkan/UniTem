using System;
using UnityEngine;

public class Item_Gun : Item
{
    public GameObject bullet;
    public Transform spawnPoint;
    
    public override bool Interact(GameObject player)
    {
        itemData.AlreadyFound = true;
        ActionContext context = new Context_Action_Fire(bullet, spawnPoint.position, spawnPoint.forward, 1f, 10f);
        _actionInvoker.QueueAction(new Action_Fire(), context);
        
        return base.Interact(player);
    }
}