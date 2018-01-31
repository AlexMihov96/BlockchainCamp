const bitcoin = require('bitcoinjs-lib')

let transaction = new bitcoin.TransactionBuilder()
let transactionId = 'aa94ab02c182214f090e99a0d57021caffd0f195a81c24502b1028b130b63e31'

transaction.addInput(transactionId,0)

let publicKey = '1Gokm82v6DmtwKEB8AiVhm82hyFSsEvBDK'
let satoshies = 15000
transaction.addOutput(publicKey, satoshies)

let privateKeyWIF = 'L1uyy5qTuGrVXrmrsvHWHgVzW9kKdrp27wBC7Vs6nZDTF2BRUVwy'
let keyPair = bitcoin.ECPair.fromWIF(privateKeyWIF)

transaction.sign(0,keyPair)

console.log(transaction.build().toHex())