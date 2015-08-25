using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class M_ManagerScript : MonoBehaviour {

	Sprite [] foodSprites;
	public GameObject prefab;
	public int foodNum = 15;
	private RectTransform panelRT;
	private string[] orderedName;
	private GameObject[] cells;

	void Awake()
	{
		foodSprites = new Sprite[foodNum];
		orderedName = new string[foodNum];
		panelRT = GameObject.FindWithTag ("Panel").GetComponent<RectTransform>() ;
		cells = new GameObject[foodNum];
	}

	void Start () 
	{
		GetSprites ();
		ShuffleFoods ();
		InitFoods ();
	}
	
	void GetSprites()
	{
		for (int i = 0; i < foodNum; i++)
		{
			if(i == 13)		//small king
			{
				foodSprites[i] = Resources.Load<Sprite> ("52");
			}
			else  if(i == 14)  		//big king
			{
				foodSprites[i] = Resources.Load<Sprite> ("53");
			}
			else
			{
				foodSprites[i] = Resources.Load<Sprite>(i.ToString ());
			}
			orderedName[i] = foodSprites[i].name;
		}
	}

	void ShuffleFoods()
	{
		for (int i = foodSprites.Length - 1; i >= 0 ; i--) 
		{
			int ran = Random.Range (0, i + 1);
			Sprite temp = foodSprites[i];
			foodSprites[i] = foodSprites[ran];
			foodSprites[ran] = temp;
		}
	}

	void InitFoods()
	{
		for (int i = 0; i < foodNum; i++)
		{
			cells[i] = Instantiate (prefab) as GameObject;
			cells[i].transform.SetParent (panelRT);
			cells[i].name = orderedName[i];
			//go is the cell, this is for get food
			cells[i].GetComponentsInChildren<Image>()[1].sprite = foodSprites[i];
		}
	}

	public bool CheckWin()
	{
		foreach (GameObject cell in cells)
		{
			if (cell.name != cell.GetComponentsInChildren<Image>()[1].sprite.name) 
			{
				return false;
			}
		}
		return true;
	}
	
}













