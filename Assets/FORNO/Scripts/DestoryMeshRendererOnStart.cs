using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryMeshRendererOnStart : MonoBehaviour
{
    void Start()
    {
        Destroy(GetComponent<MeshRenderer>());
    }
}
