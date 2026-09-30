using System.Collections.Generic;
using System.Linq;
using RAXY.InteractionSystem;
using UnityEngine;

public class SampleInteractableListUI : MonoBehaviour
{
    [SerializeField]
    Transform interactableUiContainer;

    [SerializeField]
    SampleInteractableRowUI interactableUiPrefab;

    [SerializeField]
    Interactor interactorToListen;

    Interactor _interactor;
    readonly List<SampleInteractableRowUI> _spawnedRows = new();

    void Start()
    {
        if (interactorToListen != null)
            Setup(interactorToListen);
    }

    void OnDestroy()
    {
        Unsubscribe();
    }

    public void Setup(Interactor interactor)
    {
        Unsubscribe();
        _interactor = interactor;
        ClearSpawnedRows();

        if (_interactor == null)
        {
            SetVisible(false);
            return;
        }

        _interactor.OnInteractableUpdated += HandleInteractableUpdated;
        SyncFromInteractor();
    }

    void HandleInteractableUpdated(List<string> tags, int selectedIndex)
    {
        RefreshUi(tags, selectedIndex);
    }

    void SyncFromInteractor()
    {
        if (_interactor == null)
        {
            SetVisible(false);
            return;
        }

        var tags = _interactor.ScannedInteractables != null
            ? _interactor.ScannedInteractables.Select(i => i.InteractableTag).ToList()
            : new List<string>();

        RefreshUi(tags, _interactor.SelectedIndex);
    }

    void RefreshUi(List<string> tags, int selectedIndex)
    {
        if (interactableUiContainer == null || interactableUiPrefab == null)
        {
            Debug.LogError($"[{nameof(SampleInteractableListUI)}] Missing container or prefab reference.", this);
            return;
        }

        if (tags == null)
            tags = new List<string>();

        while (_spawnedRows.Count < tags.Count)
        {
            var ui = Instantiate(interactableUiPrefab, interactableUiContainer);
            ui.gameObject.SetActive(true);
            _spawnedRows.Add(ui);
        }

        while (_spawnedRows.Count > tags.Count)
        {
            var lastIndex = _spawnedRows.Count - 1;
            var ui = _spawnedRows[lastIndex];
            _spawnedRows.RemoveAt(lastIndex);
            Destroy(ui.gameObject);
        }

        for (int i = 0; i < tags.Count; i++)
        {
            var ui = _spawnedRows[i];
            ui.Setup(tags[i], _interactor, i);
            ui.SetSelected(i == selectedIndex);
        }

        SetVisible(tags.Count > 0);
    }

    void ClearSpawnedRows()
    {
        if (interactableUiContainer != null)
        {
            foreach (Transform child in interactableUiContainer)
                Destroy(child.gameObject);
        }

        _spawnedRows.Clear();
    }

    void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    void Unsubscribe()
    {
        if (_interactor != null)
            _interactor.OnInteractableUpdated -= HandleInteractableUpdated;
    }
}
