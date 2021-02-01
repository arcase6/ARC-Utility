using System.Collections;
using System.Collections.Generic;
using ARC.SerializationToolkit;
using SerializationToolkit;
using UnityEngine;

public class MixedListUser : MonoBehaviour
{
    [SerializeField] private MixedList<ITestListMember, Member1, Member2> m_MixedList;
}


public interface ITestListMember
{
    void InterfaceMethod();
}

[System.Serializable]
public class Member1 : ITestListMember
{
    public int data1;
    public string data2;
    
    public void InterfaceMethod() => Debug.Log("Member1 called");
}

[System.Serializable]

public class Member2 : ITestListMember
{
    public int data2;
    
    [RList("Nested : ")]
    public ListWrapper<int> RList = new ListWrapper<int>();
    public void InterfaceMethod() => Debug.Log("Member2 called");
}
