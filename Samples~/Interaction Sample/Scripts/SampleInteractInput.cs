using RAXY.InteractionSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleInteractInput : MonoBehaviour
{
    [SerializeField]
    Interactor interactor;

    void Awake()
    {
        if (interactor == null)
            interactor = GetComponent<Interactor>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null || interactor == null)
            return;

        if (keyboard.eKey.wasPressedThisFrame)
            interactor.Interact();

        if (keyboard.tabKey.wasPressedThisFrame)
            interactor.CycleDownInteractable();
    }
}
