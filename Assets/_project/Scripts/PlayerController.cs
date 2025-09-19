using System;
using UnityEngine;

namespace CameraSystem._project.Scripts
{
    /// <summary>
    /// The class of our mighty and totally accurate car controller (Totally Not A Spaceship)
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 50f)]
        private float _speed;
        
        [SerializeField, Range(0.1f, 50f)]
        private float _angularSpeed;
        private Quaternion _startingRotation;

        private void Awake()
        {
            _startingRotation = transform.rotation;
        }

        public void Update()
        {
            // if you have any questions regarding why we use up to go forward and forward to turn left/right
            // it's just the obj has its axis baked all wrong, so I had to make do with what do I have.
            if (Input.GetKey(KeyCode.A))
            {
                transform.rotation *= Quaternion.AngleAxis(-90 * _angularSpeed * Time.deltaTime, Vector3.forward);
            } 
            if (Input.GetKey(KeyCode.D))
            {
                transform.rotation *= Quaternion.AngleAxis(90 * _angularSpeed * Time.deltaTime, Vector3.forward);
            } 
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += -transform.up * (_speed * Time.deltaTime);
            } 
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += transform.up * (_speed * Time.deltaTime);
            }
            

            if (Input.GetKey(KeyCode.Space))
            {
                transform.rotation = _startingRotation;
            }
        }
    }
}