mergeInto(LibraryManager.library, {
  GetAccount: function () {
    var account = '';
    if (typeof window.ethereum !== 'undefined' && window.ethereum.isMetaMask) {
        account = window.ethereum.selectedAddress;
        if(typeof account === 'undefined'){
            account = '';
        }
    }
    var buffer = _malloc(lengthBytesUTF8(account) + 1);
    stringToUTF8(account, buffer, account.length + 1);
    return buffer;
  },

  SendTransaction: function (to, data) {
    var tostr = Pointer_stringify(to);
    var wei = parseFloat(Pointer_stringify(data)) * Math.pow(10, 18);
    console.log(wei);
    var from = "";

    ethereum.request({ method: 'eth_accounts' }).then(function(response){
      from = response[0];
      var args = {
          "from": from,
          "to": tostr,
          "value": wei.toString(16) //convert to hex
          //"data": datastr
        };

        ethereum.request({ method: 'eth_sendTransaction', params : [args], });

    });


  },
});
