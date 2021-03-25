using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private int count = 0;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
       if ( other.name.Contains("Token") ) {
            count++;
        }
    }

    // Update is called once per frame
    void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Token")) {
            count--;
        }
    }

    public int GetCount() {
        return count;
    }
}
