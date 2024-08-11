using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostParticleTestScript : MonoBehaviour
{
    [SerializeField] private Camera battleCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = battleCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 100f;
        transform.position = pos;
    }
}
