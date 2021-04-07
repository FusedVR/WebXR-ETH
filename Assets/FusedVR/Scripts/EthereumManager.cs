using System.Collections;
using System.Runtime.InteropServices;
using Nethereum.Web3;
using Nethereum.JsonRpc.UnityClient;
using UnityEngine;
using Nethereum.RPC.Eth.DTOs;
using TMPro;

public class EthereumManager : MonoBehaviour {

    public string Url = "https://ropsten.infura.io/v3/7aab67d98b0b42f181905b62c2025d9a";
    public string AddressTo = "0x7336bb564f9007A1f0A06985255B0Ce0F44BDB3D";

    public TextMeshPro mText;

    public delegate void OnTransactionProcessed(string txHash);
    public OnTransactionProcessed onTransactionDelegate;

    [DllImport("__Internal")]
    private static extern string GetAccount();

    [DllImport("__Internal")]
    private static extern void SendTransaction(string to, string data, string returnObj, string returnFunc);

    public void TransferRequest(float amount) {
        Logger("Sending " + amount);
        SendTransaction(AddressTo, amount.ToString(), gameObject.name, "TransactionCallback");
    }

    public string GetAddress() {
        return GetAccount();
    }

    public IEnumerator GetAccountBalance(string account , System.Action<float> callback) {
        var balanceRequest = new EthGetBalanceUnityRequest(Url);
        yield return balanceRequest.SendRequest(account, BlockParameter.CreateLatest());
        var ether = Web3.Convert.FromWei(balanceRequest.Result.Value);
        if (callback != null) callback( decimal.ToSingle(ether) );

        Logger("Balance is " + ether);
    }

    public void TransactionCallback(string txHash) {
        Logger("Got Tx Hash from Unity: " + txHash);
        StartCoroutine(TransactionPolling(txHash));
    }

    private IEnumerator TransactionPolling(string txHash) {
        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(Url);
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(txHash, 2);

        Logger ( "Transaction mined" );

        onTransactionDelegate?.Invoke(txHash);
    }

    private void Logger(string log) {
        mText.text = log;
        Debug.Log(log);
    }
}
