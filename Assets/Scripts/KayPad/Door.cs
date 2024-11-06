using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float openSpeed = 2.0f; // Velocidade de abertura da porta
    public float openHeight = 2.0f; // Altura que a porta deve subir
    public Transform doorModel; // Modelo da porta que será movido
    public bool lockedByPassword = true; // Controla se a porta está trancada pela senha

    private bool isOpen = false; // Estado da porta (aberta ou fechada)
    private Vector3 initialPosition; // Posição inicial da porta
    private Vector3 targetPosition; // Posição alvo da porta quando aberta

    private void Start()
    {
        initialPosition = doorModel.position;
        targetPosition = initialPosition + Vector3.up * openHeight; // Define a posição alvo da porta
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

    public void OpenDoor()
    {
        if (!lockedByPassword) // Verifica se a porta está destrancada
        {
            isOpen = true;
            StartCoroutine(CloseDoorAfterDelay(5.0f)); // Inicia a corrotina para fechar a porta após 5 segundos
        }
        else
        {
            Debug.Log("Porta está trancada por senha!");
        }
    }

    // Método para fechar a porta
    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Aguarda o tempo especificado
        CloseDoor(); // Fecha a porta
    }

    public void CloseDoor()
    {
        isOpen = false;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
