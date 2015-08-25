//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18063
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
namespace AssemblyCSharp
{
    public class RX_Card
    {
        //define level property
        private RX_CARD_LEVEL level;
        public RX_CARD_LEVEL Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }
        //define name property
        private RX_CARD_NAME name;
        public RX_CARD_NAME Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        //is selected
        private bool isPop;
        public bool IsPop
        {
            set
            {
                this.isPop = value;
            }
            get
            {
                return this.isPop;
            }
        }
        //if selected ,we put it position.y up 10
        private float positionY;
        public float PositionY
        {
            get
            {
                if (isPop)
                {
                    return 10;
                }
                else
                {
                    return 0;
                }
            }
        }

        private int index_value;
        private int Index_value
        {
            set
            {
                index_value = value;
                //define name
                if (index_value >= RX_Define.RX_HOT_Number && index_value < RX_Define.RX_FAP_Number)
                {
                    name = RX_CARD_NAME.RX_NAME_HOT;
                }
                if (index_value >= RX_Define.RX_FAP_Number && index_value < RX_Define.RX_HET_Number)
                {
                    name = RX_CARD_NAME.RX_NAME_FAP;
                }
                if (index_value >= RX_Define.RX_HET_Number && index_value < RX_Define.RX_MEH_Number)
                {
                    name = RX_CARD_NAME.RX_NAME_HET;
                }
                if (index_value >= RX_Define.RX_MEH_Number && index_value < RX_Define.RX_SMA_Number)
                {
                    name = RX_CARD_NAME.RX_NAME_MEH;
                }
                //define level
                switch (index_value % 13)
                {
                    case 0: { this.Level = RX_CARD_LEVEL.RX_LEVEL_A; break; }
                    case 1: { this.Level = RX_CARD_LEVEL.RX_LEVEL_2; break; }
                    case 2: { this.Level = RX_CARD_LEVEL.RX_LEVEL_3; break; }
                    case 3: { this.Level = RX_CARD_LEVEL.RX_LEVEL_4; break; }
                    case 4: { this.Level = RX_CARD_LEVEL.RX_LEVEL_5; break; }
                    case 5: { this.Level = RX_CARD_LEVEL.RX_LEVEL_6; break; }
                    case 6: { this.Level = RX_CARD_LEVEL.RX_LEVEL_7; break; }
                    case 7: { this.Level = RX_CARD_LEVEL.RX_LEVEL_8; break; }
                    case 8: { this.Level = RX_CARD_LEVEL.RX_LEVEL_9; break; }
                    case 9: { this.Level = RX_CARD_LEVEL.RX_LEVEL_0; break; }
                    case 10: { this.Level = RX_CARD_LEVEL.RX_LEVEL_J; break; }
                    case 11: { this.Level = RX_CARD_LEVEL.RX_LEVEL_Q; break; }
                    case 12: { this.Level = RX_CARD_LEVEL.RX_LEVEL_K; break; }
                    default:
                        break;
                }

                //hanle king
                if (index_value == 52)
                {
                    name = RX_CARD_NAME.RX_NAME_KIN;
                    this.Level = RX_CARD_LEVEL.RX_LEVEL_S;
                }
                if (index_value == 53)
                {
                    name = RX_CARD_NAME.RX_NAME_KIN;
                    this.Level = RX_CARD_LEVEL.RX_LEVEL_B;
                }

                string_value = index_value.ToString();
            }
            get
            {
                return index_value;
            }
        }

        private string string_value;
        public RX_Card(int index)
        {
            this.Index_value = index;
        }

        public void SetIndex(int index)
        {
            this.Index_value = index;
        }

        //override father class ToString()
        public override string ToString()
        {
            return string_value;
        }

    }
}

