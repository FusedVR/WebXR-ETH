function checkMetamask(){
    if ( typeof window.ethereum !== 'undefined' && window.ethereum.isMetaMask ) {
      if (ethereum.chainID !== "0x3") { //Ropsten
        document.getElementById("metamaskWarning").innerText = 'Please connect to Rinkeby for testing';
      }
      document.getElementById("btnConnectToMetamask").style.visibility = "hidden";
    } else {
      document.getElementById("metamaskWarning").innerText = 'Please install and connect to Metamask';
      document.getElementById("btnConnectToMetamask").style.visibility = "hidden";
    }
}

async function connectToMetamask(){
  try {
    await window.ethereum.enable();
    checkMetamask();
  } catch (error) {
    // Handle error. Likely the user rejected the login
    console.error(error)
  }
}