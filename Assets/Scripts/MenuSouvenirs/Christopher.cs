using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Christopher : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _freeSlots;
    [SerializeField]
    private GameObject _prefab;

    private List<SouvenirSlot> _slots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            Instantiate(_prefab, _freeSlots[0]);
        }
    }
}
