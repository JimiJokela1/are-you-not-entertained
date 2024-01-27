using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageDealer
{
    Player _player;
    private Rigidbody _rb;
    public int Damage;
    public int MovementSpeed;
    public int MinHealth;
    public int MaxHealth;
    [SerializeField]
    int _health;

    private bool _stunned = false;
    public float KnockbackDistance = 1f;
    public float StunTime = 1f;
    
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _rb = GetComponent<Rigidbody>();

        _health = Random.Range(MinHealth, MaxHealth + 1);
    }

    void FixedUpdate()
    {
        if (_stunned)
            return;

        Vector3 movement = (_rb.position + MovementSpeed * (_player.transform.position - transform.position).normalized * Time.fixedDeltaTime);
        _rb.MovePosition(movement);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() && collision.collider.tag != "Sword")
        {
            _player.TakeDamage(Damage, this);
        }
    }

    public void TakeDamage(int damage, Vector3 sourceVector, IDamageDealer damageSource)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(gameObject);

            if (damageSource is Player)
            {
                _player.Score += 10;
            }
        }

        _stunned = true;
        Vector3 knockback = (transform.position - sourceVector).normalized * KnockbackDistance;
        knockback.y = 0f;
        transform.position += knockback;

        Invoke("StopStun", StunTime);

        Flash();
    }

    void StopStun()
    {
        _stunned = false;
    }

    void Flash()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);

        Invoke("StopFlash", 0.1f);
    }

    void StopFlash()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
    }
}
