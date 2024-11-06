using UnityEngine;

public enum STATE
{
    DISABLED,
    WAITING,
    TYPING
}

public class DialogueSystem : MonoBehaviour
{
    DialogueData dialogueData;

    int currentText = 0;
    bool finished = false;

    TypeTextAnimation typeText;
    DialogueUI dialogueUI;

    STATE state;

    void Awake()
    {
        typeText = FindObjectOfType<TypeTextAnimation>();
        dialogueUI = FindObjectOfType<DialogueUI>();

        typeText.TypeFinished = OnTypeFinished;
        Debug.Log("DialogueSystem Awake: Componentes TypeTextAnimation e DialogueUI inicializados.");
    }

    void Start()
    {
        state = STATE.DISABLED;
        Debug.Log("DialogueSystem Start: Estado inicial definido como DISABLED.");
    }

    void Update()
    {
        if (state == STATE.DISABLED)
            return;

        Debug.Log("DialogueSystem Update: Estado atual - " + state);

        switch (state)
        {
            case STATE.WAITING:
                Waiting();
                break;
            case STATE.TYPING:
                Typing();
                break;
        }
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        Debug.Log("Iniciando diálogo com DialogueData: " + dialogueData);

        this.dialogueData = dialogueData;
        currentText = 0;
        finished = false;

        dialogueUI.Enable();
        Next();

        Debug.Log("Diálogo iniciado.");
    }

    public void Next()
    {
        if (dialogueData == null || dialogueData.talkScript.Count == 0)
        {
            Debug.LogWarning("DialogueData está nulo ou talkScript está vazio.");
            return;
        }

        if (currentText == 0)
        {
            dialogueUI.Enable();
            Debug.Log("Diálogo habilitado.");
        }

        Debug.Log("Configurando fala do personagem: " + dialogueData.talkScript[currentText].name);
        dialogueUI.SetName(dialogueData.talkScript[currentText].name);
        typeText.fullText = dialogueData.talkScript[currentText++].text;

        if (currentText == dialogueData.talkScript.Count)
        {
            finished = true;
            Debug.Log("Último texto do diálogo alcançado.");
        }

        typeText.StartTyping();
        state = STATE.TYPING;
    }

    void OnTypeFinished()
    {
        Debug.Log("Texto digitado terminado.");
        state = STATE.WAITING;
    }

    void Waiting()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter pressionado no estado WAITING.");
            if (!finished)
            {
                Next();
            }
            else
            {
                dialogueUI.Disable();
                state = STATE.DISABLED;
                currentText = 0;
                finished = false;

                Debug.Log("Diálogo terminado, interação habilitada.");
                FindObjectOfType<Player>().EnableInteraction();
            }
        }
    }

    void Typing()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter pressionado no estado TYPING, pulando animação.");
            typeText.Skip();
            state = STATE.WAITING;
        }
    }

    public STATE GetState()
    {
        return state;
    }
}
