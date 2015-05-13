using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour
{
    public GameObject DefaultEquipmentPrefab;

    public GameObject Equipment;

    void Awake()
    {
        if (DefaultEquipmentPrefab == null)
        {
            DefaultEquipmentPrefab = Resources.Load<GameObject>("Fist");
        } 
    }

    void Update()
    {
        if (Equipment == null)
        {
            Equipment = (Instantiate(DefaultEquipmentPrefab) as GameObject);
            Equipment.transform.SetParent(transform, false);
        }

    }
}
