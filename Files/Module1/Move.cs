using UnityEngine;

public class Move : MonoBehaviour
{

    public float _moveSpeed;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (_anim != null)
        {
            _anim.SetFloat("right", 0.0f);
            _anim.SetFloat("left", 0.0f);
            _anim.SetFloat("top", 0.0f);
            _anim.SetFloat("bot", 0.0f);
            _anim.SetFloat("idle", 0.1f);

            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * _moveSpeed * Time.deltaTime;
                _anim.SetFloat("right", 0.1f);
                _anim.SetFloat("idle", 0.0f);
            } else if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
                _anim.SetFloat("left", 0.1f);
                _anim.SetFloat("idle", 0.0f);
            } else if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
                _anim.SetFloat("top", 0.1f);
                _anim.SetFloat("idle", 0.0f);
            } else if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * _moveSpeed * Time.deltaTime;
                _anim.SetFloat("bot", 0.1f);
                _anim.SetFloat("idle", 0.0f);
            }
        }

    }
}
