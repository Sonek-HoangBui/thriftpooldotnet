using System;
using System.Collections.Generic;

using Thrift.Protocol;

namespace ThriftPoolDotNet
{
   public class ClientFactory
    {
        private static Dictionary<string, Stack<TClientInfo>> m_clients = new Dictionary<string, Stack<TClientInfo>>();
       private static Dictionary<string, TProtocolFactory> m_factories = new Dictionary<string, TProtocolFactory>();
        // static ReentrantLock m_lock = new ReentrantLock();
        static readonly object syncLock = new object();
        
        public ClientFactory() {
        }

        public static int getCountTotalClient()
        {
            if (m_clients != null) return m_clients.Count;
            else return 0;
        }
        public static void setFactory(String host, int port,  Object clientClass, TProtocolFactory protocolFactory) {
            lock (syncLock)
            {
                m_factories.Add(getKey(host, port, clientClass), protocolFactory);
            }
        }

        public static TProtocolFactory getProtocolFactory(String host, int port,  Object  clientClass) {
            String aKey = getKey(host, port, clientClass);
            lock (syncLock)
            {
                TProtocolFactory aResult = m_factories.GetValueOrDefault(aKey);
                return aResult;

            }
        }

        private static String getKey(String host, int port, Object clientClass) {
            return clientClass + "_" + host + port.ToString();
        }

        public static TClientInfo fastGetClient(String host, int port,  Object  clientClass) {
            lock (syncLock)
            {
                // Dictionary<string, Stack<TClientInfo>> m_clients = new Dictionary<string, Stack<TClientInfo>>();
                
                String aKey = getKey(host, port, clientClass);
                Stack<TClientInfo> aContainer = m_clients.GetValueOrDefault(aKey);
                TClientInfo aResult;
                if (aContainer == null) {
                    Console.WriteLine("Container == null");
                    aResult = null;
                } else if (aContainer.Count == 0) {
                    Console.WriteLine("aContainer.Count == 0");
                    aResult = null;
                } else {
                    aResult = aContainer.Pop();
                    //Console.WriteLine("ngon ");
                }
                return aResult;
            }
            
        }

        public static TClientInfo getClient(String host, int port, Object clientClass, Object protocolClass) {
            TClientInfo aInfo = fastGetClient(host, port, clientClass);
            if (aInfo != null) {
                //Console.WriteLine("I1");
                return aInfo;
            } else {
                //Console.WriteLine("I2");
                aInfo = new TClientInfo(host, port, clientClass, protocolClass);
                return aInfo;
            }
        }

        public static void releaseClient(TClientInfo aClientInfo) {
            lock (syncLock)
            {
                String aKey = getKey(aClientInfo.m_host, aClientInfo.m_port, aClientInfo.m_clientClass);
                Stack<TClientInfo> aContainer = m_clients.GetValueOrDefault(aKey);
                if (aContainer == null) {
                    //Console.WriteLine("I5");
                    aContainer = new Stack<TClientInfo>();
                    aContainer.Push(aClientInfo);
                    m_clients.Add(aKey, aContainer);
                } else {
                    //Console.WriteLine("I6");
                    aContainer.Push(aClientInfo);
                }

                //Console.WriteLine("aContainer.Count: " + aContainer.Count);
            }
            
        }

        public static TClientInfo getClient(string mHost, in int mPort, object clientClass)
        {
            throw new NotImplementedException();
        }
    }
}