using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpinning : MonoBehaviour
{
    public float spinningRate;

    void Update()
    {
        Vector3 angle = new Vector3(0, 0, 1);
        transform.Rotate(angle * spinningRate * Time.deltaTime);        
    }
}
