using System.Collections;
using System.Collections.Generic;
using ARC.SerializationToolkit;
using SerializationToolkit;
using UnityEngine;

public class RListUser : MonoBehaviour
{
    [RList("Prefix Here: ")] [SerializeField]
    private ListWrapper<int> m_List = null;
}
