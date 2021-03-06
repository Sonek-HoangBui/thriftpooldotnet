/**
 * Autogenerated by Thrift Compiler (0.13.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Thrift;
using Thrift.Collections;

using Thrift.Protocol;
using Thrift.Protocol.Entities;
using Thrift.Protocol.Utilities;
using Thrift.Transport;
using Thrift.Transport.Client;
using Thrift.Transport.Server;
using Thrift.Processor;


namespace org.openstars.core.bigset.Generic
{
  public partial class TCluserOrdinatorService
  {
    public interface IAsync
    {
      Task removeCacheAsync(long key, CancellationToken cancellationToken = default(CancellationToken));

      Task<int> putAsync(byte[] key, byte[] @value, CancellationToken cancellationToken = default(CancellationToken));

    }


    public class Client : TBaseClient, IDisposable, IAsync
    {
      public Client(TProtocol protocol) : this(protocol, protocol)
      {
      }

      public Client(TProtocol inputProtocol, TProtocol outputProtocol) : base(inputProtocol, outputProtocol)      {
      }
      public async Task removeCacheAsync(long key, CancellationToken cancellationToken = default(CancellationToken))
      {
        await OutputProtocol.WriteMessageBeginAsync(new TMessage("removeCache", TMessageType.Oneway, SeqId), cancellationToken);
        
        var args = new removeCacheArgs();
        args.Key = key;
        
        await args.WriteAsync(OutputProtocol, cancellationToken);
        await OutputProtocol.WriteMessageEndAsync(cancellationToken);
        await OutputProtocol.Transport.FlushAsync(cancellationToken);
      }
      public async Task<int> putAsync(byte[] key, byte[] @value, CancellationToken cancellationToken = default(CancellationToken))
      {
        await OutputProtocol.WriteMessageBeginAsync(new TMessage("put", TMessageType.Call, SeqId), cancellationToken);
        
        var args = new putArgs();
        args.Key = key;
        args.Value = @value;
        
        await args.WriteAsync(OutputProtocol, cancellationToken);
        await OutputProtocol.WriteMessageEndAsync(cancellationToken);
        await OutputProtocol.Transport.FlushAsync(cancellationToken);
        
        var msg = await InputProtocol.ReadMessageBeginAsync(cancellationToken);
        if (msg.Type == TMessageType.Exception)
        {
          var x = await TApplicationException.ReadAsync(InputProtocol, cancellationToken);
          await InputProtocol.ReadMessageEndAsync(cancellationToken);
          throw x;
        }

        var result = new putResult();
        await result.ReadAsync(InputProtocol, cancellationToken);
        await InputProtocol.ReadMessageEndAsync(cancellationToken);
        if (result.__isset.success)
        {
          return result.Success;
        }
        throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "put failed: unknown result");
      }

    }

    public class AsyncProcessor : ITAsyncProcessor
    {
      private IAsync _iAsync;

      public AsyncProcessor(IAsync iAsync)
      {
        if (iAsync == null) throw new ArgumentNullException(nameof(iAsync));

        _iAsync = iAsync;
        processMap_["removeCache"] = removeCache_ProcessAsync;
        processMap_["put"] = put_ProcessAsync;
      }

      protected delegate Task ProcessFunction(int seqid, TProtocol iprot, TProtocol oprot, CancellationToken cancellationToken);
      protected Dictionary<string, ProcessFunction> processMap_ = new Dictionary<string, ProcessFunction>();

      public async Task<bool> ProcessAsync(TProtocol iprot, TProtocol oprot)
      {
        return await ProcessAsync(iprot, oprot, CancellationToken.None);
      }

      public async Task<bool> ProcessAsync(TProtocol iprot, TProtocol oprot, CancellationToken cancellationToken)
      {
        try
        {
          var msg = await iprot.ReadMessageBeginAsync(cancellationToken);

          ProcessFunction fn;
          processMap_.TryGetValue(msg.Name, out fn);

          if (fn == null)
          {
            await TProtocolUtil.SkipAsync(iprot, TType.Struct, cancellationToken);
            await iprot.ReadMessageEndAsync(cancellationToken);
            var x = new TApplicationException (TApplicationException.ExceptionType.UnknownMethod, "Invalid method name: '" + msg.Name + "'");
            await oprot.WriteMessageBeginAsync(new TMessage(msg.Name, TMessageType.Exception, msg.SeqID), cancellationToken);
            await x.WriteAsync(oprot, cancellationToken);
            await oprot.WriteMessageEndAsync(cancellationToken);
            await oprot.Transport.FlushAsync(cancellationToken);
            return true;
          }

          await fn(msg.SeqID, iprot, oprot, cancellationToken);

        }
        catch (IOException)
        {
          return false;
        }

        return true;
      }

      public async Task removeCache_ProcessAsync(int seqid, TProtocol iprot, TProtocol oprot, CancellationToken cancellationToken)
      {
        var args = new removeCacheArgs();
        await args.ReadAsync(iprot, cancellationToken);
        await iprot.ReadMessageEndAsync(cancellationToken);
        try
        {
          await _iAsync.removeCacheAsync(args.Key, cancellationToken);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public async Task put_ProcessAsync(int seqid, TProtocol iprot, TProtocol oprot, CancellationToken cancellationToken)
      {
        var args = new putArgs();
        await args.ReadAsync(iprot, cancellationToken);
        await iprot.ReadMessageEndAsync(cancellationToken);
        var result = new putResult();
        try
        {
          result.Success = await _iAsync.putAsync(args.Key, args.Value, cancellationToken);
          await oprot.WriteMessageBeginAsync(new TMessage("put", TMessageType.Reply, seqid), cancellationToken); 
          await result.WriteAsync(oprot, cancellationToken);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
          var x = new TApplicationException(TApplicationException.ExceptionType.InternalError," Internal error.");
          await oprot.WriteMessageBeginAsync(new TMessage("put", TMessageType.Exception, seqid), cancellationToken);
          await x.WriteAsync(oprot, cancellationToken);
        }
        await oprot.WriteMessageEndAsync(cancellationToken);
        await oprot.Transport.FlushAsync(cancellationToken);
      }

    }


    public partial class removeCacheArgs : TBase
    {
      private long _key;

      public long Key
      {
        get
        {
          return _key;
        }
        set
        {
          __isset.key = true;
          this._key = value;
        }
      }


      public Isset __isset;
      public struct Isset
      {
        public bool key;
      }

      public removeCacheArgs()
      {
      }

      public async Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          await iprot.ReadStructBeginAsync(cancellationToken);
          while (true)
          {
            field = await iprot.ReadFieldBeginAsync(cancellationToken);
            if (field.Type == TType.Stop)
            {
              break;
            }

            switch (field.ID)
            {
              case 1:
                if (field.Type == TType.I64)
                {
                  Key = await iprot.ReadI64Async(cancellationToken);
                }
                else
                {
                  await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                }
                break;
              default: 
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                break;
            }

            await iprot.ReadFieldEndAsync(cancellationToken);
          }

          await iprot.ReadStructEndAsync(cancellationToken);
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
      {
        oprot.IncrementRecursionDepth();
        try
        {
          var struc = new TStruct("removeCache_args");
          await oprot.WriteStructBeginAsync(struc, cancellationToken);
          var field = new TField();
          if (__isset.key)
          {
            field.Name = "key";
            field.Type = TType.I64;
            field.ID = 1;
            await oprot.WriteFieldBeginAsync(field, cancellationToken);
            await oprot.WriteI64Async(Key, cancellationToken);
            await oprot.WriteFieldEndAsync(cancellationToken);
          }
          await oprot.WriteFieldStopAsync(cancellationToken);
          await oprot.WriteStructEndAsync(cancellationToken);
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override bool Equals(object that)
      {
        var other = that as removeCacheArgs;
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ((__isset.key == other.__isset.key) && ((!__isset.key) || (System.Object.Equals(Key, other.Key))));
      }

      public override int GetHashCode() {
        int hashcode = 157;
        unchecked {
          if(__isset.key)
            hashcode = (hashcode * 397) + Key.GetHashCode();
        }
        return hashcode;
      }

      public override string ToString()
      {
        var sb = new StringBuilder("removeCache_args(");
        bool __first = true;
        if (__isset.key)
        {
          if(!__first) { sb.Append(", "); }
          __first = false;
          sb.Append("Key: ");
          sb.Append(Key);
        }
        sb.Append(")");
        return sb.ToString();
      }
    }


    public partial class putArgs : TBase
    {
      private byte[] _key;
      private byte[] _value;

      public byte[] Key
      {
        get
        {
          return _key;
        }
        set
        {
          __isset.key = true;
          this._key = value;
        }
      }

      public byte[] Value
      {
        get
        {
          return _value;
        }
        set
        {
          __isset.@value = true;
          this._value = value;
        }
      }


      public Isset __isset;
      public struct Isset
      {
        public bool key;
        public bool @value;
      }

      public putArgs()
      {
      }

      public async Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          await iprot.ReadStructBeginAsync(cancellationToken);
          while (true)
          {
            field = await iprot.ReadFieldBeginAsync(cancellationToken);
            if (field.Type == TType.Stop)
            {
              break;
            }

            switch (field.ID)
            {
              case 1:
                if (field.Type == TType.String)
                {
                  Key = await iprot.ReadBinaryAsync(cancellationToken);
                }
                else
                {
                  await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                }
                break;
              case 2:
                if (field.Type == TType.String)
                {
                  Value = await iprot.ReadBinaryAsync(cancellationToken);
                }
                else
                {
                  await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                }
                break;
              default: 
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                break;
            }

            await iprot.ReadFieldEndAsync(cancellationToken);
          }

          await iprot.ReadStructEndAsync(cancellationToken);
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
      {
        oprot.IncrementRecursionDepth();
        try
        {
          var struc = new TStruct("put_args");
          await oprot.WriteStructBeginAsync(struc, cancellationToken);
          var field = new TField();
          if (Key != null && __isset.key)
          {
            field.Name = "key";
            field.Type = TType.String;
            field.ID = 1;
            await oprot.WriteFieldBeginAsync(field, cancellationToken);
            await oprot.WriteBinaryAsync(Key, cancellationToken);
            await oprot.WriteFieldEndAsync(cancellationToken);
          }
          if (Value != null && __isset.@value)
          {
            field.Name = "value";
            field.Type = TType.String;
            field.ID = 2;
            await oprot.WriteFieldBeginAsync(field, cancellationToken);
            await oprot.WriteBinaryAsync(Value, cancellationToken);
            await oprot.WriteFieldEndAsync(cancellationToken);
          }
          await oprot.WriteFieldStopAsync(cancellationToken);
          await oprot.WriteStructEndAsync(cancellationToken);
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override bool Equals(object that)
      {
        var other = that as putArgs;
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ((__isset.key == other.__isset.key) && ((!__isset.key) || (TCollections.Equals(Key, other.Key))))
          && ((__isset.@value == other.__isset.@value) && ((!__isset.@value) || (TCollections.Equals(Value, other.Value))));
      }

      public override int GetHashCode() {
        int hashcode = 157;
        unchecked {
          if(__isset.key)
            hashcode = (hashcode * 397) + Key.GetHashCode();
          if(__isset.@value)
            hashcode = (hashcode * 397) + Value.GetHashCode();
        }
        return hashcode;
      }

      public override string ToString()
      {
        var sb = new StringBuilder("put_args(");
        bool __first = true;
        if (Key != null && __isset.key)
        {
          if(!__first) { sb.Append(", "); }
          __first = false;
          sb.Append("Key: ");
          sb.Append(Key);
        }
        if (Value != null && __isset.@value)
        {
          if(!__first) { sb.Append(", "); }
          __first = false;
          sb.Append("Value: ");
          sb.Append(Value);
        }
        sb.Append(")");
        return sb.ToString();
      }
    }


    public partial class putResult : TBase
    {
      private int _success;

      public int Success
      {
        get
        {
          return _success;
        }
        set
        {
          __isset.success = true;
          this._success = value;
        }
      }


      public Isset __isset;
      public struct Isset
      {
        public bool success;
      }

      public putResult()
      {
      }

      public async Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          await iprot.ReadStructBeginAsync(cancellationToken);
          while (true)
          {
            field = await iprot.ReadFieldBeginAsync(cancellationToken);
            if (field.Type == TType.Stop)
            {
              break;
            }

            switch (field.ID)
            {
              case 0:
                if (field.Type == TType.I32)
                {
                  Success = await iprot.ReadI32Async(cancellationToken);
                }
                else
                {
                  await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                }
                break;
              default: 
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                break;
            }

            await iprot.ReadFieldEndAsync(cancellationToken);
          }

          await iprot.ReadStructEndAsync(cancellationToken);
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
      {
        oprot.IncrementRecursionDepth();
        try
        {
          var struc = new TStruct("put_result");
          await oprot.WriteStructBeginAsync(struc, cancellationToken);
          var field = new TField();

          if(this.__isset.success)
          {
            field.Name = "Success";
            field.Type = TType.I32;
            field.ID = 0;
            await oprot.WriteFieldBeginAsync(field, cancellationToken);
            await oprot.WriteI32Async(Success, cancellationToken);
            await oprot.WriteFieldEndAsync(cancellationToken);
          }
          await oprot.WriteFieldStopAsync(cancellationToken);
          await oprot.WriteStructEndAsync(cancellationToken);
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override bool Equals(object that)
      {
        var other = that as putResult;
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ((__isset.success == other.__isset.success) && ((!__isset.success) || (System.Object.Equals(Success, other.Success))));
      }

      public override int GetHashCode() {
        int hashcode = 157;
        unchecked {
          if(__isset.success)
            hashcode = (hashcode * 397) + Success.GetHashCode();
        }
        return hashcode;
      }

      public override string ToString()
      {
        var sb = new StringBuilder("put_result(");
        bool __first = true;
        if (__isset.success)
        {
          if(!__first) { sb.Append(", "); }
          __first = false;
          sb.Append("Success: ");
          sb.Append(Success);
        }
        sb.Append(")");
        return sb.ToString();
      }
    }

  }
}
