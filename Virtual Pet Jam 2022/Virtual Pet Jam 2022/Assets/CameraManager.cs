using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public void ChangeCameraPosition(Vector3 pos) {
        transform.position = pos;
    }    
}
