using System.Collections;
using UnityEngine;

public class Clotho : MonoBehaviour
{
    public int maxHealth = 100;  // Vida máxima do boss
    public int currentHealth;

    public float gravityChangeInterval = 20f; // Intervalo de mudança de gravidade
    public float gravityMultiplier = 0.5f; // Fator de multiplicação da gravidade
    private bool gravityChanged = false; // Flag para verificar se a gravidade foi alterada

    public GameObject objectToActivate; // Objeto que será ativado
    public float activationInterval = 30f; // Intervalo de ativação do objeto

    public GameObject projectilePrefab; // Prefab do projétil
    public Transform projectileSpawnPoint; // Ponto de spawn do projétil
    public float shootingInterval = 5f; // Intervalo de tiros

    private Transform playerTransform; // Transform do player

    void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(ChangeGravityPeriodically());
        StartCoroutine(ActivateObjectPeriodically());
        StartCoroutine(ShootAtPlayerPeriodically());

        // Encontrar o transform do player
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Aqui você pode adicionar outras atualizações do Clotho
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Aqui você pode adicionar animações de morte, efeitos sonoros, etc.
        Destroy(gameObject); // Destroi o objeto do boss
    }

    private IEnumerator ChangeGravityPeriodically()
    {
        while (true)
        {
            // Manipula a gravidade ao redor do boss
            ManipulateGravity();
            
            // Espera pelo próximo intervalo
            yield return new WaitForSeconds(gravityChangeInterval);
        }
    }

    private void ManipulateGravity()
    {
        // Verifica se a gravidade não foi alterada recentemente
        if (!gravityChanged)
        {
            // Alterar a gravidade global do Unity
            Physics2D.gravity *= gravityMultiplier;
            gravityChanged = true;
            
            // Definir um temporizador para reverter a alteração da gravidade
            StartCoroutine(RevertGravityAfterDelay());
        }
    }

    private IEnumerator RevertGravityAfterDelay()
    {
        // Espera um tempo antes de reverter a gravidade
        yield return new WaitForSeconds(gravityChangeInterval);
        
        // Reverter a gravidade global ao seu valor original
        Physics2D.gravity /= gravityMultiplier;
        gravityChanged = false;
    }

    private IEnumerator ActivateObjectPeriodically()
    {
        while (true)
        {
            // Ativa o objeto
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                // Opcional: Desativar o objeto após um curto período
                yield return new WaitForSeconds(5f); // Ajuste o tempo conforme necessário
                objectToActivate.SetActive(false);
            }
            
            // Espera pelo próximo intervalo
            yield return new WaitForSeconds(activationInterval);
        }
    }

    private IEnumerator ShootAtPlayerPeriodically()
    {
        while (true)
        {
            // Atira no player
            ShootAtPlayer();
            
            // Espera pelo próximo intervalo
            yield return new WaitForSeconds(shootingInterval);
        }
    }

    private void ShootAtPlayer()
    {
        if (projectilePrefab != null && playerTransform != null && projectileSpawnPoint != null)
        {
            // Instanciar o projétil no ponto de spawn
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            // Calcular a direção para o player
            Vector2 direction = (playerTransform.position - projectileSpawnPoint.position).normalized;

            // Inicializa o projétil com a direção calculada
            Feixe feixeScript = projectile.GetComponent<Feixe>();
            if (feixeScript != null)
            {
                feixeScript.Initialize(direction);
            }
        }
    }

}
