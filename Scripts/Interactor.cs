using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RAXY.InteractionSystem
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField]
        [SuffixLabel("units")]
        float scanRange = 10f;

        [SerializeField]
        LayerMask interactableLayer;

        [SerializeField]
        [SuffixLabel("seconds")]
        float scanDelay;

        [Title("Runtime")]
        [ShowInInspector]
        public int SelectedIndex { get; set; }

        [ShowInInspector]
        public Interactable SelectedInteractable => 
            ScannedInteractables != null && ScannedInteractables.Count > 0 && SelectedIndex >= 0 && SelectedIndex < ScannedInteractables.Count
                ? ScannedInteractables[SelectedIndex]
                : null;

        [ShowInInspector]
        [ListDrawerSettings(ShowIndexLabels = true)]
        public List<Interactable> ScannedInteractables { get; set; } = new();

        /// <summary>
        /// parameters: List<string> interactableTags, int selectedIndex
        /// </summary>
        public event Action<List<string>, int> OnInteractableUpdated;

        private Coroutine scanCoroutine;
        private List<Interactable> _lastEmittedInteractables;
        private int _lastEmittedSelectedIndex = -1;

        private void Awake()
        {
            if (ScannedInteractables == null)
                ScannedInteractables = new List<Interactable>();

            SelectedIndex = 0;
        }

        private void OnEnable()
        {
            StartScanCoroutine();
        }

        private void OnDisable()
        {
            StopScanCoroutine();
        }

        private void StartScanCoroutine()
        {
            if (scanCoroutine != null)
                return;

            scanCoroutine = StartCoroutine(ScanInteractablesCo());
        }

        private void StopScanCoroutine()
        {
            if (scanCoroutine != null)
            {
                StopCoroutine(scanCoroutine);
                scanCoroutine = null;
            }
        }

        public void Set_ScanRange(float newRange)
        {
            scanRange = newRange;
        }

        public void Set_ScanDelay(float newDelay)
        {
            scanDelay = newDelay;
        }

        public void Set_InteractableScanLayer(LayerMask layer)
        {
            interactableLayer = layer;
        }

        [TitleGroup("Debug Function")]
        [Button]
        public void ScanInteractables()
        {
            if (ScannedInteractables == null)
                ScannedInteractables = new List<Interactable>();

            ScannedInteractables.Clear();

            // Get all colliders within range using OverlapSphere
            var colliders = Physics.OverlapSphere(transform.position, scanRange, interactableLayer);

            // Extract Interactable components
            foreach (var collider in colliders)
            {
                var interactable = collider.GetComponent<Interactable>();
                if (interactable != null && !ScannedInteractables.Contains(interactable))
                    ScannedInteractables.Add(interactable);
            }

            // Reset index if out of bounds
            if (ScannedInteractables.Count > 0)
            {
                SelectedIndex = Mathf.Clamp(SelectedIndex, 0, ScannedInteractables.Count - 1);
            }
            else
            {
                SelectedIndex = 0;
            }

            RaiseInteractableUpdatedEvent();
        }

        [TitleGroup("Debug Function")]
        [Button]
        public void CycleUpInteractable()
        {
            if (ScannedInteractables == null || ScannedInteractables.Count == 0)
                return;

            // Move to previous interactable (wrap around to end if at start)
            SelectedIndex--;
            if (SelectedIndex < 0)
                SelectedIndex = ScannedInteractables.Count - 1;

            RaiseInteractableUpdatedEvent();
        }
        
        [TitleGroup("Debug Function")]
        [Button]
        public void CycleDownInteractable()
        {
            if (ScannedInteractables == null || ScannedInteractables.Count == 0)
                return;

            // Move to next interactable (wrap around to start if at end)
            SelectedIndex++;
            if (SelectedIndex >= ScannedInteractables.Count)
                SelectedIndex = 0;

            RaiseInteractableUpdatedEvent();
        }

        public void SetSelectedIndex(int index)
        {
            if (ScannedInteractables == null || ScannedInteractables.Count == 0)
                return;

            SelectedIndex = Mathf.Clamp(index, 0, ScannedInteractables.Count - 1);
            RaiseInteractableUpdatedEvent();
        }

        [TitleGroup("Debug Function")]
        [Button]
        public void Interact()
        {
            if (SelectedInteractable == null)
            {
                Debug.Log("No interactable selected");
                return;
            }

            SelectedInteractable.Interact();
        }

        private bool HasInteractableStateChanged()
        {
            if (_lastEmittedInteractables == null)
                return true;

            if (_lastEmittedSelectedIndex != SelectedIndex)
                return true;

            if (_lastEmittedInteractables.Count != ScannedInteractables.Count)
                return true;

            for (int i = 0; i < ScannedInteractables.Count; i++)
            {
                if (_lastEmittedInteractables[i] != ScannedInteractables[i])
                    return true;
            }

            return false;
        }

        private void RaiseInteractableUpdatedEvent()
        {
            if (!HasInteractableStateChanged())
                return;

            _lastEmittedSelectedIndex = SelectedIndex;
            _lastEmittedInteractables = new List<Interactable>(ScannedInteractables);

            var tags = _lastEmittedInteractables.Select(i => i.InteractableTag).ToList();
            OnInteractableUpdated?.Invoke(tags, SelectedIndex);
        }

        private IEnumerator ScanInteractablesCo()
        {
            var wait = new WaitForSeconds(scanDelay);

            while (true)
            {
                ScanInteractables();
                yield return wait;
            }
        }
    }
}
