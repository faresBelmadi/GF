using UnityEngine;
using UnityEditor;
using System.Collections;

//%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
//!
//! \\class	SynapseAssetInspector
//!
//! \\brief	inspector of a Synapse asset
//!
//%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
[CustomEditor(typeof(Synapse.Assets.AssetFile))]
public class SynapseAssetInspector : Editor
{
    ///////////////////////////////////////////////////////////////////////
    //
    //
    ///////////////////////////////////////////////////////////////////////
    public void OnEnable()
	{
		hideFlags = HideFlags.DontSave;
		EditorApplication.update += Update;
	}

    ///////////////////////////////////////////////////////////////////////
    //
    //
    ///////////////////////////////////////////////////////////////////////
    public void OnDisable()
	{
		EditorApplication.update -= Update;
	}

    ///////////////////////////////////////////////////////////////////////
    //
    //
    ///////////////////////////////////////////////////////////////////////
    public void Update()
	{
		Synapse.Editor.AssetInspector.Update();
	}

    ///////////////////////////////////////////////////////////////////////
    //
    //
    ///////////////////////////////////////////////////////////////////////
    public override void OnInspectorGUI()
	{	
		Synapse.Editor.AssetInspector.OnInspectorGUI(serializedObject);
	}
}
