using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public void Hit(Collider other)
    {
        Player player = other.GetComponent<Player>() as Player;
        if (player != null)
        {
            player.CollectPickup();
            FindObjectOfType<PickupSpawner>().NotifyDestroyedPickup(gameObject);
            Destroy(gameObject);
        }
    }
}
