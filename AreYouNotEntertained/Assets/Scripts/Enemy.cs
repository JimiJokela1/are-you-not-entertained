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

    public Transform SpearPivot;
    public float SpearMoveDistance;
    public float SpearMoveTime;
    public AnimationCurve SpearMoveCurve;

    private float _spearMoveTimer = 0f;
    private bool _attacking = false;
    private bool _attackReturning = false;
    private float _initialSpearZ = 0f;

    public float AttackDistance;
    
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _rb = GetComponent<Rigidbody>();

        _health = Random.Range(MinHealth, MaxHealth + 1);
        _initialSpearZ = SpearPivot.localPosition.z;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance
            && !_attacking && !_attackReturning)
        {
            _attacking = true;
        }

        if (_attacking)
        {
            Vector3 pos = SpearPivot.localPosition;
            pos.z = _initialSpearZ + SpearMoveDistance *
                SpearMoveCurve.Evaluate(_spearMoveTimer / SpearMoveTime);
            SpearPivot.localPosition = pos;

            _spearMoveTimer += Time.deltaTime;

            if (_spearMoveTimer >= SpearMoveTime)
            {
                _spearMoveTimer = 0f;
                _attacking = false;
                _attackReturning = true;
            }
        }
        else if (_attackReturning)
        {
            Vector3 pos = SpearPivot.localPosition;
            pos.z = _initialSpearZ + SpearMoveDistance *
                SpearMoveCurve.Evaluate(1f - _spearMoveTimer / SpearMoveTime);
            SpearPivot.localPosition = pos;

            _spearMoveTimer += Time.deltaTime;

            if (_spearMoveTimer >= SpearMoveTime)
            {
                _spearMoveTimer = 0f;
                _attackReturning = false;
            }
        }

        if (!_attacking || !_attackReturning)
        {
            transform.LookAt(_player.transform, Vector3.up);
        }
    }

    void FixedUpdate()
    {
        if (_stunned)
            return;

        if (_attacking || _attackReturning)
            return;

        Vector3 movement = (_rb.position + MovementSpeed * (_player.transform.position - transform.position).normalized * Time.fixedDeltaTime);
        _rb.MovePosition(movement);
    }

    public void Hit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() && other.tag != "Sword")
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
