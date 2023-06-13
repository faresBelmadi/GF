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
            new Container(-20, 20, 10, 10),
            new Container(0, 20, 10, 10),
            new Container(20, 20, 10, 10),
            new Container(40, 20, 10, 10),
            new Container(60, 20, 10, 10)
        };
        return temp;
    }
}
