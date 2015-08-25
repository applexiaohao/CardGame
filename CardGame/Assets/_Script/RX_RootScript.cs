using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class RX_RootScript : MonoBehaviour {

    public UIInput nameField;
    public UIInput pwdField;

    private string userName;
    private string UserName
    {
        get {
            return nameField.value.Trim(); 
        }
    }

    private string userPwd;
    private string UserPwd
    {
        get { 
            return pwdField.value.Trim(); 
        }
    }

    private bool CheckUserInfo()
    {
        bool result = true;

        if (this.UserName.Length < 7 || this.UserPwd.Length < 7)
        {
            Debug.Log("invalid user information");
            result = false;
        }

        return result;
    }

	public void OnClicRegister()
    {
        if (CheckUserInfo())
        {
            //the 3rd param is the act type,0 means register,others means login
            RX_DataServer.DefaultServer.Register(this.UserName, this.UserPwd, 0, (object data) =>
            {
                Debug.Log(data.ToString());
            });
        }
        
    }
    public void OnClickLogin()
    {
        if (CheckUserInfo())
        {
            RX_DataServer.DefaultServer.Register(this.UserName, this.UserPwd, 1, (object data) =>
            {
                RX_UserInfo user = (RX_UserInfo)LO_XMLTool.Deserialize(typeof(RX_UserInfo),data.ToString());

                if (user == null)
                {
                    Debug.Log("failed");
                }
                else
                {
                    RX_UserManager.DefaultUser = user;
                    Application.LoadLevel("Room");
                }
            });
        }
    }

    void Start()
    {
        Application.runInBackground = true; 
    }
}
