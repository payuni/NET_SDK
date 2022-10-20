using PayuniSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.examples.trade
{
    public class Trade
    {
        /// <summary>
        /// api宣告
        /// </summary>
        public payuniAPI payuniapi;
        /// <summary>
        /// 加密資訊宣告
        /// </summary>
        public EncryptInfoModel info;

        public Trade(string request)
        {
            string key = "12345678901234567890123456789012";
            string iv = "1234567890123456";
            payuniapi = new payuniAPI(key, iv);
        }

        /// <summary>
        /// trade query sample code
        /// </summary>
        public void tradeQuery()
        {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.MerTradeNo = "test20220829111528";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            string result = payuniapi.UniversalTrade(info, "trade_query");
        }

        /// <summary>
        /// trade close sample code
        /// </summary>
        public void tradeClose()
        {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.MerTradeNo = "16614190477810373246";
            info.CloseType = "1";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            string result = payuniapi.UniversalTrade(info, "trade_close");
        }

        /// <summary>
        /// trade cancel sample code
        /// </summary>
        public void tradeCancel()
        {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.MerTradeNo = "16614190477810373246";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            string result = payuniapi.UniversalTrade(info, "trade_cancel");
        }

        /// <summary>
        /// trade refund icash sample code
        /// </summary>
        public void tradeRefundIcash()
        {
            info = new EncryptInfoModel();
            info.MerID = "abc";
            info.MerTradeNo = "1665472985627866043";
            info.TradeAmt = "100";
            info.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            string result = payuniapi.UniversalTrade(info, "trade_refund_icash");
        }
    }
}
