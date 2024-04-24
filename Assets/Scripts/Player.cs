using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("InterConnections")]
    private HealthAndVariables healthAndVariables;

    [Header ("Movement")]
    private CharacterController _charController;
    private Vector3 _velocity;
    [SerializeField]private float _speed;
    [SerializeField] private float _jumpForce;

    [Header ("Camera Movement")]
    [SerializeField] private Transform _cameraPosition;
    [SerializeField]private float _sense;
    private Vector3 _mouseRotation;
    [SerializeField]private float _gravity;

    [Header ("Ground Check")]
    [SerializeField] private Transform _grounCheck;
    [SerializeField] private float _groundDistance;
    [SerializeField] private LayerMask _groundMask;
    private bool _isGrounded;


    #region Unity Callbacks
    private void Start()
    {
        _charController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        healthAndVariables = FindAnyObjectByType<HealthAndVariables>();
        GameController.instance.loadMenuState();
        GameController.instance.getBarText();
        GameController.instance.LoadPression();
        GameController.instance.getMenuConfig();
        GameController.instance.Textinho();
    }

    private void Update()
    {
        CameraMove();
        Gravity();

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Spike"))
        {
            healthAndVariables.Damege(10);
        }if (collision.collider.CompareTag("Heal"))
        {
            healthAndVariables.Damege(-10);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    #endregion

    #region Movement
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * _speed;
        float moveY = Input.GetAxis("Vertical") * _speed;
        
        Vector3 move = transform.right * moveX * Time.deltaTime+ transform.forward * moveY * Time.deltaTime;

        _charController.Move(move);

    }
    void Gravity()
    {
        _isGrounded = Physics.CheckSphere(_grounCheck.position, _groundDistance, _groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2;
        }

        _velocity.y += _gravity * Time.deltaTime;
        _charController.Move(_velocity * Time.deltaTime);
    }

    void Jump()
    {
        _velocity.y = Mathf.Sqrt(_jumpForce * _gravity * -2f);
    }
    #endregion

    #region Camera Movement

    void CameraMove() 
    {
        Vector3 mousePos = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
        _mouseRotation = new Vector3(_mouseRotation.x + mousePos.x * _sense * Time.deltaTime, _mouseRotation.y + mousePos.y * _sense * Time.deltaTime, 0);

        _mouseRotation.y = Mathf.Clamp(_mouseRotation.y, -20, 80);
        
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _mouseRotation.x, transform.eulerAngles.z);
        _cameraPosition.localEulerAngles = new Vector3(-_mouseRotation.y, _cameraPosition.localEulerAngles.y, _cameraPosition.localEulerAngles.z);
    }

    public void ChangeSense(float sense)
    {
        _sense = sense;
    }

    #endregion

    public void SaveButton()
    {
        GameController.instance.SaveGame();
    }

    public void ResumeGame()
    {
        GameController.instance.Resume();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        GameController.instance.Pause();
    }

    
}
