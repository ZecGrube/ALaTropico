using UnityEngine;
using CaudilloBay.Data;

namespace CaudilloBay.Data
{
    public static class ArtifactResource
    {
        public static ResourceType CreateArtifacts(Sprite icon)
        {
            ResourceType artifacts = ScriptableObject.CreateInstance<ResourceType>();
            artifacts.resourceId = "artifacts";
            artifacts.resourceName = "Artifacts";
            artifacts.icon = icon;
            artifacts.unitWeight = 0.5f;
            artifacts.baseValue = 500f;
            return artifacts;
        }
    }
}
