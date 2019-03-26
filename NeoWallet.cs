using Neo.Lux.Core;
using Neo.Lux.Cryptography;
using Neo.Lux.Utils;
using System;
using System.Text;

namespace Neo.Lux.Wallet
{
    public class NeoWallet
    {
        public static KeyPair Mnemonickeypair(string menimonic)
        {
            var getmenimonicBytes = Encoding.ASCII.GetBytes(menimonic);
            var lengthofmenimonic = getmenimonicBytes.Length;

            byte[] data = new byte[lengthofmenimonic];
            data[0] = 0x80;
            Buffer.BlockCopy(getmenimonicBytes, 0, data, 1, lengthofmenimonic - 2);
            data[lengthofmenimonic - 1] = 0x01;
            string wif = data.Base58CheckEncode();
            Array.Clear(data, 0, data.Length);

            if (wif == null) throw new ArgumentNullException();
            byte[] data1 = wif.Base58CheckDecode();
            if (data1.Length != lengthofmenimonic || data1[0] != 0x80 || data1[lengthofmenimonic - 1] != 0x01)
                throw new FormatException();
            byte[] PrivateKeyOfmenimonic = new byte[32];
            Buffer.BlockCopy(data1, 1, PrivateKeyOfmenimonic, 0, PrivateKeyOfmenimonic.Length);
            Array.Clear(data1, 0, data1.Length);

            KeyPair menimonickeypair = new KeyPair(PrivateKeyOfmenimonic);


            var privatekeyBytes = menimonickeypair.PrivateKey;
            var pubKeyBytes = menimonickeypair.PublicKey;
            var privateKey = Base58.Encode(privatekeyBytes);
            var pubKey = Base58.Encode(pubKeyBytes);
            var sc = KeyPair.CreateSignatureScript(privatekeyBytes);
            var address = menimonickeypair.address;
            Console.WriteLine("\n In Function ................. : Private Key: " + privateKey + "\nPublic key:" + pubKey + "\n Address :" + address);
            return menimonickeypair;
        }

        public static void LatestMainNet(string add)
        {
            RemoteRPCNode api;

            try
            {
                api = new RemoteRPCNode("http://seed9.ngd.network:10332", "https://seed1.switcheo.network:10331", "http://seed10.ngd.network:10332", "http://nep5.otcgo.cn:10332", "http://seed6.ngd.network:10332", "http://seed4.neo.org:10332", "http://neoscan.io", "http://seed6.ngd.network:10332", "http://seed.neoeconomy.io:10332");
                Console.WriteLine("*Syncing balances From Main Net ...");

            }
            catch (Exception)
            {
                Console.WriteLine("Nodes not responding ....... , Try later ");
                throw;
            }
            var balances = api.GetAssetBalancesOf(add);
            foreach (var entry in balances)
            {
                Console.WriteLine(entry.Value + " " + entry.Key);
            }

        }

        public static void LatestTestNet(string add)
        {
            RemoteRPCNode api;

            try
            {
                api = new RemoteRPCNode("http://seed5.neo.org:20332", "https://test2.cityofzion.io:443", "http://seed3.ngd.network:20332", "http://seed4.ngd.network:20332", "http://seed1.ngd.network:20332", "https://neoscan-testnet.io:20332");
                Console.WriteLine("*Syncing balances  From Test Net ...");

            }
            catch (Exception)
            {
                Console.WriteLine("Nodes not responding ....... , Try later ");
                throw;
            }
            var balances = api.GetAssetBalancesOf(add);
            foreach (var entry in balances)
            {
                Console.WriteLine(entry.Value + " " + entry.Key);
            }

        }

        public static void transection(KeyPair pair, string toaddress, string symbol, decimal amt)
        {
            RemoteRPCNode api;
            try
            {
                api = new RemoteRPCNode("http://seed5.neo.org:20332", "https://test2.cityofzion.io:443", "http://seed3.ngd.network:20332", "http://seed4.ngd.network:20332", "http://seed1.ngd.network:20332", "https://neoscan-testnet.io:20332");
                // 
                var result = api.SendAsset(pair /*destination address*/, toaddress, symbol/* can be another */, amt /*amount to send */ );
                Console.WriteLine("Transection Succeed");
            }
            catch (Exception)
            {
                Console.WriteLine("Transection Failed");
                throw;
            }


        }
    }
}
