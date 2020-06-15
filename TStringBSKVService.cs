using System;
using System.Threading;
using org.openstars.core.bigset.Generic;

namespace ThriftPoolDotNet
{
    public class TStringBSKVService
    {
        private static BigSetClient _bigSetClient;
        private static TStringBigSetKVService.Client _aClient;
        private static readonly object _object = new object();
        public TStringBSKVService(BigSetClient bigSetClient)
        {
            _bigSetClient = bigSetClient;
        }
        public bool BsgPutItem(string key, TItem item)
        {
            lock (_object)
            {
                TClientInfo clientInfo = _bigSetClient.getClient();
                if (!clientInfo.isOpen())
                {
                    clientInfo.doOpen();
                }

                if (!clientInfo.sureOpen())
                {
                    Thread.Sleep(1 * 100);
                    if (!clientInfo.sureOpen())
                    {
                        Console.WriteLine("Can't open");
                        return false;
                    }
                }

                object tmp = clientInfo.getClient();
                _aClient = (TStringBigSetKVService.Client) tmp;
                var bsPutItemAsync = _aClient.bsPutItemAsync(key, item);

                if (bsPutItemAsync.IsCompleted == false)
                {
                    bsPutItemAsync.Wait();
                }

                clientInfo.cleanUp();
                return true;
            }
        }
    }
}