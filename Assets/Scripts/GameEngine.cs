using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameEngine : MonoBehaviour
{
	[System.Serializable]
    public struct MyData
    {
        public int Money;
        public int Gun;
		public MyData(int _money, int _gun)
		{
			this.Money = _money;
			this.Gun = _gun;
		}
    };
	
	public bool gamePaused = true;
	MyData savedata;
	float ingameMoney = 0F;
	float ingameMultiplier = 1F;
	public GameObject menuStartButton;
	public GameObject menuStopButton;
	public GameObject menuShopButton;
	public GameObject menuBanner;
	public GameObject menuIngame;
	public bool multiplierStarted = false;
	public Transform multiplierStagePrefab;
	public Transform multiplierModelPrefab;
	Transform lastMultiplierStage;
	public Transform chestPrefab;
	public Transform bonusDoorPrefab;
	public GameObject exitPanel;
	bool isEPOpen = false;
	
    public GameObject platform;
	private List<GameObject> CreatedPlatforms = new List<GameObject>();
	private int nextZIndex;
	Coroutine Timer_1000;
	bool waitTimer = false;
	private GameObject _player;
	
	void Awake()
	{
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 60;
	}
    void Start()
    {
		savedata = LoadGame();
		_player = GameObject.FindGameObjectsWithTag("Player")[0];
		SaveGame();
    }
	MyData LoadGame()
	{
		string path = System.IO.Path.Combine(Application.persistentDataPath, "save.dat");
		if(File.Exists(path)){
			FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryFormatter bin = new BinaryFormatter();
			MyData target = (MyData) bin.Deserialize(reader); 
			reader.Close();
			return target;
		}else{
			MyData newsave = new MyData(9999, 0);
			return newsave;
		}
	}
	public void SaveGame()
	{
		try
        {
			string path = System.IO.Path.Combine(Application.persistentDataPath, "save.dat");
            BinaryFormatter bin = new BinaryFormatter();
            FileStream writer = new FileStream(path,FileMode.Create);
            bin.Serialize(writer, (object)savedata);
            writer.Close();
        }
        catch (IOException) {}
	}
	void Update()
    {
		if(!waitTimer && !gamePaused){
			waitTimer=true;
			Timer_1000 = StartCoroutine(TimerFunc());
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if(!isEPOpen){
				isEPOpen=true;
				exitPanel.SetActive(true);
			}
        }
    }
	IEnumerator TimerFunc() {
		menuIngame.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ingameMoney.ToString("N0")+" <b>x "+ingameMultiplier.ToString("N2")+"</b> <sprite index=15>";
		if(_player.GetComponent<Player>().movespeed<2F) _player.GetComponent<Player>().movespeed += 0.0005F;
		yield return new WaitForSeconds(0.5f);
		if(_player.transform.position.z + 8f > (float)nextZIndex)
		{
			nextZIndex += 1;
			if(nextZIndex % 10 == 0)
			{
				if(nextZIndex % 20 == 0){
					if(multiplierStarted)
					{
						multiplierStarted=false;
					}
				}else{
					if(!multiplierStarted)
					{
						multiplierStarted=true;
						if(lastMultiplierStage != null) Destroy(lastMultiplierStage.gameObject);
						lastMultiplierStage = Instantiate(multiplierStagePrefab,new Vector3(-0.8f,-0.2f,(float)nextZIndex + 4.5F),Quaternion.Euler(0,180,0));
					}
				}
			}
			GameObject _insbuffer = Instantiate(platform,new Vector3(0f,0f,(float)nextZIndex),Quaternion.identity);
			CreatedPlatforms.Add(_insbuffer);
			Destroy(CreatedPlatforms[nextZIndex-10]);
			if(multiplierStarted){
				int _rastgele = UnityEngine.Random.Range(1,101);
				if(_rastgele>25){
					float _xpos = 0F;
					switch(UnityEngine.Random.Range(1,5))
					{
						case 1:_xpos = -0.375F;break;
						case 2:_xpos = -0.125F;break;
						case 3:_xpos = 0.125F;break;
						case 4:_xpos = 0.375F;break;
					}
					if(_rastgele>50)
						Instantiate(bonusDoorPrefab,new Vector3(_xpos,0f,(float)nextZIndex),Quaternion.identity);
					else
						Instantiate(multiplierModelPrefab,new Vector3(_xpos,0f,(float)nextZIndex),Quaternion.identity);
				}
			}else{
				if(UnityEngine.Random.Range(1,101)>50){
					Instantiate(chestPrefab,new Vector3(-0.375f,0f,(float)nextZIndex),Quaternion.identity);
				}
				if(UnityEngine.Random.Range(1,101)>50){
					Instantiate(chestPrefab,new Vector3(-0.125f,0f,(float)nextZIndex),Quaternion.identity);
				}
				if(UnityEngine.Random.Range(1,101)>50){
					Instantiate(chestPrefab,new Vector3(0.125f,0f,(float)nextZIndex),Quaternion.identity);
				}
				if(UnityEngine.Random.Range(1,101)>50){
					Instantiate(chestPrefab,new Vector3(0.375f,0f,(float)nextZIndex),Quaternion.identity);
				}
			}
		}
		
		waitTimer=false;
        yield return null;
	}

    
	
	

	
	public void StartGame()
	{
		ingameMoney = 0F;
		ingameMultiplier = 1F;
		gamePaused = false;
		menuStartButton.SetActive(false);
		menuStopButton.SetActive(true);
		menuShopButton.SetActive(false);
		menuBanner.SetActive(false);
		menuIngame.SetActive(true);
		multiplierStarted = false;
		nextZIndex = 9;
		for(int i=0;i<=9;i++)
		{
			GameObject _insbuffer = Instantiate(platform,new Vector3(0f,0f,(float)i),Quaternion.identity);
			CreatedPlatforms.Add(_insbuffer);
		}
		_player.GetComponent<Player>().ChangeGun(savedata.Gun);
		_player.GetComponent<Player>().movespeed = 0.25F;
		#if UNITY_EDITOR
		_player.GetComponent<Player>().movespeed = 0.75F;
		#endif
	}
	public void StopGame()
	{
		gamePaused = true;
		_player.GetComponent<Player>().Stop();
		waitTimer = false;
		menuStartButton.SetActive(true);
		menuStopButton.SetActive(false);
		menuShopButton.SetActive(true);
		menuBanner.SetActive(true);
		menuIngame.SetActive(false);
		
		savedata.Money += (int)(ingameMoney*ingameMultiplier);
		if(Timer_1000 != null) StopCoroutine(Timer_1000);
		foreach(GameObject _p in CreatedPlatforms)
		{
			Destroy(_p);
		}
		CreatedPlatforms.Clear();
		if(lastMultiplierStage != null) Destroy(lastMultiplierStage.gameObject);
		GameObject[] multitargets = GameObject.FindGameObjectsWithTag("MultiTarget");
		foreach(GameObject _m in multitargets)
		{
			Destroy(_m);
		}
		GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
		foreach(GameObject _c in chests)
		{
			Destroy(_c);
		}
		GameObject[] doors = GameObject.FindGameObjectsWithTag("BonusDoor");
		foreach(GameObject _d in doors)
		{
			Destroy(_d);
		}
		SaveGame();
	}
	public void CreateChest(float posz)
	{
	}
	public void IncMultiplier(int _val)
	{
		ingameMultiplier += 0.01F * _val;
	}
	public void IncMoney(int _val)
	{
		ingameMoney += _val;
	}
	public string getMoney()
	{
		return savedata.Money.ToString();
	}
	public int getGun()
	{
		return savedata.Gun;
	}
	public void buyWeapon(int _gun, int _price)
	{
		savedata.Gun = _gun;
		savedata.Money -= _price;
		SaveGame();
	}
	public void ApplicationEnd()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	public void ExitCancel()
	{
		exitPanel.SetActive(false);
		isEPOpen=false;
	}
}
