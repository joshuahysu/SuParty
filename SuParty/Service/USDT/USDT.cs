using Microsoft.Extensions.Options;
using TronNet;
using TronNet.Contracts;
namespace SuParty.Service.USDT
{
    public class USDT
    {
            // 定義私有變數，用於依賴注入
            private readonly ITransactionClient _transactionClient; // 用於與區塊鏈進行交易的客戶端
            private readonly IOptions<TronNetOptions> _options; // 用於存放配置選項（如網路類型等）

 
        // 建構函式，通過依賴注入方式初始化 ITransactionClient 和配置選項
        public USDT(ITransactionClient transactionClient,IOptions<TronNetOptions> options)
        {
            _transactionClient = transactionClient;
            _options = options;
        }
            /// <summary>
            /// 異步方法，用於創建、簽署並廣播 USDT 交易
            /// </summary>
            /// <returns></returns>
            public async Task SignAsync(string from,string to,long amount)
            {
                // 私鑰，用於簽署交易（需妥善保護，避免洩露）
                var privateKey = "D95611A9AF2A2A45359106222ED1AFED48853D9A44DEFF8DC7913F5CBA727366";

                // 使用私鑰創建 TronECKey 物件，並設置網路類型（例如主網或測試網）
                var ecKey = new TronECKey(privateKey, _options.Value.Network);

                // 從私鑰生成公鑰地址，表示交易的發送方
                //var from = ecKey.GetPublicAddress();

                // 定義接收方地址
                //var to = "TGehVcNhud84JDCGrNHKVz9jEAVKUpbuiv";

                // 定義轉帳的數量，這裡是 100 TRX（以 sun 為單位，1 TRX = 1_000_000 sun）
                //var amount = 100_000_000L;

                // 創建交易，將指定金額從發送方地址轉到接收方地址
                var transactionExtension = await _transactionClient.CreateTransactionAsync(from, to, amount);

                // 使用私鑰簽署交易，生成簽名的交易
                var transactionSigned = _transactionClient.GetTransactionSign(transactionExtension.Transaction, privateKey);

                // 廣播簽署後的交易到區塊鏈網路
                var result = await _transactionClient.BroadcastTransactionAsync(transactionSigned);

                // 注意：此處未處理 result 的回傳值，建議根據需要檢查是否成功
            }
    }    
}