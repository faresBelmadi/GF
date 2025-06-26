using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.CM.Common.CmCallContext;

[CustomPropertyDrawer(typeof(TradId))]
public class TradIdCustomEditor : PropertyDrawer
{
    private static TradManager _tradMgr;
    static TradIdCustomEditor()
    {
        _tradMgr = TradManager.CreateEditorInstance();
        _tradMgr.LoadTrad();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUIStyle boldStyle = new(EditorStyles.label) { fontStyle = FontStyle.Bold };
        EditorGUILayout.LabelField(property.displayName, boldStyle);

        EditorGUI.indentLevel++;
        {
            SerializedProperty idProp = property.FindPropertyRelative(nameof(TradId.Id));
            string newId = EditorGUILayout.TextField(idProp.displayName, idProp.stringValue);
            idProp.stringValue = newId;

            SerializedProperty defaultTextProp = property.FindPropertyRelative(nameof(TradId.DefaultText));
            defaultTextProp.stringValue = EditorGUILayout.TextField(defaultTextProp.displayName, defaultTextProp.stringValue);

            string previewText = _tradMgr.GetRawTranslations(newId, out var idIsUnknown);
            EditorGUILayout.HelpBox(previewText, idIsUnknown ? MessageType.Warning : MessageType.Info);
        }
        EditorGUI.indentLevel--;
    }
}
