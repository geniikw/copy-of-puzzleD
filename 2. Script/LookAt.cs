using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class LookAt : MonoBehaviour 
{

    public Transform lookAt;
	
	// Update is called once per frame
	void Update () 
    {
//        transform.rotation = Quaternion.LookRotation(lookAt.position - transform.position);
	}
}
