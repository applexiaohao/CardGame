using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class M_FoodMoveScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 originPos;
	private bool isDrag;
	private RectTransform rt;
	private RectTransform fatherRT;
	private RectTransform canvasRT;
	private CanvasGroup cg;

	private bool drawWin;

	void Awake()
	{
		rt = GetComponent<RectTransform>();
		canvasRT = GameObject.FindWithTag ("Canvas").GetComponent<RectTransform>() ;
		cg = GetComponent<CanvasGroup>();
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		originPos = rt.position;
		fatherRT = rt.parent.GetComponent<RectTransform>();
		cg.blocksRaycasts = false;
		isDrag = true;
	}

	Vector3 curMousePos;
	public void OnDrag (PointerEventData eventData)
	{
		if (isDrag)
		{
			//move
			SetParent (canvasRT, rt);
			RectTransformUtility.ScreenPointToWorldPointInRectangle (rt, Input.mousePosition, eventData.enterEventCamera, out curMousePos);
			rt.position = curMousePos;
		}
	}

	GameObject underObj;
	public void OnEndDrag (PointerEventData eventData)
	{
		//get underObj
		underObj = eventData.pointerEnter;
		//change parent
		if (underObj && underObj.tag == this.tag)
		{
			SetParent (underObj.transform.parent, rt);
			SetParent (fatherRT, underObj.transform);
		}
		else
		{
			SetParent (fatherRT, rt);
		}
		cg.blocksRaycasts = true;
		isDrag = false;
		if( Camera.main.GetComponent<M_ManagerScript>().CheckWin () )
		{
			drawWin = true;
		}
	}
	
	void SetParent(Transform father, Transform son)
	{
		son.SetParent (father);
		son.localPosition = Vector3.zero;
	}

	void OnGUI()
	{
		if (drawWin) 
		{
			if(GUI.Button (new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 50), "Congratulations! You Win! Press Here To Restart!") )
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}







}
