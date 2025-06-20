using UnityEngine;
using UnityEngine.UI.Extensions;

namespace GF.Editor.Sample
{
    [CreateAssetMenu(fileName = "Sample", menuName = "Samples/ReadOnly", order = 500)]
    public class ReadOnlySample : ScriptableObject
    {
#pragma warning disable CS0414 // Warning for unused property
        [Tooltip("Public field")]
        public string PublicField = "visible, editable, saved";
        [Tooltip("Private field")]
        private string PrivateField = "non-visible, non-editable, unsaved";

        [SerializeField, Tooltip("Public field with SerializeField attribute")]
        public string PublicSerializedField = "visible, editable, saved";
        [SerializeField, Tooltip("Private field with SerializeField attribute")]
        private string PrivateSerializedField = "visible, editable, saved";

        [ReadOnly, Tooltip("Public field with ReadOnlyInInspector attribute")]
        public string PublicReadOnlyInInspector = "visible, non-editable, saved";
        [ReadOnly, Tooltip("Private field with ReadOnlyInInspector attribute")]
        private string PrivateReadOnlyInInspector = "visible, non-editable, unsaved";

        [SerializeField, ReadOnly, Tooltip("Public field with SerializeField and ReadOnlyInInspector attribute")]
        public string PublicSerializedReadOnlyInInspector = "visible, non-editable, saved";
        [SerializeField, ReadOnly, Tooltip("Private field with SerializeField and ReadOnlyInInspector attribute")]
        private string PrivateSerializedReadOnlyInInspector = "visible, non-editable, saved";
#pragma warning restore CS0414 // Warning for unused property
    }
}