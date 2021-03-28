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

    // Start is called before the first frame update
    void Start() {
        string address = "0x74BAA21278E661eCea04992d5e8fBE6c29cF6f64"; //test address
#if !UNITY_EDITOR
        address = ether.GetAddress();
#endif
        StartCoroutine(ether.GetAccountBalance(address, BalanceCallback));
    }

    void BalanceCallback(float balance) {
        Debug.LogError(balance);
        int numTokens = Mathf.FloorToInt(balance / TokenValue);
        StartCoroutine( CreateTokens(numTokens) );
    }

    IEnumerator CreateTokens(int numTokens) {
        for (int i = 0; i < numTokens; i++) {
            Instantiate(tokenPrefab, spawnLocation.position, spawnLocation.rotation);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void SendTransction() {
        int count = zone.GetCount();
        Debug.Log(count);

        ether.TransferRequest(count * TokenValue);
    }
}
