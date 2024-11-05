using UnityEngine;

public class Door : MonoBehaviour
{
    #region Variables

    [Header("Properties")]
    [SerializeField] private bool _left; // Define se a porta abre para esquerda
    [SerializeField] private float openSpeed = 2.0f; // Velocidade de abertura da porta
    [SerializeField] private float openHeight = 2.0f; // Altura que a porta deve subir

    [Header("Model")]
    [SerializeField] private Transform doorModel; // Modelo da porta que será movido

    [Header("State")]
    private bool isOpen = false; // Estado da porta (aberta ou fechada)
    public bool lockedByPassword = true; // Porta bloqueada até a senha ser correta

    [Header("Positions")]
    private Vector3 initialPosition; // Posição inicial da porta
    private Vector3 targetPosition; // Posição alvo da porta quando aberta

    #endregion

    #region Unity Callbacks
    private void Start()
    {
        initialPosition = doorModel.position;
        if (!_left) 
        { 
            targetPosition = initialPosition + Vector3.up * openHeight; // Define a posição alvo da porta
        }else
        {
            targetPosition = initialPosition + Vector3.left * openHeight; // Define a posição alvo da porta
        }
    }

    private void Update()
    {
        // Verifica se a porta está aberta e move a porta para a posição alvo
        if (isOpen)
        {
            // Move a porta suavemente para cima
            doorModel.position = Vector3.Lerp(doorModel.position, targetPosition, openSpeed * Time.deltaTime);
        }
        else
        {
            // Se a porta não estiver aberta, retorna à sua posição inicial
            doorModel.position = Vector3.Lerp(doorModel.position, initialPosition, openSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region ChangeState

    public void OpenDoor()
    {
        // Abre a porta somente se não estiver bloqueada
        if (!lockedByPassword)
        {
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        // Define o estado da porta como fechada
        isOpen = false;
    }

    public bool IsOpen()
    {
        // Retorna true se a porta estiver aberta, senão retorna false
        return isOpen;
    }

    #endregion
}
