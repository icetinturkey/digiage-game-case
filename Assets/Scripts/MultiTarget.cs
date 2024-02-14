using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class MultiTarget : MonoBehaviour
{
    private int currentRot = 0;
	private int level = 0;
	private float currentAnimation = 1F;
	private float currentEdge = 1F;
	private bool moveToFinish;
	private Transform _player;
	public DamageNumber numberPrefab;
	bool normalizeRot = false;
	
    void Start()
    {
		moveToFinish = false;
        StartCoroutine(SetPlayer());
    }
	IEnumerator SetPlayer()
	{
		yield return new WaitForSeconds(0.1F);
		_player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        yield return null;
	}

    void Update()
    {
        if(currentAnimation<1F){
			currentAnimation += 0.1F;
			float newRotPos = Mathf.Lerp(currentRot, level * 60, currentAnimation);
			transform.rotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, newRotPos);
		}else if(!normalizeRot){
			normalizeRot = true;
			currentRot = level * 60;
		}
		
		if(currentEdge<1F){
			currentEdge += 0.01F;
			float newPos = Mathf.Lerp(Mathf.Abs(transform.position.x), 0.8F, currentEdge);
			transform.position = new Vector3(newPos * -1f, transform.position.y, transform.position.z);
		}else if(moveToFinish)
		{
			transform.position += Vector3.forward * Time.deltaTime * (_player.GetComponent<Player>().movespeed+6F);
		}
		
		if(!moveToFinish){
		if(Vector3.Distance(transform.position, GameObject.FindGameObjectsWithTag("Player")[0].transform.position) < 0.5F)
		{
			currentEdge = 0F;
			moveToFinish = true;
		}
		}
    }
	
	public void Hit()
	{
		if(level<6)
		{
			transform.GetChild(level).gameObject.SetActive(true);
			level += 1;
			currentAnimation = 0F;
			string damageText = string.Concat("+0.0",level.ToString("N0"));
			DamageNumber damageNumber = numberPrefab.Spawn(transform.position, damageText);
			normalizeRot = false;
			if(level == 6){
				currentEdge = 0F;
				moveToFinish = true;
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.transform.gameObject.tag == "MultiStage"){
			GameObject.FindGameObjectsWithTag("GameEngine")[0].GetComponent<GameEngine>().IncMultiplier(level);
			Destroy(gameObject);
		}
	}
}
