using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RAXY.InteractionSystem
{
    public class Interactable : MonoBehaviour
    {
        [TitleGroup("Tag")]
        [SerializeField]
        bool useTagProvider;

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

        [HorizontalGroup("Tag/Op")]
        [Button]
        [ShowIf("@useTagProvider")]
        void Find_TagProviderSO()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (so is IInteractableTagProvider)
                {
                    tagProviderObj = so;
                    return;
                }
            }

            tagProviderObj = null;
        }

        [HorizontalGroup("Tag/Op")]
        [Button]
        [ShowIf("@useTagProvider")]
        void Find_TagProviderObj()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                    continue;

                var behaviours = prefab.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var behaviour in behaviours)
                {
                    if (behaviour is IInteractableTagProvider)
                    {
                        tagProviderObj = behaviour;
                        return;
                    }
                }
            }

            tagProviderObj = null;
        }
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
