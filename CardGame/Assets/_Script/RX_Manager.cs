using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using AssemblyCSharp;

public class RX_Manager : MonoBehaviour 
{
    public UILabel labelMsg;
    public UILabel labelBottom;
    public UILabel labelLeft;
    public UILabel labelRight;
    public UIButton popBtn;
    public UIButton passBtn;

    public UISprite handed_bootom;
    public UISprite handed_left;
    public UISprite handed_right;

    private UISprite bottom_pool;
    private UISprite left_pool;
    private UISprite right_pool;
    private UILabel giveAHandButtonLabel;

    public static RX_SeatInfo bottom_seat;
    private RX_SeatInfo left_seat;
    private RX_SeatInfo right_seat;

    private RX_SeatInfo currentSeat;
    public static RX_CardSet prevCardSet;
    private int passCount = 0;

    private List<string> userList;
    List<RX_Card> list;

    enum RX_GAME_STATE
    {
        QIANGDIZHU  = 0,
        PLAYING     = 1,
        BALANCE     = 2,
    }
    private RX_GAME_STATE gameState;
    

    void Awake()
    {
        bottom_pool = GameObject.FindWithTag("cbottom").GetComponent<UISprite>();
        left_pool = GameObject.FindWithTag("cleft").GetComponent<UISprite>(); ;
        right_pool = GameObject.FindWithTag("cright").GetComponent<UISprite>();
        giveAHandButtonLabel = GameObject.FindWithTag("GiveAHand").GetComponent<UILabel>();
        userList = new List<string>();
        list = new List<RX_Card>();

        if (Network.isServer)
        {
            userList.Add(RX_UserManager.DefaultUser.user_name);
        }
    }

    void Start()
    {
        Application.runInBackground = true;
        if (Network.isServer)
        {
        }
        if (Network.isClient)
        {
            GetComponent<NetworkView>().RPC("InitUserList", RPCMode.Server, RX_UserManager.DefaultUser.user_name);
        }
        labelMsg.text += RX_UserManager.DefaultUser.user_id;
        popBtn.onClick.Add(new EventDelegate(() => { PopSet(0); }));
        passBtn.onClick.Add(new EventDelegate(() => { Pass(0); }));
    }

    [RPC]
    void InitReshuffleRPC(string pokerInfos)
    {
        #region Clear the existing cards on the screen
        List<UISprite> clearBottom = new List<UISprite>(bottom_pool.GetComponentsInChildren<UISprite>());
        List<UISprite> clearLeft = new List<UISprite>(left_pool.GetComponentsInChildren<UISprite>());
        List<UISprite> clearAll = new List<UISprite>(right_pool.GetComponentsInChildren<UISprite>());
        clearAll.AddRange(clearBottom);
        clearAll.AddRange(clearLeft);
        clearAll.RemoveAll((UISprite obj) =>
        {
            return obj.tag == "cbottom"
                || obj.tag == "cleft" || obj.tag == "cright";
        });
        for (int i = 0; i < clearAll.Count; i++)
        {
            Destroy(clearAll[i].gameObject);
        }
        #endregion

        string[] pairs = pokerInfos.Split('|');

        //convert pokerInfo to cardList
        int[] cardsdInfo = new int[54];
        for (int i = 0; i < 54; i ++ )
        {
            cardsdInfo[i] = int.Parse(pairs[i].Split(':')[1]);
        }
        list = CreateCardList(cardsdInfo);

        //if current player is the 2nd player...
        if (pairs[17].Split(':')[0] == RX_UserManager.DefaultUser.user_name)
        {
            //generate cardInfo array,(killed userName)  -- me bottom
            labelBottom.text = (pairs[17].Split(':')[0]);
            bottom_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_BOTTOM, this.bottom_pool);
            bottom_seat.Card_list = list.GetRange(17, 17);
            bottom_seat.Seat_user = pairs[17].Split(':')[0];
            bottom_seat.Handed_container = handed_bootom;
            RX_CardManager.ClearPool();

            //server -- right
            labelRight.text = (pairs[0].Split(':')[0]);
            right_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_RIGHT, this.right_pool);
            right_seat.Card_list = list.GetRange(0, 17);
            right_seat.Seat_user = pairs[0].Split(':')[0];
            right_seat.Handed_container = handed_right;
            RX_CardManager.ClearPool();

