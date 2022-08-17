using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BLINK
{
    public class AnimationDemo : MonoBehaviour
    {
        public enum AnimationType
        {
            Trigger,
            Bool
        }

        [System.Serializable]
        public class AnimationEntry
        {
            public string animationName;
            public AnimationType type;
        }

        public List<AnimationEntry> entries = new List<AnimationEntry>();

        public List<Animator> animators = new List<Animator>();

        public int entryIndex;
        public Text animationNameText;
        public Transform positionBeer;

        public float Speed = 2f;
        public float Rotate = 1f;
        public Transform visual;
        private Vector2 move;

        private void Start()
        {
            positionBeer = GetComponent<Transform>();
            visual = GameObject.FindWithTag("Visual").GetComponent<Transform>();
        }

        private void Update()
        {

            Vector3 movement = new Vector3(move.x, 0f, move.y);
            positionBeer.Translate(movement * 2 * Time.deltaTime);

            if (movement.magnitude > 0)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement, Vector3.up);
                visual.rotation = Quaternion.RotateTowards(visual.rotation, newRotation, Rotate);
            }

            Move();
            PlayAnimation();
        }

        private void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        private void Move()
        {
            if (move.x != 0 || move.y != 0)
            {
                entryIndex = 5;
            } else
            {
                entryIndex = 1;
            }
        }

        private void ResetAllBool()
        {
            foreach (var entry in entries)
            {
                if (entry.type != AnimationType.Bool) continue;
                foreach (var animator in animators)
                {
                    animator.SetBool(entry.animationName, false);
                }
            }
        }

        private void PlayAnimation()
        {
            ResetAllBool();

            if (entries[entryIndex].type == AnimationType.Bool)
            {
                foreach (var animator in animators)
                {
                    animator.SetBool(entries[entryIndex].animationName, true);
                }
            } else
            {
                foreach (var animator in animators)
                {
                    animator.SetTrigger(entries[entryIndex].animationName);
                }
            }
        }
    }
}
