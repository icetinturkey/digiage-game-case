using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controls : MonoBehaviour
{
	private float savedX;
	private float editormove = 0F;
	float playerPosX;
	float _bufferX;
	float nihaisonuc;
	
    void Start()
    {
		playerPosX = 0F;
		_bufferX = 0F;
    }


    void Update()
    {
        if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				savedX = touch.position.x;
				_bufferX = 0F;
			}else if(touch.phase == TouchPhase.Moved){
				float _diff = touch.position.x - savedX;
				float bolum = (Screen.width/4f) / 100f; //1.8
				nihaisonuc = Mathf.Clamp(Mathf.Abs(_diff) / bolum,0f,200f) * 0.00375f;

				if(_diff>0){
					//transform.position = new Vector3(nihaisonuc, transform.position.y, transform.position.z);
					_bufferX = nihaisonuc;
				}else{
					//transform.position = new Vector3(nihaisonuc*-1, transform.position.y, transform.position.z);
					_bufferX = nihaisonuc * -1F;
				}

			}else if(touch.phase == TouchPhase.Ended){
				playerPosX = Mathf.Clamp(playerPosX + _bufferX, -0.375F, 0.375F);
				_bufferX = 0F;
			}
			
		}
		
		#if UNITY_EDITOR
		editormove += Input.GetAxis("Mouse X") * 4F;
		editormove = Mathf.Clamp(editormove,-100f,100f);
		transform.position = new Vector3(editormove * 0.00375f, transform.position.y, transform.position.z);
		#else
		transform.position = new Vector3(Mathf.Clamp(playerPosX + _bufferX, -0.375F, 0.375F), transform.position.y, transform.position.z);	
		#endif
    }
	
	/*void OnGUI()
    {
        GUI.Label(new Rect(20, 200, 200, 40), "playerposx="+playerPosX.ToString("N3"));
		GUI.Label(new Rect(20, 240, 200, 40), "bufferX="+_bufferX.ToString("N3"));
    }*/

}
