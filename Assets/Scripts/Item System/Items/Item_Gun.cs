using UnityEngine;

public class Item_Gun : OldItem
{
    public GameObject bullet;
    public Transform spawnPoint;
    
    public override bool Interact(GameObject player)
    {
        return base.Interact(player);
    }
}