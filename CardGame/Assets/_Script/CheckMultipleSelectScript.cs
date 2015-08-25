using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class CheckMultipleSelectScript : MonoBehaviour {

    Ray ray;
    RaycastHit hit;
	void Update () 
    {
	    if (Input.GetMouseButton(0))
	    {
            ray = transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)
                && hit.collider.transform.parent == RX_Manager.bottom_seat.Seat_container.transform)
            {
                RX_Card selectCard = RX_Manager.bottom_seat.Card_list.Find((RX_Card obj) => { return obj.ToString() == hit.collider.name; });

                if (!selectCard.IsPop)
                {
                    selectCard.IsPop = !selectCard.IsPop;
                    UISprite selectSprit = hit.collider.GetComponent<UISprite>();
                    //selectSprit.SetRect(selectSprit.worldCenter.x, selectSprit.worldCenter.y + 10,
                    //    52f, 70f);
                    Vector3 toPos = selectSprit.transform.position + Vector3.up * 0.045f;
                    iTween.MoveTo(selectSprit.gameObject, toPos, 0f);
                }

            }
            
	    }
	    
	}
}
