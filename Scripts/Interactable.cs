using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace RAXY.InteractionSystem
{
    public class Interactable : MonoBehaviour
    {
        [TitleGroup("Tag")]
        [HideIf("@useTagProvider")]
        [SerializeField]
        [LabelText("Interactable Tag")]
        string interactableTag;

        [TitleGroup("Tag")]
        [ShowIf("@useTagProvider")]
        [SerializeField]
        [LabelText("Interactable Tag")]
        [ValueDropdown("Tags")]
        string interactableDropdownTag;

        [TitleGroup("Tag")]
        [SerializeField]
        bool useTagProvider;

        public string InteractableTag
        {
            get
            {
                return useTagProvider ? 
                        interactableDropdownTag : 
                        interactableTag;
            }
        }

#if UNITY_EDITOR
        [TitleGroup("Tag")]
        [SerializeField]
        [ShowIf("@useTagProvider")]
        Object tagProviderObj;

        IInteractableTagProvider TagProvider
        {
            get
            {
                if (tagProviderObj is not null and IInteractableTagProvider tagProvider)
                    return tagProvider;
                else
                    return null;
            }
        }

        List<string> Tags => TagProvider != null ? TagProvider.Tags : new List<string>();
#endif

        [TitleGroup("Events")]
        [FoldoutGroup("Events/Events")]
        [FormerlySerializedAs("OnScanned")]
        public UnityEvent OnScanEnter;

        [FoldoutGroup("Events/Events")]
        public UnityEvent OnScanExit;

        [FoldoutGroup("Events/Events")]
        public UnityEvent OnInteracted;

        public void Interact()
        {
            OnInteracted?.Invoke();
        }
    }
}
