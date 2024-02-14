using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject _tPlayer;
	private Player _player;
	
    void Start()
    {
        _tPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
		_player = _tPlayer.GetComponent<Player>();
    }

    void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime * (_player.movespeed+1f);
		if(Vector3.Distance(transform.position, _tPlayer.transform.position) > _player.aprange)
		{
			Destroy(gameObject);
		}
    }
	
	void OnTriggerEnter(Collider other)
	{
		if(other.transform.gameObject.tag == "MultiTarget"){
			MultiTarget _mt = other.transform.GetComponent<MultiTarget>();
			_mt.Hit();
			Destroy(gameObject);
		}
		if(other.transform.gameObject.tag == "Chest"){
			Chest _chest = other.transform.GetComponent<Chest>();
			_chest.Hit();
			Destroy(gameObject);
		}
		if(other.transform.gameObject.tag == "BonusDoor"){
			Destroy(gameObject);
		}
	}
}
