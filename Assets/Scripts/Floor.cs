using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    
    void Start()
    {
		if(Mathf.Floor(transform.position.z / 10F) % 2 == 1){
			GameObject.FindGameObjectsWithTag("GameEngine")[0].GetComponent<GameEngine>().CreateChest(transform.position.z);
		}
    }

    void Update()
    {
        
    }
}
