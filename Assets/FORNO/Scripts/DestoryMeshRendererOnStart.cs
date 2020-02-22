using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Forno
{
    public class DestoryMeshRendererOnStart : MonoBehaviour
    {
        void Start()
        {
            Destroy(GetComponent<MeshRenderer>());
            // Remove itself
            Destroy(this);
        }
    }
}
