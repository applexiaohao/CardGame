using System;
using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
    class RX_DataServer:MonoBehaviour
    {
        public delegate void DataServerHandler(object sender);
        private DataServerHandler blockEvent;

        #region Singleton
        private RX_DataServer() { }
        private static RX_DataServer s_RX_DataServer = null;
        private static GameObject s_RX_DataObject = null;
        public static RX_DataServer DefaultServer
        {
            get
            {
                s_RX_DataObject = new GameObject("DataServer");
                s_RX_DataServer = s_RX_DataObject.AddComponent<RX_DataServer>();

                return s_RX_DataServer;
            }
        }
        #endregion

        private bool isConnectedToDataServer = false;
        private string Name { get; set; }
        private string Pwd {get; set;}
        public void Register(string name, string pwd, int act, DataServerHandler sender)
        {
            if (isConnectedToDataServer)
            {
                return;
            }
            this.blockEvent = sender;
            this.Name = name;
            this.Pwd = pwd;
            StartCoroutine(RegisterToDataServer(act));
        }

        IEnumerator RegisterToDataServer(int act)
        {
            string register_url = "http://project.lanou3g.com/game/unity/doudizhu/login.php?";
            //act : 0 is register ,else is login
            if (act == 0)  
            {
                register_url = "http://project.lanou3g.com/game/unity/doudizhu/register.php?";
            }
            
            register_url += "name=" + this.Name;
            register_url += "&&";
            register_url += "pwd=" + this.Pwd;
            WWW www = new WWW(register_url);

            yield return www;
            isConnectedToDataServer = false;
            this.blockEvent(www.text);
        }

        public static void Login(string name, string pwd)
        {

        }







    }
}
