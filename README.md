# 目錄
* [環境需求](#環境需求)
* [安裝](#安裝)
* [使用方式](#使用方式)
# 環境需求
* .NET Framework 4.7.2
# 安裝
* 請加入PayuniSDK參考至專案
* 請至NuGet安裝Newtonsoft.Json
```clike
NuGet\Install-Package Newtonsoft.Json -Version 13.0.1
```
* 請至NuGet安裝Portable.BouncyCastle
```clike
NuGet\Install-Package Portable.BouncyCastle -Version 1.9.0
```

# 使用方式
* 正式區
```csharp
payuniAPI payuniapi = new payuniAPI(key, iv);
```
* 測試區
```csharp
payuniAPI payuniapi = new payuniAPI(key, iv, type);
```
* API串接
```csharp
string result = payuniApi.UniversalTrade(encryptInfo, mode);
```
* upp ReturnURL、NotifyURL接收到回傳參數後處理方式
```csharp
string result = payuniApi.ResultProcess(requestData);
```
* 參數說明
  * EncryptInfoModel encryptInfo
    * 參數詳細內容請參考[統一金流API串接文件](https://www.payuni.com.tw/docs/web/#/7/34)對應功能請求參數的EncryptInfo
```csharp=
EncryptInfoModel encryptInfo = new EncryptInfoModel();
encryptInfo.MerID = "ABC";
encryptInfo.Timestamp= DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
```
  * string merKey
    * 請登入PAYUNi平台檢視商店串接資訊取得 Hash Key
  * string merIV
    * 請登入PAYUNi平台檢視商店串接資訊取得 Hash IV
  * string type (非必填)
    * 連線測試區 => t
    * 連線正式區 => 不給該參數或給空值
  * string mode
    * 整合式支付頁 => upp
    * 虛擬帳號幕後 => atm
    * 超商代碼幕後 => cvs
    * 信用卡幕後　 => credit
    * 交易查詢　　 => trade_query
    * 交易請退款　 => trade_close
    * 交易取消授權 => trade_cancel
    * 信用卡Token(約定) => credit_bind_query
    * 信用卡Token取消(約定/記憶卡號) => credit_bind_cancel
    * 愛金卡退款(ICASH) => trade_refund_icash
* 其餘請參考[範例](https://github.com/payuni/NET_SDK/tree/main/example)

* 原生C#
```csharp=
using PayuniSDK;
string merKey = "12345678901234567890123456789012";
string merIV = "1234567890123456";
payuniAPI payuni = new payuniAPI(key, iv);
EncryptInfoModel encryptInfo = new EncryptInfoModel();
encryptInfo.MerID = "ABC";
encryptInfo.TradeNo = "16614190477810373246";
encryptInfo.Timestamp= DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
string result = payuniApi.UniversalTrade(encryptInfo, "trade_query");
```
# LICENSE
```text
Copyright 2022 PRESCO. All rights reserved.
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```
