using System;
using System.Text;
using System.Threading;
using org.openstars.core.bigset.Generic;
using ThriftPoolDotNet;
namespace BigSetServices
{
    public class TStringBSKVService
    {
        private static SetClient _setClient;
        private static TStringBigSetKVService.Client _aClient;
        private static readonly object _object = new object();
        public TStringBSKVService(SetClient setClient)
        {
            _setClient = setClient;
        }
        public bool BsgPutItem(string key, TItem item)
        {
            lock (_object)
            {
                TClientInfo clientInfo = _setClient.getClient();
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
                
                _aClient = (TStringBigSetKVService.Client) clientInfo.getClient();
                
                var bsPutItemAsync = _aClient.bsPutItemAsync(key, item);
                
                if (bsPutItemAsync.IsCompleted == false)
                {
                    bsPutItemAsync.Wait();
                    if (bsPutItemAsync.IsCompleted == false)
                    {
                        return false;
                    }
                }

                clientInfo.cleanUp();
                return true;
            }
        }

        public bool BsgMultiPutItem(string[] keys, TItem item)
        {
            lock (_object)
            {
                TClientInfo clientInfo = _setClient.getClient();
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

                _aClient = (TStringBigSetKVService.Client) clientInfo.getClient();

                foreach (var key in keys)
                {
                    var bsPutItemAsync = _aClient.bsPutItemAsync(key, item);
                    if (bsPutItemAsync.IsCompleted == false)
                    {
                        bsPutItemAsync.Wait();
                        if (bsPutItemAsync.IsCompleted == false)
                        {
                            return false;
                        }
                    }
                }
                clientInfo.cleanUp();
                return true;
            }
        }
        
        
        // BsGetItem get item by bskey and itemkey
        public bool BsGetItem(string bsKey,string key,ref TItem item)
        {
            lock (_object)
            {
                TClientInfo clientInfo = _setClient.getClient();
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
                
                _aClient = (TStringBigSetKVService.Client) clientInfo.getClient();

                var bsGetItemAsync = _aClient.bsGetItemAsync(bsKey,Encoding.ASCII.GetBytes(key));
                if (bsGetItemAsync.IsCompleted == false)
                {
                    bsGetItemAsync.Wait();
                    if (bsGetItemAsync.IsCompleted == false)
                    {
                        Console.WriteLine("bsGetItemAsync.IsCompleted == false");
                        return false;
                    }
                }

                if (bsGetItemAsync.Result.Item == null)
                {
                    Console.WriteLine("Item not exists");
                    return false;
                }
                item = bsGetItemAsync.Result.Item;
                
                clientInfo.cleanUp();
                return true;

            }
        }
        
        // BsRemoveItem remove item by bskey and itemkey
        public bool BsRemoveItem(string bsKey, string key)
        {
            lock (_object)
            {
                TClientInfo clientInfo = _setClient.getClient();
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
                
                _aClient = (TStringBigSetKVService.Client) clientInfo.getClient();
                TItem item = new TItem();
                if (BsGetItem(bsKey, key, ref item) == false)
                {
                    Console.WriteLine("Item not exists");
                    return false;
                }
                var bsRemoveItemAsync = _aClient.bsRemoveItemAsync(bsKey,Encoding.ASCII.GetBytes(key));
                return true;    
            }
        }
    }
}