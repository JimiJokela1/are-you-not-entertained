using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpear : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GetComponentInParent<Enemy>().Hit(other);
    }
}
