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
using System.Collections.Generic;

namespace AssemblyCSharp
{
		public enum RX_CARD_SET
		{
				RX_TYPE_DAN				= 0,
				RX_TYPE_DUI				= 1,
				RX_TYPE_FEI_BUDAI	= 2,
				RX_TYPE_FEI_DAI		= 3,
				RX_TYPE_SHUN			= 4,
				RX_TYPE_LIANDUI		= 5,
				RX_TYPE_BOOM			= 6,
				RX_TYPE_WANGZHA		= 7,
				RX_TYPE_SIDAIER		= 8,
				RX_TYPE_SAN_BUDAI	= 9,
				RX_TYPE_SAN_DAI	= 10
		}

		public class RX_CardSet
		{
				#region MyRegion
				//pokerSet list
				private List<RX_Card>	 card_list;

				public List<RX_Card>	 Card_lister {
						get {
								return card_list;
						}
						set {
								card_list = value;
						}
				}

				//card type
				private RX_CARD_SET 	card_type;

				public RX_CARD_SET	Card_type {
						get {
								return card_type;
						}
						set {
								card_type = value;
						}
				}

				//card leve
				private RX_CARD_LEVEL 	card_level;

				public RX_CARD_LEVEL		Card_level {
						get {
								return card_level;
						}
						set {
								card_level = value;
						}
				}
				#endregion


				/// <summary>
				/// Initializes a new instance of the <see cref="AssemblyCSharp.RX_CardSet"/> class.
				/// </summary>
				public RX_CardSet ()
				{
				}
		}
}
