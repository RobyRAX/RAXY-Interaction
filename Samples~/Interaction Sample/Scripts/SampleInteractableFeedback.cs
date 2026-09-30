using RAXY.InteractionSystem;
using TMPro;
using UnityEngine;

public class SampleInteractableFeedback : MonoBehaviour
{
    [SerializeField]
    Color scannedColor = new(1f, 0.85f, 0.2f, 1f);

    [SerializeField]
    Color interactedColor = new(0.35f, 0.9f, 0.45f, 1f);

    [SerializeField]
    TextMeshPro worldLabel;

    Interactable _interactable;
    Renderer _renderer;
    Material _materialInstance;
    Color _idleColor = Color.white;
    string _tag;

    void Awake()
    {
        _interactable = GetComponent<Interactable>();
        _renderer = GetComponent<Renderer>();

        if (_renderer != null)
        {
            _materialInstance = _renderer.material;
            _idleColor = _materialInstance.HasProperty("_BaseColor")
                ? _materialInstance.GetColor("_BaseColor")
                : _materialInstance.color;
        }

        _tag = _interactable != null ? _interactable.InteractableTag : name;
        SetLabel(_tag);
    }

    void OnEnable()
    {
        if (_interactable == null)
            return;

        _interactable.OnScanEnter.AddListener(HandleScanEnter);
        _interactable.OnScanExit.AddListener(HandleScanExit);
        _interactable.OnInteracted.AddListener(HandleInteracted);
    }

    void OnDisable()
    {
        if (_interactable == null)
            return;

        _interactable.OnScanEnter.RemoveListener(HandleScanEnter);
        _interactable.OnScanExit.RemoveListener(HandleScanExit);
        _interactable.OnInteracted.RemoveListener(HandleInteracted);
    }

    void OnDestroy()
    {
        if (_materialInstance != null)
            Destroy(_materialInstance);
    }

    void HandleScanEnter()
    {
        ApplyColor(scannedColor);
    }

    void HandleScanExit()
    {
        ApplyColor(_idleColor);
        SetLabel(_tag);
    }

    void HandleInteracted()
    {
        ApplyColor(interactedColor);
        SetLabel(_tag + " — done");
        Debug.Log($"[{nameof(SampleInteractableFeedback)}] Interacted: {_tag}", this);
    }

    void ApplyColor(Color color)
    {
        if (_materialInstance == null)
            return;

        if (_materialInstance.HasProperty("_BaseColor"))
            _materialInstance.SetColor("_BaseColor", color);

        _materialInstance.color = color;
    }

    void SetLabel(string text)
    {
        if (worldLabel != null)
            worldLabel.text = text ?? string.Empty;
    }
}
