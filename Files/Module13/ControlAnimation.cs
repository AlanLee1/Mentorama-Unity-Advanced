using UnityEngine;
using UnityEngine.InputSystem;

public class ControlAnimation : MonoBehaviour
{
    public Animator animator;
    public Transform target;
    private Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        SetOrientation();
        SetMovimentation();
    }

    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    private void SetOrientation()
    {
        if (move.x > 0)
        {
            target.Translate(-Vector3.forward * 2 * Time.deltaTime);
        } else if (move.x < 0)
        {
            target.Translate(Vector3.forward * 2 * Time.deltaTime);
        }
    }

    private void SetMovimentation()
    {
        if (!move.y.Equals(1))
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
        } else
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
        }
    }
}
