using System.Runtime.InteropServices;
using CustomInspector;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController1 : MonoBehaviour
{
    [ReadOnly] public CharacterController controller;
    [ReadOnly] public Animator animator;   

    [ReadOnly] public bool isGrounded;

    void Awake()
    {
        TryGetComponent(out controller);
        TryGetComponent(out animator);
    }

    
}