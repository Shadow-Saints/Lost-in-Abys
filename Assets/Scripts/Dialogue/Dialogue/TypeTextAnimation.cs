using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TypeTextAnimation : MonoBehaviour
{
    public Action TypeFinished;

    public float typeDelay = 0.05f;
    public TextMeshProUGUI textObject; // Verifique no Inspector se este objeto foi associado

    public string fullText;

    Coroutine coroutine;

    public void StartTyping()
    {
        if (textObject == null)
        {
            Debug.LogError("textObject n�o foi associado no Inspector! Certifique-se de que TextMeshProUGUI est� associado.");
            return;
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textObject.text = "";
        foreach (char c in fullText)
        {
            textObject.text += c;
            yield return new WaitForSeconds(typeDelay);
        }

        TypeFinished?.Invoke();
    }

    public void Skip()
    {
        if (coroutine == null)
        {
            Debug.LogWarning("Skip foi chamado, mas a coroutine ainda n�o foi inicializada.");
            return;
        }

        StopCoroutine(coroutine);
        textObject.text = fullText;
    }
}
