using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public List<Rigidbody> rigidbodies;
    public float force;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddForce((transform.position - rigidbody.transform.position) * (force * Time.deltaTime));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            rigidbodies.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            rigidbodies.Remove(other.gameObject.GetComponent<Rigidbody>());
        }
    }
}
