using System.Collections;
using UnityEngine;

namespace MuhammetInce.HelperClass
{
    public static class HelperClass
    {
        public static Vector3 RandomizedVector3()
        {
            return new Vector3(Random.value, Random.value, Random.value);
        }

        public static IEnumerator ColliderToggle(this Collider targetObj, float delay)
        {
            targetObj.enabled = false;
            yield return new WaitForSeconds(delay);
            targetObj.enabled = true;
        }

        public static Vector3 UpAndLeftOrRight()
        {
            float value = Random.value;

            if (value >= 0.5f)
            {
                return Vector3.up + Vector3.right;
            }
            else
            {
                return Vector3.up + Vector3.left;
            }
        }

        public static Vector3 DownLeftOrRight()
        {
            float value = Random.value;

            if (value >= 0.5f)
            {
                return Vector3.down + Vector3.right;
            }
            else
            {
                return Vector3.down + Vector3.left;
            }
        }

        public static int GetRandomInteger(int min, int max)
        {
            return Random.Range(min, max);
        }
    }
}