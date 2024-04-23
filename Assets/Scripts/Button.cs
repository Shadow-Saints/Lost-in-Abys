using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{
    #region Variables

    [Header("Mouse")]
    private bool _mouseEnter;

    [Header("Door")]
    [SerializeField]private Door door;
    private bool canClose = true;

    [Header("Delay")]
    [SerializeField]private float delayBeforeClose = 3f;

    [Header("Audio")]
    private AudioSource _sound;

    #endregion

    #region Unity Callbacks
    private void OnMouseEnter()
    {
        _mouseEnter = true;
        GameController.instance.ActiveTutorial(1);
    }

    private void OnMouseExit()
    {
        _mouseEnter = false;
        GameController.instance.DisableTutorial(1);
    }

    private void Start()
    {
        _sound = GetComponent<AudioSource>();  
    }

    private void Update()
    {
        // Verifica se o jogador pressionou a tecla "E" e se a porta pode ser fechada
        if (Input.GetKeyDown(KeyCode.E) && canClose)
        {
            // Verifica se o jogador está dentro do trigger do botão
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f);
            foreach (Collider collider in colliders)
            {
                if (Input.GetKeyDown(KeyCode.E) && _mouseEnter)
                {
                        _sound.Play();
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
    #endregion

    #region Delays

    // Coroutine para habilitar a capacidade de fechar a porta após o atraso
    IEnumerator EnableClose()
    {
        yield return new WaitForSeconds(delayBeforeClose);
        canClose = true;
    }

    #endregion
}
