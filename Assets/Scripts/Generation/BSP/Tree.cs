using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tree
{
    public string Nom;

    public int Layer;

    [SerializeField]
    public Tree ParentNode;

    [SerializeField]
    public Tree LeftChild;

    [SerializeField]
    public Tree RighChild;

    [SerializeField]
    public Container Rect;

}
