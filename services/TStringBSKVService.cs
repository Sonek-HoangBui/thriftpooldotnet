using System;
using System.Collections.Generic;
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
                if (bsGetItemAsync.IsCanceled || bsGetItemAsync.IsCanceled)
                {
                    clientInfo.cleanUp();
                    return false;
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
                if (bsRemoveItemAsync.IsCanceled || bsRemoveItemAsync.IsFaulted)
                {
                    clientInfo.cleanUp();
                    return false;
                }
                clientInfo.cleanUp();
                return true;    
            }
        }

        // BsGetSlice get all
        public bool BsGetSlice(string bsKey,ref TItemSetResult itemSetResult){
            lock(_object){
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
                
                var getTotalCountAsync = _aClient.getTotalCountAsync(bsKey);
                if (getTotalCountAsync.IsCanceled || getTotalCountAsync.IsFaulted)
                {
                    Console.WriteLine("getTotalCountAsync.IsCanceled || getTotalCountAsync.IsFaulted");
                    clientInfo.cleanUp();
                    return false;
                }
                long totalCount = getTotalCountAsync.Result;
                //Console.WriteLine("Count "+totalCount);
                int count = Convert.ToInt32(totalCount);
                if (count < 1)
                {
                    clientInfo.cleanUp();
                    return false;
                }
                var bsGetSliceRAsync = _aClient.bsGetSliceRAsync(bsKey,0,count);
                if (bsGetSliceRAsync.IsCanceled || bsGetSliceRAsync.IsFaulted)
                {
                    Console.WriteLine("bsGetSliceRAsync.IsCanceled || bsGetSliceRAsync.IsFaulted");
                    clientInfo.cleanUp();
                    return false;
                }
                itemSetResult = bsGetSliceRAsync.Result;
                
                clientInfo.cleanUp();
                return true;
            }
        }

        
        
        // BsRangeQuery get >= startkey && <= endkey
        public bool BsRangeQuery(string bsKey,string startItemKey,string endItemKey,ref TItemSetResult itemSetResult)
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

                byte[] BstartItemKey;
                byte[] BendItemKey;
                try
                {
                     BstartItemKey = Encoding.UTF8.GetBytes(startItemKey);
                     BendItemKey = Encoding.UTF8.GetBytes(endItemKey);
                }
                catch (Exception e)
                {
                    Console.WriteLine("error "+e.ToString());
                    clientInfo.cleanUp();
                    return false;
                }
                

                var bsRangeQueryAsync = _aClient.bsRangeQueryAsync(bsKey,BstartItemKey  ,BendItemKey);
                if (bsRangeQueryAsync.IsCanceled || bsRangeQueryAsync.IsFaulted)
                {
                    Console.WriteLine("bsRangeQueryAsync.IsCanceled || bsRangeQueryAsync.IsFaulted");
                    clientInfo.cleanUp();
                    return false;
                }
                
                
                itemSetResult = bsRangeQueryAsync.Result;
                if (itemSetResult.Items.Items.Count < 1)
                {
                    clientInfo.cleanUp();
                    return false;
                }
                clientInfo.cleanUp();
                return true;
                
            }
        }
        
        // BsGetSliceByArray get by position begin -> end
        public bool BsGetSliceByArray(string bsKey,int begin,int count,ref TItemSetResult itemSetResult)
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
            var bsGetSliceRAsync = _aClient.bsGetSliceRAsync(bsKey, begin, count);
            if (bsGetSliceRAsync.IsCanceled || bsGetSliceRAsync.IsFaulted)
            {
                Console.WriteLine("bsGetSliceRAsync.IsCanceled || bsGetSliceRAsync.IsFaulted");
                clientInfo.cleanUp();
                return false;
            }
            if (bsGetSliceRAsync.Result.Items.Items.Count < 1)
            {
                clientInfo.cleanUp();
                return false;
            }
            itemSetResult = bsGetSliceRAsync.Result;
            clientInfo.cleanUp();
            return true;
        }

        //g// GetTotalCount return number item of bskey
        public bool GetTotalCount(string bsKey,ref long count)
        {
            lock (_object)
            {
                count = 0;
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

                var getTotalCountAsync = _aClient.getTotalCountAsync(bsKey);
                if (getTotalCountAsync.IsFaulted || getTotalCountAsync.IsCanceled)
                {
                    Console.WriteLine("getTotalCountAsync.IsFaulted");
                    clientInfo.cleanUp();
                    return false;
                }

                long totalCount = getTotalCountAsync.Result;
                if (totalCount < 1)
                {
                    clientInfo.cleanUp();
                    return false;
                }
                count = totalCount;
                clientInfo.cleanUp();
                return true;
            }
        }
        
        // BsRangeQueryByPage get >= startkey && <= endkey có chia page theo begin and end==============================
        public bool BsRangeQueryByPage(string bsKey,string startItemKey,string endItemKey,long begin,long end,
            ref TItemSetResult itemSetResult,ref long total)
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
                
                byte[] BstartItemKey;
                byte[] BendItemKey;
                try
                {
                    BstartItemKey = Encoding.UTF8.GetBytes(startItemKey);
                    BendItemKey = Encoding.UTF8.GetBytes(endItemKey);
                }
                catch (Exception e)
                {
                    Console.WriteLine("error "+e.ToString());
                    clientInfo.cleanUp();
                    return false;
                }
                
                var bsRangeQueryAsync = _aClient.bsRangeQueryAsync(bsKey,BstartItemKey,BendItemKey);

                if (bsRangeQueryAsync.Result.Items.Items.Count < 1)
                {
                    clientInfo.cleanUp();
                    return false;
                }

                if (begin < 0)
                {
                    begin = 0;
                }

                if (end > bsRangeQueryAsync.Result.Items.Items.Count)
                {
                    end = bsRangeQueryAsync.Result.Items.Items.Count;
                }

                total = bsRangeQueryAsync.Result.Items.Items.Count;

                itemSetResult.Items.Items.CopyTo(Convert.ToInt32(begin),bsRangeQueryAsync.Result.Items.Items.ToArray(),0,Convert.ToInt32(end-begin));
                clientInfo.cleanUp();
                return false;

            }
        }
        
        
        //CheckBsExisted================================================================================================
        public bool CheckBsExisted(string bsKey,string itemKey)
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
                byte[] bitemKey;
                try
                {
                    bitemKey =  Encoding.UTF8.GetBytes(itemKey);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    clientInfo.cleanUp();
                    return false;

                }
                var bsExistedAsyncs = _aClient.bsExistedAsync(bsKey,bitemKey);
                if (bsExistedAsyncs.IsCanceled || bsExistedAsyncs.IsFaulted)
                {
                    Console.WriteLine("bsExistedAsyncs.IsFaulted");
                    clientInfo.cleanUp();
                    return false;
                }
                clientInfo.cleanUp();
                return bsExistedAsyncs.Result.Existed;
            }
        }

        //TotalStringKeyCount ==========================================================================================
        public bool TotalStringKeyCount(ref long count)
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

                var totalStringKeyCountAsync = _aClient.totalStringKeyCountAsync();
                if (totalStringKeyCountAsync.IsFaulted || totalStringKeyCountAsync.IsCanceled)
                {
                    Console.WriteLine("totalStringKeyCountAsync.IsFaulted");
                    count = 0;
                    clientInfo.cleanUp();
                    return false;
                }
                count = totalStringKeyCountAsync.Result;
                clientInfo.cleanUp();
                return true;
            }
        }
        
        //GetListKey ==================================================================================================
        public bool GetListKey(long fromIndex,int count,List<String> listKey)
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
                var getListKeyAsync = _aClient.getListKeyAsync(fromIndex,count);
                if (getListKeyAsync.IsCanceled || getListKeyAsync.IsFaulted)
                {
                    Console.WriteLine("getListKeyAsync.IsCanceled || getListKeyAsync.IsFaulted");
                    clientInfo.cleanUp();
                    return false;
                }

                if (getListKeyAsync.Result.Count < 1)
                {
                    clientInfo.cleanUp();
                    return false;
                }
                listKey = getListKeyAsync.Result;
                clientInfo.cleanUp();
                return true;
            }
        }
        
        //BsGetSliceFromPos=============================================================================================
        public bool BsGetSliceFromPos(string bsKey, int fromPos, int count, ref TItemSetResult tItemSetResult)
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
                var bsGetSlice = _aClient.bsGetSliceAsync(bsKey, fromPos, count);
                if (bsGetSlice.IsCanceled || bsGetSlice.IsFaulted)
                {
                    Console.WriteLine("bsGetSlice.IsCanceled || bsGetSlice.IsFaulted");
                    clientInfo.cleanUp();
                    return false;
                }

                if (bsGetSlice.Result.Items.Items.Count < 1)
                {
                    clientInfo.cleanUp();
                    return false;
                }
                tItemSetResult = bsGetSlice.Result;
                
                clientInfo.cleanUp();
                return true;
            }
        }
        
        //BsRemoveKeyBs=================================================================================================
        public bool BsRemoveKeyBs(string bsKey,ref long totalCount)
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
                var removeAllAsync = _aClient.removeAllAsync(bsKey);
                if (removeAllAsync.IsCanceled || removeAllAsync.IsFaulted)
                {
                    clientInfo.cleanUp();
                    return false;
                }
                totalCount = removeAllAsync.Result;
                clientInfo.cleanUp();
                return true;
            }
        }
    }
}