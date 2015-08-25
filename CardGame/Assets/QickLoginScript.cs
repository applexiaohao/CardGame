using UnityEngine;
using System.Collections;

public class QickLoginScript : MonoBehaviour {

    public UIInput nameField;
    public UIInput pwdField;

	public void Login1()
    {
        nameField.value = "sirlpc1";
        pwdField.value = "123456789";
    }

    public void Login2()
    {
        nameField.value = "sirlpc2";
        pwdField.value = "123456789";
    }

    public void Login3()
    {
        nameField.value = "sirlpc3";
        pwdField.value = "123456789";
    }
}
