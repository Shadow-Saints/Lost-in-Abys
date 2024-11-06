using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    [Header("InterConnections")]
    private HealthAndVariables healthAndVariables;

    [Header("Movement")]
    private CharacterController _charController;
    private Vector3 _velocity;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;

    [Header("Camera Movement")]
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private float _sense;
    private Vector3 _mouseRotation;

    [Header("Ground Check")]
    [SerializeField] private Transform _grounCheck;
    [SerializeField] private float _groundDistance;
    [SerializeField] private LayerMask _groundMask;
    private bool _isGrounded;

    [Header("Dialogue")]
    DialogueSystem dialogueSystem;
    NPCManager npcManager;
    bool canInteract = true;
    [SerializeField] private float interactionDistance = 3.0f;

    public void EnableInteraction()
    {
        canInteract = true;
        Debug.Log("Interação habilitada novamente.");
    }

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        _charController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        healthAndVariables = FindAnyObjectByType<HealthAndVariables>();

        Debug.Log("Player Start: Configuração inicializada.");
    }

    private void Awake()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        npcManager = FindObjectOfType<NPCManager>();

        if (dialogueSystem == null) Debug.LogWarning("DialogueSystem não encontrado.");
        if (npcManager == null) Debug.LogWarning("NPCManager não encontrado.");
    }

    private void Update()
    {
        if (dialogueSystem.GetState() == STATE.DISABLED)
        {
            CameraMove();
        }

        Gravity();

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.instance.Pause();
        }

        Interact();
    }

    private void FixedUpdate()
    {
        if (dialogueSystem.GetState() == STATE.DISABLED)
        {
            Move();
        }
    }

    #endregion

    #region Movement

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * _speed;
        float moveY = Input.GetAxis("Vertical") * _speed;

        Vector3 move = transform.right * moveX * Time.deltaTime + transform.forward * moveY * Time.deltaTime;
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

    #region Interaction

    // Método para interação com portas e keypad
    void Interact()
    {
        if (!canInteract) return;

        // Tenta encontrar o NPC mais próximo usando a posição do jogador e a distância de interação
        NPCData closestNPC = npcManager.GetClosestNPC(transform.position, interactionDistance);

        // Verifica se o NPC mais próximo foi encontrado e se está dentro do alcance de interação
        if (closestNPC.npc != null && Vector3.Distance(transform.position, closestNPC.npc.transform.position) <= interactionDistance)
        {
            Debug.Log($"NPC próximo encontrado: {closestNPC.npc.name}");

            // Inicia o diálogo com o NPC mais próximo
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueSystem.StartDialogue(closestNPC.dialogueData);
                canInteract = false;
                Debug.Log("Iniciando diálogo com o NPC.");
            }
        }
        else
        {
            Debug.Log("Nenhum NPC próximo para interação.");
        }

        // Interação com objetos como portas e teclados
        RaycastHit hit;
        if (Physics.Raycast(_cameraPosition.position, _cameraPosition.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(_cameraPosition.position, _cameraPosition.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance <= interactionDistance)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.transform.GetComponent<KeypadKey>() != null)
                    {
                        Debug.Log("Interagindo com o teclado.");
                        hit.transform.GetComponent<KeypadKey>().SendKey();
                    }
                    else if (hit.transform.GetComponent<Door>() != null)
                    {
                        Debug.Log("Interagindo com a porta.");
                        hit.transform.GetComponent<Door>().OpenDoor();
                    }
                }
            }
        }
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
}
