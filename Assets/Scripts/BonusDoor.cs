using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusDoor : MonoBehaviour
{
    public int bonusIndex;
	private TextMeshPro doortext;
	GameObject _player;
	
    void Start()
    {
		_player = GameObject.FindGameObjectsWithTag("Player")[0];
		doortext = transform.GetChild(0).GetComponent<TextMeshPro>();
        bonusIndex = UnityEngine.Random.Range(1,4);
		switch(bonusIndex)
		{
			case 1:
			doortext.text = "<sprite index=12> Attack\n<b>+1</b>";
			break;
			case 2:
			doortext.text = "<sprite index=13> Range\n<b>+0.2</b>";
			break;
			case 3:
			doortext.text = "<sprite index=14> Speed\n<b>+10%</b>";
			break;
		}
    }
	void LateUpdate()
	{
		if(_player.transform.position.z - transform.position.z > 1F || _player.transform.position.z == 0){
			DestroyMe();
		}
	}
    public void DestroyMe()
	{
		Destroy(gameObject);
	}
}
