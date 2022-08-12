using UnityEngine;
using UnityEngine.AI;

public class EnemyTarget : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navMeshAgent.SetDestination(_player.GetComponent<Transform>().position);
    }
}
