//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: common_pb.proto
namespace protocol
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AwardUnit")]
  public partial class AwardUnit : global::ProtoBuf.IExtensible
  {
    public AwardUnit() {}
    
    private uint _type = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint type
    {
      get { return _type; }
      set { _type = value; }
    }
    private uint _gid = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"gid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint gid
    {
      get { return _gid; }
      set { _gid = value; }
    }
    private uint _val = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"val", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint val
    {
      get { return _val; }
      set { _val = value; }
    }
    private uint _min_val = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"min_val", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint min_val
    {
      get { return _min_val; }
      set { _min_val = value; }
    }
    private uint _max_val = default(uint);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"max_val", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint max_val
    {
      get { return _max_val; }
      set { _max_val = value; }
    }
    private uint _test = default(uint);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"test", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint test
    {
      get { return _test; }
      set { _test = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}