using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GetComponentInParent<Trap>().Hit(other);
    }
}
