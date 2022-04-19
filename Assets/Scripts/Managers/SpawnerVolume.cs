using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerVolume : MonoBehaviour
{
    BoxCollider spawnerVolumeCollider;
    // Start is called before the first frame update
    void Start()
    {
        spawnerVolumeCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPositionInBounds()
    {
        Bounds boxBounds = spawnerVolumeCollider.bounds;
        return new Vector3(Random.Range(boxBounds.min.x, boxBounds.max.x), transform.position.y, Random.Range(boxBounds.min.z,boxBounds.max.z));
    }
        
}
