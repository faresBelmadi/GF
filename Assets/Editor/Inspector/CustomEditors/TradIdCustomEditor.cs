using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

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

            DrawDefaultText(property);

            string previewText = _tradMgr.GetRawTranslations(newId, out var idIsUnknown);
            EditorGUILayout.HelpBox(previewText, idIsUnknown ? MessageType.Warning : MessageType.Info);
        }
        EditorGUI.indentLevel--;
    }

    private void DrawDefaultText(SerializedProperty property)
    {
        var defaultTextStyleAttributes = fieldInfo.GetCustomAttributes<DefaultTextStyleAttribute>(true);
        var defaultTextStyle = defaultTextStyleAttributes.FirstOrDefault()?.Style ?? TradIdDefaultTextStyles.TextField;

        SerializedProperty defaultTextProp = property.FindPropertyRelative(nameof(TradId.DefaultText));

        if (defaultTextStyle == TradIdDefaultTextStyles.TextArea)
        {
            EditorGUILayout.LabelField(defaultTextProp.displayName);
            defaultTextProp.stringValue = EditorGUILayout.TextArea(defaultTextProp.stringValue, EditorStyles.textArea, GUILayout.MinHeight(50));
        }
        else
        {
            defaultTextProp.stringValue = EditorGUILayout.TextField(defaultTextProp.displayName, defaultTextProp.stringValue);
        }
    }
}
