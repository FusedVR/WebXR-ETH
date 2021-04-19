using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tilia.Interactions.Interactables.Interactables;

public class TokenManager : MonoBehaviour
{
    public EthereumManager ether;
    public InteractableFacade tokenPrefab;

    public Transform spawnLocation;

    public const float TokenValue = 0.1f;

    public TriggerZone zone;

    private List<GameObject> tokens = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(ether.GetAccountBalance(GetAddress(), BalanceCallback));
        ether.onTransactionDelegate += TransactionProcessed;
    }

    void BalanceCallback(float balance) {
        int numTokens = Mathf.FloorToInt(balance / TokenValue);
        StartCoroutine( CreateTokens(numTokens) );
    }

    IEnumerator CreateTokens(int numTokens) {
        for (int i = 0; i < numTokens; i++) {
            tokens.Add( Instantiate(tokenPrefab, spawnLocation.position, spawnLocation.rotation).gameObject );
            yield return new WaitForSeconds(.1f);
        }
    }

    public void SendTransction() {
        int count = zone.GetCount();
        Debug.Log(count);

        ether.TransferRequest(count * TokenValue);
    }

    void TransactionProcessed(string txHash) {
        for (int i = 0; i < tokens.Count; i++) {
            Destroy(tokens[i]);
        }
        tokens.Clear();

        StartCoroutine(ether.GetAccountBalance(GetAddress(), BalanceCallback));
    }

    private string GetAddress() {
        string address = "0x74BAA21278E661eCea04992d5e8fBE6c29cF6f64"; //test address
#if !UNITY_EDITOR
        address = ether.GetAddress();
#endif

        return address;
    }
}
