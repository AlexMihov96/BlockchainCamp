 const bitcoin = require('bitcoinjs-lib')

let keyPair = bitcoin.ECPair.makeRandom()

let privateKey = keyPair.toWIF()
console.log(privateKey)

let publicKey = keyPair.getAddress()
console.log(publicKey)

