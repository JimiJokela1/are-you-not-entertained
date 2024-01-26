using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Player _player;
    private Rigidbody _rb;
    public int Damage;
    public int MovementSpeed;
    
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 movement = (_rb.position + MovementSpeed * (_player.transform.position - transform.position).normalized * Time.fixedDeltaTime);
        _rb.MovePosition(movement);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() && collision.collider.tag != "Sword")
        {
            _player.TakeDamage(Damage);
        }

        if (collision.collider.tag == "Sword")
        {
            Destroy(gameObject);
        }
    }
}
