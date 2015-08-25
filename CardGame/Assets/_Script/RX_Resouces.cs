using System;
using UnityEngine;

namespace AssemblyCSharp
{
    /// <summary>
    /// for add atlas
    /// </summary>
    class RX_Resources
    {
        private RX_Resources()
        {
            card_altlas = (UIAtlas)Resources.Load("Puke", typeof(UIAtlas));
        }

        //singleton
        private static RX_Resources s_RX_Resouces = null;
        public static RX_Resources DefaultResources
        {
            get
            {
                if (s_RX_Resouces == null)
                {
                    s_RX_Resouces = new RX_Resources();
                }
                return s_RX_Resouces;
            }
        }

        private UIAtlas card_altlas;
        public UIAtlas Card_atlas
        {
            get
            {
                return card_altlas;
            }
        }




    }
}
