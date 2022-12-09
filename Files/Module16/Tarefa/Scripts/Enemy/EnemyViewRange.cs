using UnityEngine;

public class EnemyViewRange : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemyController.enemyState == EnemyState.PATROL)
            {
                enemyController.SetTargetFound();
            }

            this.gameObject.SetActive(false);
        }
    }
}
