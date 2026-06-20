using System.Collections.Generic;
using UnityEngine;

namespace RAXY.InteractionSystem
{
    public interface IInteractableTagProvider
    {
        public List<string> Tags { get; }
    }
}
