using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNextSpawnPos : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 angle;

    private Vector3 nextSpawnPos;
    private Quaternion nextSpawnRot;

    private void Start() {
        Vector3 realOffset = transform.forward * offset.z + transform.right * offset.x;
        nextSpawnPos = transform.position + realOffset;
        nextSpawnRot = Quaternion.Euler(angle) * transform.rotation;
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.CompareTag("Player")) {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.SpawnNewBlock(nextSpawnPos, nextSpawnRot);         
        }
    }
}
