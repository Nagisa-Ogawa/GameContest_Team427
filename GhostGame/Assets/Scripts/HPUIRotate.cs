using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
