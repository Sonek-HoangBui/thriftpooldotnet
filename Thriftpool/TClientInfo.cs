using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using Thrift.Protocol;
using Thrift.Transport;
using Thrift.Transport.Client;


namespace ThriftPoolDotNet
{
    public class TClientInfo
    { 
      
        public Object m_client;
        public TProtocol m_protocol;
        public TTransport m_transport;
        public TProtocolFactory m_protoFactory;
        public Object m_clientClass;
        public Object m_protocolClass;
        public string m_host;
        public int m_port;
        private long m_createTime = System.DateTime.Now.Millisecond;
        static readonly object syncLock = new object();
        public TClientInfo() {
        }

        // public <T> T getClientT() {
        //     return this.sureOpen() ? this.m_client : null;
        // }

        public TClientInfo(String host, int port, Object clientClass, Object protocolClass) {
            this.m_host = new String(host) ;
            this.m_port = port;
            this.m_clientClass = clientClass;
            this.m_protocolClass = protocolClass;
            this.m_protoFactory = ClientFactory.getProtocolFactory(host, port, clientClass);
            this.doOpen();
        }

        protected TProtocol createProtocol(TTransport aTransport) {
            if (this.m_protocolClass.Equals(typeof(TBinaryProtocol)) ) {
                return new TBinaryProtocol(aTransport);
            } else if (this.m_protocolClass.Equals(typeof(TCompactProtocol))  ) {
                return new TCompactProtocol(aTransport);
            } else {
                return (TProtocol)(this.m_protoFactory != null ? this.m_protoFactory.GetProtocol(aTransport) : new TBinaryProtocol(aTransport));
            }
        }

        public Object getClient() {
            return this.sureOpen() ? this.m_client : null;
        }

        public bool doOpen() {
            lock (syncLock)
            {
                try {
                    Console.WriteLine("m_host " + m_host.ToString());
                    IPAddress ip = IPAddress.Parse(this.m_host);
                    this.m_transport = new TFramedTransport(new TSocketTransport(ip, this.m_port));
                    this.m_protocol = this.createProtocol(this.m_transport);
                    //var aClass = this.m_clientClass;
                    // TStringBigSetKVService.Client client = new TStringBigSetKVService.Client(this.m_protocol);
                    this.m_client = Activator.CreateInstance((Type)this.m_clientClass, this.m_protocol);
                    Task ts  = this.m_transport.OpenAsync();
                    //Console.WriteLine("ts "+ ts.IsCompleted);
                } catch (Exception var5) {
                    Console.WriteLine("doOpen error "+var5.ToString());
                    return false;
                }

                Console.WriteLine("this.m_trasnport "+ this.m_transport.ToString());
                return this.m_transport.IsOpen;
            }
            
        }

        public bool isOpen() {
            return this.m_transport != null && this.m_protocol != null && this.m_client != null ? this.m_transport.IsOpen : false;
        }

        public void close() {
            if (this.m_transport != null && this.m_protocol != null && this.m_client != null) {
                try {
                    this.m_transport.FlushAsync();
                    this.m_transport.Close();
                    this.m_transport = null;
                    this.m_protocol = null;
                    this.m_client = null;
                } catch (Exception var2) {
                    Console.WriteLine("close() error " + var2.ToString());
                }

            } else {
                this.m_transport = null;
                this.m_protocol = null;
                this.m_client = null;
            }
        }

        public bool sureOpen() {
            if (this.isOpen()) {
                return true;
            } else {
                this.close();
                return this.doOpen();
            }
        }

        public void cleanUp() {
            if (System.DateTime.Now.Millisecond - this.m_createTime < 600000L) {
                //Console.WriteLine("I3");
                ClientFactory.releaseClient(this);
            } else {
                //Console.WriteLine("I4");

                this.close();
            }

        }
    } 
}