            //3rd -- left
            labelLeft.text = (pairs[34].Split(':')[0]);
            left_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_LEFT, this.left_pool);
            left_seat.Card_list = list.GetRange(34, 17);
            left_seat.Seat_user = pairs[34].Split(':')[0];
            left_seat.Handed_container = handed_left;
            RX_CardManager.ClearPool();

            //set diZhu....
            InitGame(right_seat);
        }

        //if current player is the 3nd player...
        if (pairs[34].Split(':')[0] == RX_UserManager.DefaultUser.user_name)
        {
            //generate cardInfo array,(killed userName)  -- me bottom
            labelBottom.text = (pairs[34].Split(':')[0]);
            bottom_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_BOTTOM, this.bottom_pool);
            bottom_seat.Card_list = list.GetRange(34, 17);
            bottom_seat.Seat_user = pairs[34].Split(':')[0];
            bottom_seat.Handed_container = handed_bootom;
            RX_CardManager.ClearPool();

            //server -- left
            labelLeft.text = (pairs[0].Split(':')[0]);
            left_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_LEFT, this.left_pool);
            left_seat.Card_list = list.GetRange(0, 17);
            left_seat.Seat_user = pairs[0].Split(':')[0];
            left_seat.Handed_container = handed_left;
            RX_CardManager.ClearPool();

            //3rd -- right
            labelRight.text = (pairs[17].Split(':')[0]);
            right_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_RIGHT, this.right_pool);
            right_seat.Card_list = list.GetRange(17, 17);
            right_seat.Seat_user = pairs[17].Split(':')[0];
            right_seat.Handed_container = handed_right;
            RX_CardManager.ClearPool();

            //set diZhu....
            InitGame(left_seat);
        }

        //if current player is the server player...
        if (pairs[0].Split(':')[0] == RX_UserManager.DefaultUser.user_name)
        {
            //generate cardInfo array,(killed userName)  -- me bottom
            labelBottom.text = (pairs[0].Split(':')[0]);
            bottom_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_BOTTOM, this.bottom_pool);
            bottom_seat.Card_list = list.GetRange(0, 17);
            bottom_seat.Seat_user = pairs[0].Split(':')[0];
            bottom_seat.Handed_container = handed_bootom;
            RX_CardManager.ClearPool();

            //server -- left
            labelLeft.text = (pairs[17].Split(':')[0]);
            left_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_LEFT, this.left_pool);
            left_seat.Card_list = list.GetRange(17, 17);
            left_seat.Seat_user = pairs[17].Split(':')[0];
            left_seat.Handed_container = handed_left;
            RX_CardManager.ClearPool();

            //3rd -- right
            labelRight.text = (pairs[34].Split(':')[0]);
            right_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_RIGHT, this.right_pool);
            right_seat.Card_list = list.GetRange(34, 17);
            right_seat.Seat_user = pairs[34].Split(':')[0];
            right_seat.Handed_container = handed_right;
            RX_CardManager.ClearPool();

            //set diZhu....
            InitGame(bottom_seat);
        }

        //clear handed cards
        foreach (KeyValuePair<RX_SEAT_POSITION, List<UISprite>> item in RX_CardManager.HandedSprites)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                Destroy(item.Value[i]);
            }
        }
        //new prevCardSet
        prevCardSet = null;

    }

    //convert the int array to cardList
    List<RX_Card> CreateCardList(int[] cardsInfo)
    {
        List<RX_Card> listResult = new List<RX_Card>();
        for (int i = 0; i < cardsInfo.Length; i++)
        {
            listResult.Add(new RX_Card(cardsInfo[i]));
        }
        return listResult;
    }

    [RPC]
    void InitUserList(string userName)
    {   
        if (userList.Count < 3)
        {
            userList.Add(userName);
        }
        if (userList.Count == 3)
        {

            this.Reshuffle();
            string sendData = "";
            for (int i = 0; i < 17; i ++ )
            {
                sendData += userList[0] + ":";
                if (int.Parse(list[i].ToString()) < 10)
                {
                    sendData += "0" + list[i].ToString();
                }
                else
                {
                    sendData += list[i].ToString();
                }
                sendData += "|";
            }
            for (int i = 17; i < 34; i++)
            {
                sendData += userList[1] + ":";
                if (int.Parse(list[i].ToString()) < 10)
                {
                    sendData += "0" + list[i].ToString();
                }
                else
                {
                    sendData += list[i].ToString();
                }
                sendData += "|";
            }
            for (int i = 34; i < 51; i++)
            {
                sendData += userList[2] + ":";
                if (int.Parse(list[i].ToString()) < 10)
                {
                    sendData += "0" + list[i].ToString();
                }
                else
                {
                    sendData += list[i].ToString();
                }
                sendData += "|";
            }
            //add dizhu's card 51,52,53
            for (int i = 51; i < 54; i++)
            {
                sendData += "dizhu:";
                if (int.Parse(list[i].ToString()) < 10)
                {
                    sendData += "0" + list[i].ToString();
                }
                else
                {
                    sendData += list[i].ToString();
                }
                sendData += "|";
            } 
            //user info saved OK, tongBu poker data on client. 
            GetComponent<NetworkView>().RPC("InitReshuffleRPC", RPCMode.All, sendData);
        }
    }

    #region Reshuffle
    public void Reshuffle()
    {
        list = RX_CardManager.DefaultManager.Reshuffle();

        #region Clear the existing cards on the screen
        List<UISprite> clearBottom = new List<UISprite>(bottom_pool.GetComponentsInChildren<UISprite>());
        List<UISprite> clearLeft = new List<UISprite>(left_pool.GetComponentsInChildren<UISprite>());
        List<UISprite> clearAll = new List<UISprite>(right_pool.GetComponentsInChildren<UISprite>());
        clearAll.AddRange(clearBottom);
        clearAll.AddRange(clearLeft);
        clearAll.RemoveAll((UISprite obj) => { return obj.tag == "cbottom" 
            || obj.tag == "cleft" || obj.tag == "cright"; });
        for (int i = 0; i < clearAll.Count; i ++ )
        {
            Destroy(clearAll[i].gameObject);
        }
        #endregion

        bottom_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_BOTTOM, this.bottom_pool);
        bottom_seat.Card_list = list.GetRange(0, 17);
        RX_CardManager.ClearPool();
        left_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_LEFT, this.left_pool);
        left_seat.Card_list = list.GetRange(17, 17);
        RX_CardManager.ClearPool();
        right_seat = new RX_SeatInfo(RX_SEAT_POSITION.RX_SEAT_RIGHT, this.right_pool);
        right_seat.Card_list = list.GetRange(34, 17);
        RX_CardManager.ClearPool();

        foreach (KeyValuePair<RX_SEAT_POSITION,List<UISprite>> item in RX_CardManager.HandedSprites)
        {
            for (int i = 0; i < item.Value.Count; i ++ )
            {
                Destroy(item.Value[i]);
            }
        }

        prevCardSet = null;
    }
    #endregion

    void InitGame(RX_SeatInfo sender)
    {
        gameState = RX_GAME_STATE.QIANGDIZHU;
        currentSeat = sender;
        passCount = 0;

        //set labels
        SetRoll(currentSeat, RX_ROLE_TYPE.RX_ROLE_NORMAL);
        SetRoll(left_seat, RX_ROLE_TYPE.RX_ROLE_NORMAL);
        SetRoll(right_seat, RX_ROLE_TYPE.RX_ROLE_NORMAL);

        currentSeat.SetLabel(1, "������?");
        giveAHandButtonLabel.text = "������";
    }

    /// <summary>
    /// pop the selected cardset
    /// </summary>
    RX_CardSet currentCardSet;
    [RPC]
    public void PopSet(int actor)
    {
        if (currentSeat == bottom_seat || actor == 1)
        {
            if (gameState == RX_GAME_STATE.QIANGDIZHU)
            {
                SetRoll(currentSeat, RX_ROLE_TYPE.RX_ROLE_DIZHU);
                giveAHandButtonLabel.text = "����";
                gameState = RX_GAME_STATE.PLAYING;
                if (currentSeat == bottom_seat)
                {
                    GetComponent<NetworkView>().RPC("PopSet", RPCMode.Others, 1);
                }
                return;
            }

            currentCardSet = currentSeat.PopCardSet();
            if (currentCardSet == null)
            {
                currentSeat.ClearPopCard();
                labelMsg.text = "Failed!";
            }
            else
            {
                //tongBu other's pop cards    RX_CardSet
                if (currentSeat == bottom_seat)
                {
                    string sendData = "";
                    for (int i = 0; i < currentCardSet.Card_lister.Count; i++ )
                    {
                        sendData += currentCardSet.Card_lister[i].ToString() + "|";
                    }
                    GetComponent<NetworkView>().RPC("TongBuPopSet", RPCMode.Others, sendData);
                }

                currentSeat.ClearPopCard();
                passCount = 0;

                if (currentSeat.Card_list.Count == 0)
                {
                    labelMsg.text = currentSeat.Seat_label.text.Split('-')[0] + "��ʤ!";
                    return;
                }
                currentSeat.SetLabel(1, "�ȴ�����");

                //set currentSet to next
                SkipHand();

                labelMsg.text = "Play Success!\n";
                labelMsg.text += (currentCardSet.Card_type + "\n" + currentCardSet.Card_level.ToString() + "\n");
                labelMsg.text += "-->" + currentSeat.Seat_pos.ToString() + "<-- turn!\n";
                prevCardSet = currentCardSet;
                currentCardSet = null;
                return;
            }
        }
        else
        {
            labelMsg.text += "Not your turn!";
        }
    }

    [RPC]
    void TongBuPopSet(string cardStringData)
    {
        Debug.Log(cardStringData);
        string[] cardStringArray = cardStringData.Split('|');
        int[] cardsArray = new int[cardStringArray.Length - 1];
        for (int i = 0; i < cardsArray.Length; i ++ )
        {
            cardsArray[i] = int.Parse(cardStringArray[i]);
        }

        List<RX_Card> cardList = CreateCardList(cardsArray);
        for (int i = 0; i < currentSeat.Card_list.Count; i ++ )
        {
            for (int j = 0; j < cardList.Count; j ++ )
            {
                if (currentSeat.Card_list[i].ToString() == cardList[j].ToString())
                {
                    currentSeat.Card_list[i].IsPop = true;
                }
            }
        }
        PopSet(1);
    }

    [RPC]
    public void Pass(int actor)
    {
        if (currentSeat == bottom_seat || actor == 1)
        {
            Debug.Log(gameState.ToString());
            if (gameState == RX_GAME_STATE.QIANGDIZHU)
            {
                if (++passCount == 3)
                {
                    GetComponent<NetworkView>().RPC("InitUserList", RPCMode.Server, RX_UserManager.DefaultUser.user_name);
                    return;
                }
                if (currentSeat == bottom_seat)
                {
                    GetComponent<NetworkView>().RPC("Pass", RPCMode.Others, 1);
                }
                SetRoll(currentSeat, RX_ROLE_TYPE.RX_ROLE_NONGMIN);
                SkipHand();
                return;
            }
            if (currentSeat == bottom_seat)
            {
                GetComponent<NetworkView>().RPC("Pass", RPCMode.Others, 1);
            }
            if (++passCount < 2)     //one player pass,continue..
            {
                currentSeat.SetLabel(1, "Ҫ����");
                SkipHand();
            }
            else if (passCount == 2)
            {
                currentSeat.SetLabel(1, "Ҫ����");
                SkipHand();
                prevCardSet = null;
            }
            else                     //two player pass, cannot pass anymore, start new round..
            {
                currentSeat.SetLabel(1, "�»غ�");
                prevCardSet = null;
            }
        }
        else
        {
            labelMsg.text += "Not your turn!";
        }
    }

    void SkipHand()
    {
        switch (currentSeat.Seat_pos)
        {
            case RX_SEAT_POSITION.RX_SEAT_BOTTOM:
                currentSeat = left_seat;
                break;
            case RX_SEAT_POSITION.RX_SEAT_LEFT:
                currentSeat = right_seat;
                break;
            case RX_SEAT_POSITION.RX_SEAT_RIGHT:
                currentSeat = bottom_seat;
                break;
            default:
                break;
        }
        if (gameState == RX_GAME_STATE.QIANGDIZHU)
        {
            currentSeat.SetLabel(1, "������?");
            return;
        }
        currentSeat.SetLabel(1, "���ڳ���");
    }

    void SetRoll(RX_SeatInfo seat, RX_ROLE_TYPE roll)
    {
        switch (roll)
        {
            case RX_ROLE_TYPE.RX_ROLE_DIZHU:
                {
                    seat.SetLabel(0, "����");
                    seat.SetLabel(1, "���ڳ���");
                    //prevent player get DiZhu , press pass in the same round..
                    passCount = 3;

                    //set new pokerSprite (add the last 3 cards)..
                    seat.Seat_label.transform.SetParent(giveAHandButtonLabel.transform);
                    seat.Seat_container.transform.DestroyChildren();
                    seat.Seat_label.transform.SetParent(seat.Seat_container.transform);
                    List<RX_Card> temp = new List<RX_Card>(seat.Card_list);
                    temp.AddRange(list.GetRange(51, 3));
                    seat.Card_list = temp;
                    
                    seat.Seat_type = RX_ROLE_TYPE.RX_ROLE_DIZHU;
                    //after set current DiZhu , we must set other two NongMing
                    switch (seat.Seat_pos)
                    {
                        case RX_SEAT_POSITION.RX_SEAT_BOTTOM:
                            SetRoll(right_seat, RX_ROLE_TYPE.RX_ROLE_NONGMIN);
                            SetRoll(left_seat, RX_ROLE_TYPE.RX_ROLE_NONGMIN);
                            break;
                        case RX_SEAT_POSITION.RX_SEAT_LEFT:
                            SetRoll(right_seat, RX_ROLE_TYPE.RX_ROLE_NONGMIN);
                            SetRoll(bottom_seat, RX_ROLE_TYPE.RX_ROLE_NONGMIN);
                            break;
                        case RX_SEAT_POSITION.RX_SEAT_RIGHT:
                            SetRoll(bottom_seat, RX_ROLE_TYPE.RX_ROLE_NONGMIN);
                            SetRoll(left_seat, RX_ROLE_TYPE.RX_ROLE_NONGMIN);
                            break;
                        default:
                            break;
                    }
                    break;
                }
            case RX_ROLE_TYPE.RX_ROLE_NONGMIN:
                {
                    seat.SetLabel(0, "ũ��");
                    seat.SetLabel(1, "����");
                    seat.Seat_type = RX_ROLE_TYPE.RX_ROLE_DIZHU;
                    break;
                }
            case RX_ROLE_TYPE.RX_ROLE_NORMAL:
                {
                    seat.SetLabel(0, "����");
                    seat.SetLabel(1, "�ȴ���");
                    seat.Seat_type = RX_ROLE_TYPE.RX_ROLE_DIZHU;
                    break;
                }
            default:
                break;
        }
    }

    void OnGUI()
    {
        switch (Network.peerType)
        {
            case NetworkPeerType.Client:
                GUILayout.Label("Client..");
                break;
            case NetworkPeerType.Connecting:
                GUILayout.Label("Connecting..");
                break;
            case NetworkPeerType.Disconnected:
                GUILayout.Label("disconnected..");
                break;
            case NetworkPeerType.Server:
                {
                    GUILayout.Label("server..");
                    for (int i = 0; i < Network.connections.Length; i++)
                    {
                        GUILayout.Label("IP=" + Network.connections[i].ipAddress);
                    }
                    break;
                }

            default:
                break;
        }
    }
	
}
