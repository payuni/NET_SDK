using System;
using System.Collections.Generic;
using System.Text;

namespace payuniSDK
{
    public class EncryptInfoModel
    {
        public string MerID { get; set; }
        public string MerTradeNo { get; set; }
        public string TradeAmt { get; set; }
        public string Timestamp { get; set; }
        //UPP
        public string ReturnURL { get; set; }
        public string NotifyURL { get; set; }
        public string BackURL { get; set; }
        public string UsrMail { get; set; }
        public string UsrMailFix { get; set; }
        public string UseTokenType { get; set; }
        public string CreditShowType { get; set; }
        public string CreditToken { get; set; }
        public string ProdDesc { get; set; }
        public string CreditTokenType { get; set; }
        public string CreditTokenExpired { get; set; }
        public string ExpireDate { get; set; }
        public string TradeLExpireSec { get; set; }
        public string Credit { get; set; }
        public string ICash { get; set; }
        public string ATM { get; set; }
        public string CVS { get; set; }
        public string CreditUnionPay { get; set; }
        public string CreditRed { get; set; }
        public string CreditInst { get; set; }

        //ATM
        public string BankType { get; set; }
        public string PaySet { get; set; }

        //credit
        public string CardNo { get; set; }
        public string CardCVC { get; set; }
        public string CardInst { get; set; }
        public string CardType { get; set; }
        public string CardExpired { get; set; }
        public string CreditHash { get; set; }
        public string API3D { get; set; }
        public string PayNo { get; set; }

        //trade_close
        public string CloseType { get; set; }
        public string TradeNo { get; set; }

        //credit_bind_cancel
        public string BindVal { get; set; }
        public string IsPlatForm { get; set; }
    }
}
