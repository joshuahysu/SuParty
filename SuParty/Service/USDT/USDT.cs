using System;
using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TronNet;
using TronNet.Crypto;
namespace SuParty.Service.USDT
{
    public class USDT
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IOptions<TronNetOptions> _options;
        public USDT(ITransactionClient transactionClient, IOptions<TronNetOptions> options)
        {
            _options = options;
            _transactionClient = transactionClient;
        }

        public async Task SignAsync()
        {
            var privateKey = "D95611A9AF2A2A45359106222ED1AFED48853D9A44DEFF8DC7913F5CBA727366";
            var ecKey = new TronECKey(privateKey, _options.Value.Network);
            var from = ecKey.GetPublicAddress();
            var to = "TGehVcNhud84JDCGrNHKVz9jEAVKUpbuiv";
            var amount = 100_000_000L;
            var transactionExtension = await _transactionClient.CreateTransactionAsync(from, to, amount);

            var transactionSigned = _transactionClient.GetTransactionSign(transactionExtension.Transaction, privateKey);

            var result = await _transactionClient.BroadcastTransactionAsync(transactionSigned);
        }
    }
}