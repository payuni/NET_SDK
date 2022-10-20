using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace PayuniSDK
{
    public class payuniAPI
    {
        /// <summary>
        /// 加密資訊
        /// </summary>
        public EncryptInfoModel EncryptInfo;
        /// <summary>
        /// 傳入傳出參數
        /// </summary>
        public ParameterModel Parameter;
        /// <summary>
        /// 回傳結果
        /// </summary>
        public ResultModel Result;
        /// <summary>
        /// 加密字串
        /// </summary>
        public static string Plain { get; set; }
        /// <summary>
        /// 介接的 merKey
        /// </summary>
        public static string MerKey { get; set; }
        /// <summary>
        /// 介接的 merIV
        /// </summary>
        public static string MerIV { get; set; }
        /// <summary>
        /// api網址
        /// </summary>
        public static string ApiUrl { get; set; }
        /// <summary>
        /// 網域
        /// </summary>
        public static string Prefix { get; set; }
        /// <summary>
        /// 型態
        /// </summary>
        public static string Type { get; set; }

        public payuniAPI(string key, string iv, string type="")
        {
            EncryptInfo = new EncryptInfoModel();
            Parameter = new ParameterModel();
            Result = new ResultModel();
            MerKey = key;
            MerIV = iv;
            Type = type;
            ApiUrl = "api.payuni.com.tw/api/";
            Prefix = "https://";
            if (Type == "t")
            {
                Prefix += "sandbox-";
            }
            ApiUrl = Prefix + ApiUrl;
        }

        /// <summary>
        /// 呼叫各類api
        /// </summary>
        /// <param name="EnInfo"></param>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        public string UniversalTrade(EncryptInfoModel EnInfo, string tradeType)
        {
            EncryptInfo = EnInfo;
            Result = CheckParams();
            if (Result.Success)
            {
                try
                {
                    switch (tradeType)
                    {
                        case "upp":// 交易建立 整合式支付頁
                            break;
                        case "atm":// 交易建立 虛擬帳號幕後
                            break;
                        case "cvs":// 交易建立 超商代碼幕後
                            if (string.IsNullOrEmpty(EncryptInfo.MerTradeNo))
                            {
                                Result.Message = "MerTradeNo is not setting";
                            }
                            if (string.IsNullOrEmpty(EncryptInfo.TradeAmt))
                            {
                                Result.Message = "TradeAmt is not setting";
                            }
                            break;
                        case "credit":// 交易建立 信用卡幕後
                            if (string.IsNullOrEmpty(EncryptInfo.MerTradeNo))
                            {
                                Result.Message = "MerTradeNo is not setting";
                            }
                            if (string.IsNullOrEmpty(EncryptInfo.TradeAmt))
                            {
                                Result.Message = "TradeAmt is not setting";
                            }
                            if (string.IsNullOrEmpty(EncryptInfo.CardNo))
                            {
                                Result.Message = "CardNo is not setting";
                            }
                            if (string.IsNullOrEmpty(EncryptInfo.CardCVC))
                            {
                                Result.Message = "CardCVC is not setting";
                            }
                            break;
                        case "trade_close":// 交易請退款
                            if (string.IsNullOrEmpty(EncryptInfo.TradeNo))
                            {
                                Result.Message = "TradeNo is not setting";
                            }
                            if (string.IsNullOrEmpty(EncryptInfo.CloseType))
                            {
                                Result.Message = "CloseType is not setting";
                            }
                            break;
                        case "trade_cancel":// 交易取消授權
                            if (string.IsNullOrEmpty(EncryptInfo.TradeNo))
                            {
                                Result.Message = "TradeNo is not setting";
                            }
                            break;
                        case "credit_bind_cancel":// 信用卡token取消(約定/記憶卡號)
                            if (string.IsNullOrEmpty(EncryptInfo.UseTokenType))
                            {
                                Result.Message = "UseTokenType is not setting";
                            }
                            if (string.IsNullOrEmpty(EncryptInfo.BindVal))
                            {
                                Result.Message = "BindVal is not setting";
                            }
                            break;
                        case "trade_refund_icash":// 愛金卡退款(ICASH)
                            if (string.IsNullOrEmpty(EncryptInfo.TradeNo))
                            {
                                Result.Message = "TradeNo is not setting";
                            }
                            if (string.IsNullOrEmpty(EncryptInfo.TradeAmt))
                            {
                                Result.Message = "TradeAmt is not setting";
                            }
                            break;
                        case "trade_query":// 交易查詢
                            break;
                        case "credit_bind_query":// 信用卡token查詢(約定)
                            break;
                        default:
                            Result.Message = "Unknown params";
                            break;
                    }

                    if (string.IsNullOrEmpty(Result.Message))
                    {
                        SetParams(contrast(tradeType));

                        if (tradeType == "upp")
                        {
                            return HtmlApi();
                        }
                        else
                        {
                            string CurlResult = CurlApi();
                            return JsonConvert.SerializeObject(ResultProcess(CurlResult));
                        }
                    }
                    else {
                        Result.Success = false;
                        return JsonConvert.SerializeObject(Result);
                    }
                   
                }
                catch (Exception e)
                {
                    Result.Success = false;
                    Result.Message = e.Message;
                    return JsonConvert.SerializeObject(Result);
                }
            }
            else
            {
                return JsonConvert.SerializeObject(Result);
            }
        }

        /// <summary>
        /// 處理api回傳的結果
        /// </summary>
        /// <param name="CurlResult"></param>
        /// <returns></returns>
        public ResultModel ResultProcess(string CurlResult)
        {                      
            ResultModel resultArr = new ResultModel();
            resultArr.Success = false;
            try
            {
                ParameterModel resultParam = new ParameterModel();
                resultParam = JsonConvert.DeserializeObject<ParameterModel>(CurlResult);
                if (!string.IsNullOrEmpty(resultParam.EncryptInfo))
                {
                    if (!string.IsNullOrEmpty(resultParam.HashInfo))
                    {
                        string chkHash = Hash(resultParam.EncryptInfo);
                        if (chkHash != resultParam.HashInfo)
                        {
                            resultArr.Message = "Hash mismatch";
                            return resultArr;
                        }
                        resultArr.Message = Decrypt(resultParam.EncryptInfo);
                        resultArr.Success = true;
                    }
                    else
                    {
                        resultArr.Message = "missing HashInfo";
                    }
                }
                else
                {
                    resultArr.Message = "missing EncryptInfo";
                }
                return resultArr;
            }
            catch
            {
                resultArr.Message = "Result must be an array";
                return resultArr;
            }            
        }

        /// <summary>
        /// 前景呼叫
        /// </summary>
        /// <returns></returns>
        private string HtmlApi()
        {
            string htmlprint = string.Empty;
            htmlprint += "<html><body onload='document.getElementById(\"upp\").submit();'>";
            htmlprint += "<form action='" + ApiUrl + "' method='post' id='upp'>";
            htmlprint += "<input name='MerID' type='hidden' value='" + Parameter.MerID + "' />";
            htmlprint += "<input name='Version' type='hidden' value='" + Parameter.Version + "' />";
            htmlprint += "<input name='EncryptInfo' type='hidden' value='" + Parameter.EncryptInfo + "' />";
            htmlprint += "<input name='HashInfo' type='hidden' value='" + Parameter.HashInfo + "' />";
            htmlprint += "</form></body></html>";
            return htmlprint;
        }

        /// <summary>
        /// CURL
        /// </summary>
        /// <returns></returns>
        private string CurlApi()
        {
            string parame = GetQueryString(Parameter);
            byte[] postData = Encoding.UTF8.GetBytes(parame);

            HttpWebRequest request = HttpWebRequest.Create(ApiUrl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 1000;
            request.ContentLength = postData.Length;
            // 寫入 Post Body Message 資料流
            using (Stream st = request.GetRequestStream())
            {
                st.Write(postData, 0, postData.Length);
            }
            string result = "";
            // 取得回應資料
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        /// <summary>
        /// 轉換為網址
        /// </summary>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        private string contrast(string tradeType)
        {
            switch (tradeType)
            {
                case "trade_query":
                    tradeType = "trade/query";
                    break;
                case "trade_close":
                    tradeType = "trade/close";
                    break;
                case "trade_cancel":
                    tradeType = "trade/cancel";
                    break;
                case "credit_bind_query":
                    tradeType = "credit_bind/query";
                    break;
                case "credit_bind_cancel":
                    tradeType = "credit_bind/cancel";
                    break;
                case "trade_refund_icash":
                    tradeType = "trade/common/refund/icash";
                    break;
            }
            return tradeType;
        }
        /// <summary>
        /// 設定要curl的參數
        /// </summary>
        /// <param name="type"></param>
        private void SetParams(string type = "")
        {
            Plain = GetQueryString(EncryptInfo);
            Parameter.MerID = EncryptInfo.MerID;
            Parameter.Version = "1.0";
            Parameter.EncryptInfo = Encrypt();
            Parameter.HashInfo = Hash(Parameter.EncryptInfo);

            ApiUrl = ApiUrl + type;
        }
        /// <summary>
        /// 檢查必填參數是否存在
        /// </summary>
        /// <returns></returns>
        private ResultModel CheckParams()
        {
            Result = new ResultModel();
            Result.Success = false;
            if (string.IsNullOrEmpty(MerKey))
            {
                Result.Message = "key is not setting";
                return Result;
            }

            if (string.IsNullOrEmpty(MerIV))
            {
                Result.Message = "iv is not setting";
                return Result;
            }
            try
            {
                if (string.IsNullOrEmpty(EncryptInfo.MerID))
                {
                    Result.Message = "MerID is not setting";
                    return Result;
                }
                if (string.IsNullOrEmpty(EncryptInfo.Timestamp.ToString()))
                {
                    Result.Message = "Timestamp is not setting";
                    return Result;
                }
                Result.Success = true;
                return Result;
            }
            catch (Exception e)
            {
                Result.Message = e.Message;
                return Result; ;
            }
        }

        /// <summary>
        /// 轉換QueryString
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetQueryString(Object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            string queryString = String.Join("&", properties.ToArray());
            return queryString;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <returns></returns>
        private string Encrypt()
        {
            if (string.IsNullOrEmpty(Plain))
            {
                return Plain;
            }
            //參數設定
            var tagLength = 16;
            var key = Encoding.UTF8.GetBytes(MerKey);
            var iv = Encoding.UTF8.GetBytes(MerIV);
            var plaintextData = Encoding.UTF8.GetBytes(Plain);
            var encryptedTagData = new byte[plaintextData.Length + tagLength];
            Byte[] encrypted = new Byte[plaintextData.Length];
            Byte[] tag = new Byte[tagLength];
            //加密設定
            var cipher = new GcmBlockCipher(new AesEngine());
            var keyParameters = new AeadParameters(new KeyParameter(key), tagLength * 8, iv);
            cipher.Init(true, keyParameters);
            var offset = cipher.ProcessBytes(plaintextData, 0, plaintextData.Length, encryptedTagData, 0);
            //加密:密文+tag
            cipher.DoFinal(encryptedTagData, offset);
            //分解密文和tag
            Array.Copy(encryptedTagData, encrypted, plaintextData.Length);
            Array.Copy(encryptedTagData, plaintextData.Length, tag, 0, tagLength);

            return bin2hex(Encoding.UTF8.GetBytes(Convert.ToBase64String(encrypted) + ":::" + Convert.ToBase64String(tag))).Trim();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptStr"></param>
        /// <returns></returns>
        private string Decrypt(string encryptStr)
        {
            if (string.IsNullOrEmpty(encryptStr))
            {
                return encryptStr;
            }
            //參數設定
            encryptStr = Encoding.UTF8.GetString(hex2bin(encryptStr));
            var key = Encoding.UTF8.GetBytes(MerKey);
            var iv = Encoding.UTF8.GetBytes(MerIV);
            string[] spliter = { ":::"};
            string[] data = encryptStr.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
            Byte[] encryptData = Convert.FromBase64String(data[0]);
            Byte[] tagData = Convert.FromBase64String(data[1]);
            //組成密文:密文+tag
            Byte[] plainData = new Byte[encryptData.Length + tagData.Length];
            Array.Copy(encryptData, plainData, encryptData.Length);
            Array.Copy(tagData, 0, plainData, encryptData.Length, tagData.Length);
            var result = new Byte[encryptData.Length + tagData.Length];
            //解密設定
            var keyParameters = new AeadParameters(new KeyParameter(key), tagData.Length * 8, iv);
            var cipher = new GcmBlockCipher(new AesEngine());
            cipher.Init(false, keyParameters);
            var offset = cipher.ProcessBytes(plainData, 0, plainData.Length, result, 0);

            cipher.DoFinal(result, offset);

            return Encoding.UTF8.GetString(result);
        }

        /// <summary>
        /// Hash
        /// </summary>
        /// <param name="encryptStr"></param>
        /// <returns></returns>
        private string Hash(string encryptStr = "")
        {
            var hash = SHA256.Create();
            var byteArray = hash.ComputeHash(Encoding.UTF8.GetBytes(MerKey + encryptStr + MerIV));
            return bin2hex(byteArray).ToUpper();
        }

        /// <summary>
        /// 2進位轉16進位
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string bin2hex(byte[] result)
        {
            StringBuilder sb = new StringBuilder(result.Length * 2);
            for (int i = 0; i < result.Length; i++)
            {
                int hight = ((result[i] >> 4) & 0x0f);
                int low = result[i] & 0x0f;
                sb.Append(hight > 9 ? (char)((hight - 10) + 'a') : (char)(hight + '0'));
                sb.Append(low > 9 ? (char)((low - 10) + 'a') : (char)(low + '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 16進位轉2進位
        /// </summary>
        /// <param name="hexstring"></param>
        /// <returns></returns>
        private byte[] hex2bin(string hexstring)
        {
            hexstring = hexstring.Replace(" ", "");
            if ((hexstring.Length % 2) != 0)
            {
                hexstring += " ";
            }
            byte[] returnBytes = new byte[hexstring.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexstring.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }
    }
}
