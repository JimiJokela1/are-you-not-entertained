using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IDamageDealer
{
    public Transform TrapSpikes;
    public AnimationCurve SpikesMoveCurve;
    public float SpikesMovementTime = 1f;
    public float SpikesMovementDistance = 1f;
    public int Damage = 20;

    private float _initialSpikesY;
    private bool _spikesMovingUp = true;
    private float _spikesMovementTimer = 0f;
    private Player _player;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _initialSpikesY = TrapSpikes.localPosition.y;
    }

    void Update()
    {
        if (_spikesMovingUp)
        {
            Vector3 pos = TrapSpikes.localPosition;
            pos.y = _initialSpikesY + SpikesMovementDistance *
                SpikesMoveCurve.Evaluate(_spikesMovementTimer / SpikesMovementTime);
            TrapSpikes.localPosition = pos;

            _spikesMovementTimer += Time.deltaTime;

            if (_spikesMovementTimer >= SpikesMovementTime)
            {
                _spikesMovementTimer = 0f;
                _spikesMovingUp = false;
            }
        }
        else
        {
            Vector3 pos = TrapSpikes.localPosition;
            pos.y = _initialSpikesY + SpikesMovementDistance *
                SpikesMoveCurve.Evaluate(1f - _spikesMovementTimer / SpikesMovementTime);
            TrapSpikes.localPosition = pos;
            
            _spikesMovementTimer += Time.deltaTime;

            if (_spikesMovementTimer >= SpikesMovementTime)
            {
                _spikesMovementTimer = 0f;
                _spikesMovingUp = true;
            }
        }
    }

    public void Hit(Collider other)
    {
        if (other.GetComponent<Player>() && other.tag != "Sword")
        {
            _player.TakeDamage(Damage, transform.position, this);
        }

        Enemy enemy = other.GetComponent<Enemy>() as Enemy;
        if (enemy != null)
        {
            enemy.TakeDamage(Damage, transform.position, this);
        }
    }
}
