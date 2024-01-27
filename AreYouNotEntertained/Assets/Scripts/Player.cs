using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageDealer
{
    public float MovementSpeed = 1f;
    public Transform CameraTransform;
    public Plane GroundPlane = new Plane(Vector3.up, 0);

    public int Health = 100;

    public Transform SwordPivot;
    public AnimationCurve SwordSwingCurve;

    private Rigidbody _rb;
    private bool _swordSwinging = false;
    private float _swordSwingTimer = 0f;
    public float SwordSwingTime = 2f;
    public float SwordSwingStartAngle;
    public float SwordSwingEndAngle;
    private bool _swordReturningFromSwing = false;
    public int SwordDamage = 20;

    public int Score = 0;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Move
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        movement *= MovementSpeed;
        _rb.MovePosition(_rb.position + movement);

        // Rotate
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Ray mousePosInWorldRay = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(mousePosInWorldRay, out RaycastHit hit))
        {
            Vector3 lookAt = hit.point;
            lookAt.y = 1f;
            transform.LookAt(lookAt);
        }

        // Sword swing
        if (_swordSwinging)
        {
            Vector3 swordRot = SwordPivot.localRotation.eulerAngles;
            swordRot.y += 90 * SwordSwingCurve.Evaluate(_swordSwingTimer / SwordSwingTime);
            if (swordRot.y >= SwordSwingEndAngle)
            {
                _swordSwinging = false;
                _swordSwingTimer = 0f;
                _swordReturningFromSwing = true;
            }
            else
            {
                _swordSwingTimer += Time.deltaTime;
            }

            SwordPivot.localRotation = Quaternion.Euler(swordRot);
        }

        if (_swordReturningFromSwing)
        {
            Vector3 swordRot = SwordPivot.localRotation.eulerAngles;
            swordRot.y -= 90 * SwordSwingCurve.Evaluate((_swordSwingTimer * 0.5f) / SwordSwingTime);
            if (swordRot.y <= SwordSwingStartAngle)
            {
                _swordSwingTimer = 0f;
                _swordReturningFromSwing = false;
            }
            else
            {
                _swordSwingTimer += Time.deltaTime;
            }
        
            SwordPivot.localRotation = Quaternion.Euler(swordRot);;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Swing();
    }

    void Swing()
    {
        if (_swordSwinging || _swordReturningFromSwing)
            return;

        _swordSwinging = true;
        _swordSwingTimer = 0f;
    }

    public void TakeDamage(int damage, IDamageDealer damageSource)
    {
        Health -= damage;

        if (Health <= 0)
        {
            // Game Over
            Debug.Log("Game Over");
            SceneManager.LoadScene(0);
        }
        
        Flash();
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
