using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Door door;
    public float delayBeforeClose = 3f; // Tempo de espera antes que a porta possa ser fechada novamente
    private bool canClose = true; // Controla se a porta pode ser fechada

    private void Update()
    {
        // Verifica se o jogador pressionou a tecla "E" e se a porta pode ser fechada
        if (Input.GetKeyDown(KeyCode.E) && canClose)
        {
            // Verifica se o jogador está dentro do trigger do botão
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // Chama a função para abrir ou fechar a porta se o jogador estiver dentro do trigger
                    if (door.IsOpen())
                        door.CloseDoor(); // Fecha a porta se estiver aberta
                    else
                        door.OpenDoor(); // Abre a porta se estiver fechada

                    canClose = false; // Define que a porta não pode ser fechada temporariamente
                    StartCoroutine(EnableClose()); // Inicia a coroutine para habilitar a capacidade de fechar a porta novamente
                    break;
                }
            }
        }
    }

    // Coroutine para habilitar a capacidade de fechar a porta após o atraso
    IEnumerator EnableClose()
    {
        yield return new WaitForSeconds(delayBeforeClose);
        canClose = true;
    }
}
