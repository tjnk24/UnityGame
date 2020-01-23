using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingComponent : MonoBehaviour
{
    [HideInInspector]
    public PoolingObject poolingObject;

    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    public void ReturnToPool()
    {
        poolingObject.ReturnToPool();
    }

    private void Update()
    {
        if (particle && particle.isStopped)
        {
            ReturnToPool();
        }
    }
}
