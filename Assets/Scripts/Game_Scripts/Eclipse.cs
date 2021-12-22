using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eclipse : MonoBehaviour
{
    public float eclipseTime = 30f;
    private float time = 0;

    void Update()
    {
        time += Time.deltaTime;
        if (time > eclipseTime) Destroy(gameObject);
    }
}
