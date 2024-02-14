using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnd : MonoBehaviour
{

	
	public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
			GetComponent<Animator>().SetBool("Shoot",false);
        }
    }
}
