using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private Vector3 rotor = new Vector3(0,0,-7f);

    void Update()
    {
        transform.eulerAngles += rotor;
    }
}
