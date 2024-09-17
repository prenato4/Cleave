using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
   public GameObject particlePrefab;

   private void OnEnable()
   {
      ParticleObserver.ParticleSpawnEvent += SpawnParticles;
   }


   private void OnDisable()
   {
      ParticleObserver.ParticleSpawnEvent -= SpawnParticles;
   }

   public void SpawnParticles(Vector3 posicao)
   {
      Instantiate(particlePrefab, posicao, Quaternion.identity);
      
   }
}
