using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Anim : MonoBehaviour
{
    [SerializeField] Animator controller;

    private void Start()
    {
        controller.SetBool("Abre", true);
    }
}
