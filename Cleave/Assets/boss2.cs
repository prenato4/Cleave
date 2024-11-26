using System.Collections;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    public GameObject prefab;  // Referência ao prefab a ser instanciado
    public Transform spawnPoint;  // Ponto onde o prefab será instanciado
    public float initialDelay = 5f;  // Atraso antes de iniciar a instância (em segundos)
    public float spawnInterval = 15f;  // Intervalo de tempo entre instâncias (em segundos)

    private void Start()
    {
        // Inicia a coroutine que irá instanciar o prefab após o delay inicial
        StartCoroutine(SpawnPrefabCoroutine());
    }

    private IEnumerator SpawnPrefabCoroutine()
    {
        // Aguarda o tempo do delay inicial
        yield return new WaitForSeconds(initialDelay);

        // Agora começa a instanciar o prefab a cada 'spawnInterval' segundos
        while (true)
        {
            // Instancia o prefab no ponto especificado
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            // Espera o intervalo antes de instanciar novamente
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}