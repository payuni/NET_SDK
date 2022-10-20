using PayuniSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.examples.cardit_bind
{
    public class CardBind
    {        
        /// <summary>
        /// api宣告
        /// </summary>
        public payuniAPI payuniapi;
        /// <summary>
        /// 加密資訊宣告
        /// </summary>
        public EncryptInfoModel info;
        public CardBind(string request) {
             string key = "12345678901234567890123456789012";
             string iv = "1234567890123456";
             payuniapi = new payuniAPI(key, iv);
        }
        /// <summary>
        /// trade bind query sample code
        /// </summary>
        public void tradeBindQuery()
        {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            string result = payuniapi.UniversalTrade(info, "credit_bind_query");
        }
        /// <summary>
        /// treade bind cancel sample code
        /// </summary>
        public void tradeBindCancel()
        {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            info.UseTokenType = "1";
            info.BindVal = "1";
            string result = payuniapi.UniversalTrade(info, "credit_bind_cancel");
        }
    }

    
}
