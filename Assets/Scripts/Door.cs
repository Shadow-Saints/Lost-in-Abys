using UnityEngine;

public class Door : MonoBehaviour
{
    #region Variables

    [Header("Properties")]
    [SerializeField] private bool _left; // Define se a porta abre
    [SerializeField] private float openSpeed = 2.0f; // Velocidade de abertura da porta
    [SerializeField] private float openHeight = 2.0f; // Altura que a porta deve subir

    [Header("Model")]
    [SerializeField] private Transform doorModel; // Modelo da porta que ser� movido

    [Header("State")]
    private bool isOpen = false; // Estado da porta (aberta ou fechada)

    [Header("Positions")]
    private Vector3 initialPosition; // Posi��o inicial da porta
    private Vector3 targetPosition; // Posi��o alvo da porta quando aberta

    #endregion

    #region Unity Callbacks
    private void Start()
    {
        initialPosition = doorModel.position;
        if (!_left) 
        { 
            targetPosition = initialPosition + Vector3.up * openHeight; // Define a posi��o alvo da porta
        }else
        {
            targetPosition = initialPosition + Vector3.left * openHeight; // Define a posi��o alvo da porta
        }
    }

    private void Update()
    {
        // Verifica se a porta est� aberta e move a porta para a posi��o alvo
        if (isOpen)
        {
            // Move a porta suavemente para cima
            doorModel.position = Vector3.Lerp(doorModel.position, targetPosition, openSpeed * Time.deltaTime);
        }
        else
        {
            // Se a porta n�o estiver aberta, retorna � sua posi��o inicial
            doorModel.position = Vector3.Lerp(doorModel.position, initialPosition, openSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region ChangeState

    public void OpenDoor()
    {
        // Define o estado da porta como aberta
        isOpen = true;
    }

    public void CloseDoor()
    {
        // Define o estado da porta como fechada
        isOpen = false;
    }

    public bool IsOpen()
    {
        // Retorna true se a porta estiver aberta, sen�o retorna false
        return isOpen;
    }

    #endregion
}
