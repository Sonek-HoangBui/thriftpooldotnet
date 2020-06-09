using System;
using System.Text;
using org.openstars.core.bigset.Generic;
using Thrift;
using Thrift.Protocol;
namespace ThriftPoolDotNet
{
    public class BigSetClient
    {
        public bool isCompactProtocol = false; // default is binary protocol
        private String m_host;
        private int m_port;
    
        public BigSetClient(String host, int port, bool isCompact)
        {
            m_host = host;
            m_port = port;
            isCompactProtocol = isCompact;
        }
    
        public BigSetClient(String host, int port)
        {
            this.m_host = host;
            this.m_port = port;
            this.isCompactProtocol = false;
        }    
    
        public TClientInfo getClient()
        {
            if (isCompactProtocol){
                //cols"compact");
                return ClientFactory.getClient(m_host, m_port, typeof(TStringBigSetKVService.Client),  typeof(TCompactProtocol) );
            }
            else {
                // System.out.println("binary");
                return ClientFactory.getClient(m_host, m_port, typeof(TStringBigSetKVService.Client),  typeof(TBinaryProtocol));
            }
        }
        
       
        
        
    }

}