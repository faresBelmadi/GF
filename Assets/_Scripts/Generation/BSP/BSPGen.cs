using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BSPGen : MonoBehaviour
{
    [Header("Settings BSP")]
    [Range(1,15)]
    public int MaxIteration;
    public bool DISCARD_BY_RATIO;

    [Range(0f,1f)]
    public float W_RATIO;
    [Range(0f,1f)]
    public float H_RATIO;

    [Header("BSP Result")]
    [SerializeField]
    List<Tree> BSPTree = new List<Tree>();

    [Header("Settings Map")]
    public Vector2 MinMaxRooms;

    public List<Container> Generate() 
    {
        InitTree();
        
        SplitTree();
       
        ClearTree();

        return GetContainer();
    }


    void InitTree()
    {
        BSPTree = new List<Tree>();
        Random.InitState((int)System.DateTime.Now.Ticks); 
        BSPTree.Add(new Tree(){
            Nom = "root",
            Rect = new Container(0,0,50,50),
            Layer = 0         
        });
    }

    void SplitTree()
    { 
        for (int i = 0; i < MaxIteration; i++)
        {
            var temp = new List<Tree>();
            foreach (var item in BSPTree.Where(c => c.Layer == i))
            {
                temp.AddRange(SplitParent(item,i+1));
            }
            BSPTree.AddRange(temp);
        }
    }

    List<Tree> SplitParent(Tree parent, int layer)
    {
        var t = SplitContainer(parent.Rect);
        var Left = new Tree(){
            Rect = t[0],
            Layer = layer,
            ParentNode = parent
        };
        var Right = new Tree(){
            Rect = t[1],
            Layer = layer,
            ParentNode = parent
        };
        parent.LeftChild = Left;
        parent.RighChild = Right;

        return new List<Tree>(){Left,Right};
    }

    List<Container> SplitContainer(Container ToCut)
    {
        var Side = Random.Range(0,2);
        Container SubLeft,SubRight;
        bool CutV = Side == 0 ? true : false;

        if(ToCut.w > ToCut.h && ToCut.w/ToCut.h > 1.25f)
            CutV = true;
        else if(ToCut.w < ToCut.h && ToCut.h/ToCut.w > 1.25f)
            CutV = false;

        if(CutV)
        {
            SubLeft = new Container(ToCut.x, ToCut.y,ToCut.h,Random.Range(1f,ToCut.w));
            SubRight = new Container(ToCut.x+SubLeft.w, ToCut.y,ToCut.h, ToCut.w-SubLeft.w);
            
            if (DISCARD_BY_RATIO) 
            {
                var SubLeft_w_ratio = SubLeft.w / SubLeft.h;
                var SubRight_w_ratio = SubRight.w / SubRight.h;
                if (SubLeft_w_ratio < W_RATIO || SubRight_w_ratio < W_RATIO) 
                    return SplitContainer(ToCut);
            }
        }
        else
        {
            SubLeft = new Container(ToCut.x, ToCut.y,Random.Range(1f,ToCut.h),ToCut.w);
            SubRight = new Container(ToCut.x, ToCut.y + SubLeft.h,ToCut.h-SubLeft.h, ToCut.w); 
            
            if (DISCARD_BY_RATIO) 
            {
                var SubLeft_h_ratio = SubLeft.h / SubLeft.w;
                var SubRight_h_ratio = SubRight.h / SubRight.w;
                if (SubLeft_h_ratio < H_RATIO || SubRight_h_ratio < H_RATIO) 
                    return SplitContainer(ToCut);
            }
        }

        return new List<Container>(){SubLeft,SubRight};
    }

    List<Container> GetContainer()
    {
        List<Container> temp = new List<Container>();

        foreach (var item in BSPTree)
        {
            temp.Add(item.Rect);
        }

        return temp;
    }

    void ClearTree()
    {
        var MaxRooms = Random.Range(MinMaxRooms.x,MinMaxRooms.y);
        var toDelete = BSPTree.Where(c => c.Layer != MaxIteration).ToList();

        foreach (var item in toDelete)
        {
            BSPTree.Remove(item);
        }

        do
        {
            BSPTree.RemoveAt(Random.Range(0,BSPTree.Count));
        } while (BSPTree.Count > MaxRooms);
    }

}
