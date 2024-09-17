using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleObserver 
{
    public static event Action<Vector3> ParticleSpawnEvent;


    public static void onParticleSpawn(Vector3 pos)
    {
        ParticleSpawnEvent?.Invoke(pos);
    }
}
