using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uguiposition : MonoBehaviour
{

    public GameObject obj;
    // Use this for initialization
    void Start()
    {
        var rect = obj.GetComponent<RectTransform>();
        print("anchoredPosition: " + rect.anchoredPosition);
        print("localposition: " + rect.localPosition);
        print("anchoredPosition3D: " + rect.anchoredPosition3D);
        print("position: " + rect.position);
        //rect.position = Vector3.Lerp(rect.position,new Vector3(rect.position.x + 10, rect.position.y + 10, rect.position.z+100), 10);
        //print("position: " + rect.position);
    }
}