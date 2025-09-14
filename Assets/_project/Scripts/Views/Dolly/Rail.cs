using CameraSystem._project.Scripts;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace CameraSystem
{
    public class Rail : MonoBehaviour
    {
        public bool isLoop;
        public float length;
        private List<Transform> children = new List<Transform>();

        public float TempDistance = 0.0f;
        public Vector3 TempSavedPos = Vector3.zero;
        private void OnValidate()
        {
            GetAllChildren();

        }
        private void Start()
        {
            length = GetTotalLength();
        }

        private void Update()
        {
           TempSavedPos = GetPosition(TempDistance);
        }
        private void GetAllChildren()
        {
            children.Clear();
            if (this.transform.childCount > 0)
            {
                foreach (Transform child in this.transform)
                {
                    //Debug.Log("Adding a child: " + child.name);
                    children.Add(child); 
                }
            }
        }

        private float GetTotalLength()
        {
            float temp = 0.0f;
            Vector3 savedPos = children.First().position;

            foreach (Transform item in children)
            {
               temp += Vector3.Distance(savedPos, item.position);
                savedPos = item.position;
            }
            if(isLoop)
            {
                temp += Vector3.Distance(savedPos, children.First().position);
            }
            return temp;
        }

        private void OnDrawGizmos()
        {
            DrawRail(Color.magenta, Color.green);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(TempSavedPos, 0.25f);
        }
        private void DrawRail(Color colorNode, Color colorLine)
        {
            Vector3 savedPosition = Vector3.zero;
            foreach (Transform item in children)
            {
                Gizmos.color = colorNode;
                Gizmos.DrawSphere(item.position, 0.25f);
                if(savedPosition != Vector3.zero)
                {
                    Gizmos.color = colorLine;
                    Gizmos.DrawLine(savedPosition, item.position);
                }
                savedPosition = item.transform.position;
            }
            if(isLoop)
            {
                Gizmos.color = colorLine;
                Gizmos.DrawLine(savedPosition, children.First().position);
            }
            savedPosition = Vector3.zero;
        }

        public float GetLength()
        {
            return length;
        }

        public Vector3 GetNearestPoint(Vector3 targetPos)
        {
            Vector3 savedPos = Vector3.zero;
            float cachedDistance = float.PositiveInfinity;

            if (children.Count < 2)
            {
                throw new System.Exception("Not enough points in rail");
            }

            for (int i = 0; i < children.Count - (isLoop ? 0 : 1); i++)
            {
                float newDistance = 0;
                // calculer pos
                Vector3 newPos = MathUtils.GetNearestPointOnSegment(children[i].position, 
                    children[(i + 1)%children.Count].position, 
                    targetPos);
                // calculer distance
                newDistance = (newPos - targetPos).sqrMagnitude;
                if (newDistance < cachedDistance)
                {
                    cachedDistance = newDistance;
                    savedPos = newPos;
                }
            }
            return savedPos;
        }

        public Vector3 GetPosition(float distance)
        {
            Vector3 railPos = Vector3.zero;
            int iterator = 0;
            float tempDistance = 0;

            if (isLoop)
            {
                while (distance > length)
                {
                    distance -= length;
                }
            }
            // start from first child
            while (distance > 0)
            {
                Debug.Log(iterator);
                if (iterator >= children.Count - 1)
                {
                    if (!isLoop) 
                    { 
                        return children[iterator].position;
                    } else
                    {
                        Vector3 start = children[iterator].position;
                        Vector3 end = children.First().position;

                        // Get point at specific percentage along the segment (0.0 to 1.0)
                        float t = distance / Vector3.Distance(start, end);
                        railPos = Vector3.Lerp(start, end, t);
                        return railPos;
                    }
                }
                tempDistance = Vector3.Distance(children[iterator].position, children[iterator + 1].position);
                // check if InDistance is smaller than distance between first and second point
                if (distance <= tempDistance)
                {
                    // if true, find then return point on the segment 
                    Vector3 AB = children[iterator + 1].position - children[iterator].position;
                    Vector3 D = AB.normalized;
                    railPos = children[iterator].position + D * distance;
                    return railPos;
                }
                else
                {
                    // if not, go to next point and subtract the distance between the points from the  InDistance
                    iterator += 1;
                    distance -= tempDistance;
                }
            }
            // distance = 0, return last point
            return children[iterator].position;
        }
    }
}
