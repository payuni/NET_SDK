using payuniSDK;
using System;
using System.Web;

namespace testuni
{
    class testuni
    {
        static void Main(string[] args)
        {
            string key = "RgVEIpc55RolRo3ji91UsDiNb3OcYVG8";
            string iv = "z6dHDPE0PbQ1C4JN";
            string type = "t";
            string tradeType = "trade_refund_linepay";
            EncryptInfoModel info = new EncryptInfoModel();

            info.MerID = "S07753315";
            info.TradeNo = "Yz20230503103428";
            info.MerTradeNo = "Yz20230503103428";
            info.TradeAmt = "100";
            info.BankType = "822";
            info.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            info.PayNo = "12345";
            //info.IsPlatForm = "";//代理商模式 若要啟用 參數給1
            //info.MerTradeNo = "Yz20230503103428";
            //info.CardNo = "4147631000000001";//payuni 文件提供的測試卡號
            //info.CardCVC = "123";//信用卡安全碼隨意填
            //info.CardExpired = "0530";//MMYY
            
            payuniAPI test = new payuniAPI(key,iv,type);
            
            Console.WriteLine(HttpUtility.UrlDecode(test.UniversalTrade(info, tradeType)));
        }
    }
}
