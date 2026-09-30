using RAXY.InteractionSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SampleInteractableRowUI : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    TextMeshProUGUI label;

    [SerializeField]
    GameObject selectionFrame;

    Interactor _interactor;
    int _listIndex;

    public void Setup(string message, Interactor interactor, int listIndex)
    {
        if (label != null)
            label.text = message ?? string.Empty;

        _interactor = interactor;
        _listIndex = listIndex;

        SetSelected(false);
        WireButton();
    }

    void WireButton()
    {
        if (button == null)
        {
            Debug.LogError($"[{nameof(SampleInteractableRowUI)}] Missing button reference.", this);
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        if (_interactor == null)
            return;

        _interactor.SetSelectedIndex(_listIndex);
        _interactor.Interact();
    }

    public void SetSelected(bool isSelected)
    {
        if (selectionFrame != null)
            selectionFrame.SetActive(isSelected);
    }
}
