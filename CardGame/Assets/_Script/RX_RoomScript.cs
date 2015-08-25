using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class RX_RoomScript : MonoBehaviour {

    public UILabel sceneTitle;

    /// <summary>
    /// Create room button
    /// </summary>
    public void CreateRoom()
    {
        LO_GameServer.DefaultServer.StartServer(sceneTitle.text, 25000);
        Debug.Log(RX_UserManager.DefaultUser.user_name);
    }

    public UIGrid room_grid;
    public GameObject room_item;
    /// <summary>
    /// Get Room List
    /// </summary>
    public void GetRoomList()
    {
        LO_GameServer.DefaultServer.StartRequestRoom((HostData[] dataList) =>
        {
            NGUITools.DestroyChildren(room_grid.transform);
            foreach (HostData item in dataList)
            {
                HostData temp = item;
                GameObject room = NGUITools.AddChild(room_grid.gameObject, room_item);
                room.name = item.gameName;
                UILabel[] labels = room.GetComponentsInChildren<UILabel>();
                labels[0].text = item.gameName;
                labels[1].text = item.connectedPlayers.ToString();
                labels[1].text += " / " + item.playerLimit.ToString();
                UIButton button = room.GetComponentInChildren<UIButton>();
                
                button.onClick.Add(new EventDelegate(() =>{
                    LO_GameServer.DefaultServer.JoinHostRoom(temp, (int state) =>
                    {
                        Debug.Log(state.ToString());
                    });   
                }));

                if (item.connectedPlayers > 2)
                {
                    button.isEnabled = false ;
                }
           
                room_grid.enabled = true;
            }
        });
    }

	void Start ()
    {
        Application.runInBackground = true; 
        LO_GameServer.DefaultServer.InitServer("115.28.227.1", 23466);
        if (RX_UserManager.DefaultUser!=null)
        {
            sceneTitle.text = RX_UserManager.DefaultUser.user_name;
        }
	}
	

}
