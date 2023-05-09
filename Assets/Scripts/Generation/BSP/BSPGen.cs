using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BSPGen : MonoBehaviour
{
    public List<Container> Generate() 
    {
        return GetContainer();
    }

    List<Container> GetContainer()
    {
        List<Container> temp = new List<Container>()
        {
            new Container(-15, 20, 10, 10),
            new Container(10, 20, 10, 10),
            new Container(30, 20, 10, 10),
            new Container(55, 20, 10, 10)
        };
        return temp;
    }
}
