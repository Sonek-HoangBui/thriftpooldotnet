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

  public partial class TItemSetResult : TBase
  {
    private TErrorCode _error;
    private TItemSet _items;

    /// <summary>
    /// 
    /// <seealso cref="TErrorCode"/>
    /// </summary>
    public TErrorCode Error
    {
      get
      {
        return _error;
      }
      set
      {
        __isset.error = true;
        this._error = value;
      }
    }

    public TItemSet Items
    {
      get
      {
        return _items;
      }
      set
      {
        __isset.items = true;
        this._items = value;
      }
    }


    public Isset __isset;
    public struct Isset
    {
      public bool error;
      public bool items;
    }

    public TItemSetResult()
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
              if (field.Type == TType.I32)
              {
                Error = (TErrorCode)await iprot.ReadI32Async(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 2:
              if (field.Type == TType.Struct)
              {
                Items = new TItemSet();
                await Items.ReadAsync(iprot, cancellationToken);
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
        var struc = new TStruct("TItemSetResult");
        await oprot.WriteStructBeginAsync(struc, cancellationToken);
        var field = new TField();
        if (__isset.error)
        {
          field.Name = "error";
          field.Type = TType.I32;
          field.ID = 1;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await oprot.WriteI32Async((int)Error, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (Items != null && __isset.items)
        {
          field.Name = "items";
          field.Type = TType.Struct;
          field.ID = 2;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await Items.WriteAsync(oprot, cancellationToken);
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
      var other = that as TItemSetResult;
      if (other == null) return false;
      if (ReferenceEquals(this, other)) return true;
      return ((__isset.error == other.__isset.error) && ((!__isset.error) || (System.Object.Equals(Error, other.Error))))
        && ((__isset.items == other.__isset.items) && ((!__isset.items) || (System.Object.Equals(Items, other.Items))));
    }

    public override int GetHashCode() {
      int hashcode = 157;
      unchecked {
        if(__isset.error)
          hashcode = (hashcode * 397) + Error.GetHashCode();
        if(__isset.items)
          hashcode = (hashcode * 397) + Items.GetHashCode();
      }
      return hashcode;
    }

    public override string ToString()
    {
      var sb = new StringBuilder("TItemSetResult(");
      bool __first = true;
      if (__isset.error)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("Error: ");
        sb.Append(Error);
      }
      if (Items != null && __isset.items)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("Items: ");
        sb.Append(Items== null ? "<null>" : Items.ToString());
      }
      sb.Append(")");
      return sb.ToString();
    }
  }

}