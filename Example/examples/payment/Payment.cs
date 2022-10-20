using PayuniSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.examples.payment
{
    public class Payment
    {
        /// <summary>
        /// api宣告
        /// </summary>
        public payuniAPI payuniapi;
        /// <summary>
        /// 加密資訊宣告
        /// </summary>
        public EncryptInfoModel info;
        public Payment(string request)
        {
            string key = "12345678901234567890123456789012";
            string iv = "1234567890123456";
            payuniapi = new payuniAPI(key, iv);
        }

        /// <summary>
        /// upp sample code
        /// </summary>
        public void upp() {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.MerTradeNo = "test"+DateTime.Now.ToString("yyyyMMddHHmmss");
            info.TradeAmt = "100";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            info.ReturnURL = "http://www.test.com.tw/api/return";
            info.NotifyURL = "http://www.test.com.tw/api/notify";
            string result = payuniapi.UniversalTrade(info, "upp");
        }

        /// <summary>
        /// returnURL sample code
        /// </summary>
        /// <param name="request"></param>
        public void returnURL(string request) {
            ResultModel result = new ResultModel();
            result = payuniapi.ResultProcess(request);
        }
        
        /// <summary>
        /// notifyURL sample code
        /// </summary>
        /// <param name="request"></param>
        public void notifyURL(string request)
        {
            ResultModel result = new ResultModel();
            result = payuniapi.ResultProcess(request);
        }

        /// <summary>
        /// credit sample code
        /// </summary>
        public void credit()
        {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.MerTradeNo = "test" + DateTime.Now.ToString("yyyyMMddHHmmss");
            info.TradeAmt = "100";
            info.CardNo = "1234567890123456";
            info.CardCVC = "123";
            info.CardExpired = "1230";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            
            string result = payuniapi.UniversalTrade(info, "credit");
        }

        /// <summary>
        /// atm sample code
        /// </summary>
        public void atm()
        {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.MerTradeNo = "test" + DateTime.Now.ToString("yyyyMMddHHmmss");
            info.TradeAmt = "100";
            info.BankType = "822";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            string result = payuniapi.UniversalTrade(info, "atm");
        }

        /// <summary>
        /// cvs sample code
        /// </summary>
        public void cvs()
        {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.MerTradeNo = "test" + DateTime.Now.ToString("yyyyMMddHHmmss");
            info.TradeAmt = "100";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            string result = payuniapi.UniversalTrade(info, "cvs");
        }
    }
}
