using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float movespeed = 0.25f;
	public float firerate = 1.0f;
	public float apdamage = 1.0f;
	public float aprange = 1.0f;
	Coroutine cFire;
	bool waitTimer = false;
	private Transform lastbullet;
	public Transform[] gunTransforms;
	private bool gamePaused = true;
	public TextMeshProUGUI txt_attack;
	public TextMeshProUGUI txt_range;
	public TextMeshProUGUI txt_speed;
	public Transform bubbleEffect;
	Transform effectToBeDelete;
	public AudioClip shotClip;
	AudioSource audioSource;
	
	void Start()
	{
		audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioSource.volume = 0.1F;
	}
    void Update()
    {
		if(!gamePaused)
		{
			if(!waitTimer){
				waitTimer=true;
				cFire = StartCoroutine(Fire());
			}
			transform.position += Vector3.forward * Time.deltaTime * movespeed;
		}
    }

	IEnumerator Fire()
	{
		transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed", (1F / firerate));
		yield return new WaitForSeconds(firerate);
		Transform bulletBlueprint = transform.GetChild(0).GetChild(0);
		Vector3 calculatedPosition = new Vector3(transform.position.x, bulletBlueprint.localPosition.y + 1F, transform.position.z + bulletBlueprint.localPosition.z);
		lastbullet = Instantiate(bulletBlueprint,calculatedPosition,Quaternion.Euler(bulletBlueprint.localRotation.eulerAngles));
		lastbullet.GetComponent<Bullet>().enabled = true;
		transform.GetChild(0).GetComponent<Animator>().SetBool("Shoot",true);
		audioSource.PlayOneShot(shotClip);

		txt_attack.text = "<sprite index=12> Damage : <b>"+apdamage.ToString("N1")+"</b>";
		txt_range.text = "<sprite index=13> Range : <b>"+aprange.ToString("N1")+"</b>";
		txt_speed.text = "<sprite index=14> Speed : <b>"+((1F/firerate)*10).ToString("N0")+"</b>";
		waitTimer=false;
        yield return null;
	}
	public void ChangeGun(int _index)
	{
		if(transform.childCount > 0){
			Destroy(transform.GetChild(0).gameObject);
		}
		Transform mygun = Instantiate(gunTransforms[_index], new Vector3(0, 0, 0), Quaternion.identity);
		mygun.parent = transform;
		if(cFire != null)
		{
			StopCoroutine(cFire);
			waitTimer=false;
		}
		transform.position = new Vector3(0,0,0);
		gamePaused = GameObject.FindGameObjectsWithTag("GameEngine")[0].transform.GetComponent<GameEngine>().gamePaused;
		switch(_index)
		{
			case 0:
			firerate = 1F;
			apdamage = 1F;
			aprange = 2F;
			break;
			case 1:
			firerate = 0.75F;
			apdamage = 2F;
			aprange = 2F;
			break;
			case 2:
			firerate = 1F;
			apdamage = 8F;
			aprange = 1F;
			break;
			case 3:
			firerate = 0.25F;
			apdamage = 2F;
			aprange = 3F;
			break;
		}
	}
	public void Stop()
	{
		gamePaused = GameObject.FindGameObjectsWithTag("GameEngine")[0].transform.GetComponent<GameEngine>().gamePaused;
		if(transform.childCount > 0){
			Destroy(transform.GetChild(0).gameObject);
		}
		if(cFire != null)
		{
			StopCoroutine(cFire);
			waitTimer=false;
		}
		transform.position = new Vector3(0,0,0);
		
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.transform.gameObject.tag == "Chest"){
			GameObject.FindGameObjectsWithTag("GameEngine")[0].GetComponent<GameEngine>().StopGame();
		}
		if(other.transform.gameObject.tag == "BonusDoor"){
			BonusDoor _bd = other.transform.GetComponent<BonusDoor>();
			switch(_bd.bonusIndex)
			{
				case 1:apdamage+=1F;break;
				case 2:aprange+=0.2F;break;
				case 3:
				if(firerate>0.2F) firerate*=0.9F;
				break;
			}
			_bd.DestroyMe();
			effectToBeDelete = Instantiate(bubbleEffect,new Vector3(other.transform.position.x,other.transform.position.y,other.transform.position.z-0.026F),Quaternion.identity);
			StartCoroutine(DeleteEffect());
		}
	}
	IEnumerator DeleteEffect()
	{
		GameObject _delete = effectToBeDelete.gameObject;
		yield return new WaitForSeconds(1F);
		Destroy(_delete);
        yield return null;
	}
}
