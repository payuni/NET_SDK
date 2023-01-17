using payuniSDK;
using System;
namespace testuni
{
    class testuni
    {
        static void Main(string[] args)
        {
            string key = "RgVEIpc55RolRo3ji91UsDiNb3OcYVG8";
            string iv = "z6dHDPE0PbQ1C4JN";
            string type = "t";
            string tradeType = "credit_bind_query";
            EncryptInfoModel info = new EncryptInfoModel();

            info.MerID = "S07753315";
            info.MerTradeNo = "YTEST20220929103428";
            info.TradeAmt = "100";
            info.BankType = "822";
            info.Timestamp = "1664418868";

            payuniAPI test = new payuniAPI(key,iv,type);
            
            Console.WriteLine(test.UniversalTrade(info, tradeType));
        }
    }
}
