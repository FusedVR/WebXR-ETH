using System.Collections;
using System.Runtime.InteropServices;
using Nethereum.Web3;
using Nethereum.JsonRpc.UnityClient;
using UnityEngine;
using Nethereum.RPC.Eth.DTOs;

public class EthereumManager : MonoBehaviour {

    public string Url = "https://ropsten.infura.io/v3/7aab67d98b0b42f181905b62c2025d9a";
    public string AddressTo = "0x7336bb564f9007A1f0A06985255B0Ce0F44BDB3D";

    [DllImport("__Internal")]
    private static extern string GetAccount();

    [DllImport("__Internal")]
    private static extern string SendTransaction(string to, string data);

    public void TransferRequest(float amount) {
        SendTransaction(AddressTo, amount.ToString());
    }

    public string GetAddress() {
        return GetAccount();
    }

    public IEnumerator GetAccountBalance(string account , System.Action<float> callback) {
        var balanceRequest = new EthGetBalanceUnityRequest(Url);
        yield return balanceRequest.SendRequest(account, BlockParameter.CreateLatest());
        var ether = Web3.Convert.FromWei(balanceRequest.Result.Value);
        if (callback != null) callback( decimal.ToSingle(ether) );
    }
}
