using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TypeTextAnimation : MonoBehaviour
{
    public Action TypeFinished;

    public float typeDelay = 0.05f;
    public TextMeshProUGUI textObject;

    public string fullText;

    Coroutine coroutine;

    public void StartTyping()
    {
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
        StopCoroutine(coroutine);
        textObject.text = fullText;
    }
}