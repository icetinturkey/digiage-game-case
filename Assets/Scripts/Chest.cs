using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DamageNumbersPro;

public class Chest : MonoBehaviour
{
    private TextMeshPro healthText;
	private TextMeshPro moneyText;
	Animator _animator;
	GameObject _player;
	private float chestHealth;
	private int chestMoney;
	public DamageNumber numberPrefab;
	
    void Start()
    {
		_player = GameObject.FindGameObjectsWithTag("Player")[0];
		_animator = GetComponent<Animator>();
        healthText = transform.GetChild(0).GetComponent<TextMeshPro>();
		moneyText = transform.GetChild(1).GetComponent<TextMeshPro>();
		StartCoroutine(SetChest());
    }
	
	IEnumerator SetChest()
	{
		yield return new WaitForSeconds(0.2F);
		chestHealth = Mathf.Ceil(_player.transform.position.z) - 11F;
		chestMoney = UnityEngine.Random.Range(1,101);
		healthText.text = chestHealth.ToString("N0");
		moneyText.text = chestMoney.ToString();
        yield return null;
	}

    public void Hit()
	{
		chestHealth -= _player.GetComponent<Player>().apdamage;
		healthText.text = chestHealth.ToString("N0");
		string damageText = string.Concat("-",_player.GetComponent<Player>().apdamage.ToString("N1"));
		DamageNumber damageNumber = numberPrefab.Spawn(transform.position, damageText);
		if(chestHealth<=0F)
		{
			Destroy (GetComponent<Rigidbody>());
			_animator.SetBool("isOpen",true);
		}
	}
	public void AnimationEnd()
	{
		GameObject.FindGameObjectsWithTag("GameEngine")[0].GetComponent<GameEngine>().IncMoney(chestMoney);
		Destroy(gameObject);
	}
	void LateUpdate()
	{
		if(_player.transform.position.z - transform.position.z > 1F || _player.transform.position.z == 0){
			Destroy(gameObject);
		}
	}
}
