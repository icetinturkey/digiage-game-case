using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Shop : MonoBehaviour
{
	public bool isShopOpen;
    public GameObject shopFrame;
	private float currentAnimation = 1F;
	RectTransform m_RectTransform;
	public Button[] buyButtons;
	GameEngine ge;
	
    void Start()
    {
        isShopOpen = false;
		m_RectTransform = shopFrame.transform.GetChild(0).GetComponent<RectTransform>();
		ge = GameObject.FindGameObjectsWithTag("GameEngine")[0].transform.GetComponent<GameEngine>();
		for(int a=0;a<buyButtons.Length;a++)
		{
			buyButtons[a].interactable = true;
			if(a == ge.getGun()) buyButtons[a].interactable = false;
		}
    }
	void LateUpdate()
	{
		if(currentAnimation<1F){
			currentAnimation += 0.05F;
			m_RectTransform.anchoredPosition = new Vector2(Mathf.Lerp(-750F, 0F, currentAnimation),0F);
		}
	}
    public void openShop()
	{
		shopFrame.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Your Money ; <b>"+ge.getMoney()+"</b> <sprite index=15>";
		shopFrame.SetActive(true);
		isShopOpen = true;
		currentAnimation = 0F;
	}
	public void closeShop()
	{
		m_RectTransform.anchoredPosition = new Vector2(-750F,0F);
		shopFrame.SetActive(false);
		isShopOpen = false;
	}
	public void buyWeapon(int _index)
	{
		int _price = 0;
		switch(_index)
		{
			case 1:_price=3000;break;
			case 2:_price=10000;break;
			case 3:_price=20000;break;
		}
		if(Int32.Parse(ge.getMoney())>=_price)
		{
			for(int a=0;a<buyButtons.Length;a++)
			{
				buyButtons[a].interactable = true;
				if(a == _index) buyButtons[a].interactable = false;
			}
			ge.buyWeapon(_index,_price);
			shopFrame.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Your Money ; <b>"+ge.getMoney()+"</b> <sprite index=15>";
		}
	}
}
