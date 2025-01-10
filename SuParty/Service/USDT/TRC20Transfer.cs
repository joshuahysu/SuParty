using TronNet;
using TronNet.Contracts;

namespace SuParty.Service.USDT
{
    // 定義 TRC20Transfer 類別，負責進行 TRC20 (例如 USDT) 轉帳操作
    public class TRC20Transfer
    {
        // 私有成員變數，用來存取錢包與合約客戶端工廠
        private readonly IWalletClient _wallet;  // 用來處理錢包相關操作，如從私鑰取得帳戶
        private readonly IContractClientFactory _contractClientFactory; // 用來創建合約客戶端，處理合約交易

        // 構造函數，注入依賴的錢包與合約客戶端工廠
        public TRC20Transfer(IWalletClient wallet, IContractClientFactory contractClientFactory)
        {
            _wallet = wallet;  // 初始化錢包客戶端
            _contractClientFactory = contractClientFactory;  // 初始化合約客戶端工廠
        }

        // 非同步方法：進行 TRC20 轉帳
        public async Task TransferAsync()
        {
            // 私鑰，代表發送者的身份，用來簽署交易
            var privateKey = "8e812436a0e3323166e1f0e8ba79e19e217b2c4a53c970d4cca0cfb1078979df";

            // 從私鑰生成帳戶，帳戶中包含了發送者的地址與其他資訊
            var account = _wallet.GetAccount(privateKey);

            // TRC20 合約地址，這裡使用的是 USDT 合約地址
            var contractAddress = "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t"; //USDT Contract Address

            // 目標地址，表示轉帳的收款人
            var to = "TGehVcNhud84JDCGrNHKVz9jEAVKUpbuiv";

            // 轉帳的 USDT 數量，注意 TRC20 通常有 6 位小數
            var amount = 10; //USDT Amount

            // 轉帳所需的手續費，這裡的費用是 5 USDT，轉換為最低單位 (1 USDT = 1000000)
            var feeAmount = 5 * 1000000L;

            // 使用合約客戶端工廠創建一個 TRC20 合約的客戶端
            var contractClient = _contractClientFactory.CreateClient(ContractProtocol.TRC20);

            // 呼叫 TransferAsync 方法進行 TRC20 轉帳
            // 這個方法會進行必要的簽名、發送交易並返回結果
            var result = await contractClient.TransferAsync(contractAddress, account, to, amount, string.Empty, feeAmount);

            // 結果可以根據需要進一步處理，例如檢查交易是否成功
        }
    }

}