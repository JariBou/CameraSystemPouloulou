using System;
using UnityEngine;

namespace CameraSystem._project.Scripts
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 50f)]
        private float _speed;
        
        [SerializeField, Range(0.1f, 50f)]
        private float _angularSpeed;
        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                // transform.position += Vector3.left * (_speed * Time.deltaTime);
                transform.rotation *= Quaternion.AngleAxis(-90 * _angularSpeed * Time.deltaTime, Vector3.forward);
            } 
            if (Input.GetKey(KeyCode.RightArrow))
            {
                // transform.position += Vector3.right * (_speed * Time.deltaTime);
                transform.rotation *= Quaternion.AngleAxis(90 * _angularSpeed * Time.deltaTime, Vector3.forward);
            } 
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += -transform.up * (_speed * Time.deltaTime);
            } 
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += transform.up * (_speed * Time.deltaTime);
            }
        }
    }
}