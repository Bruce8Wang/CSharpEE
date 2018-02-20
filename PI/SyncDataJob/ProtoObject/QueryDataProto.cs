using ProtoBuf;
using System;
using System.Collections.Generic;

namespace SyncDataJob.ProtoObject
{
    [Serializable, ProtoContract(Name = "RequestQueryData")]
    public class RequestQueryData : IExtensible
    {
        public RequestQueryData() { }

        private readonly List<string> _SecurityID = new List<string>();
        [ProtoMember(1, Name = @"SecurityID", DataFormat = DataFormat.Default)]
        public List<string> SecurityID
        {
            get { return _SecurityID; }
        }

        private string _StartTime;
        [ProtoMember(2, IsRequired = true, Name = @"StartTime", DataFormat = DataFormat.Default)]
        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }
        private string _EndTime;
        [ProtoMember(3, IsRequired = true, Name = @"EndTime", DataFormat = DataFormat.Default)]
        public string EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
        private uint _Frequency;
        [ProtoMember(4, IsRequired = true, Name = @"Frequency", DataFormat = DataFormat.TwosComplement)]
        public uint Frequency
        {
            get { return _Frequency; }
            set { _Frequency = value; }
        }
        private uint _MessageID;
        [ProtoMember(5, IsRequired = true, Name = @"MessageID", DataFormat = DataFormat.TwosComplement)]
        public uint MessageID
        {
            get { return _MessageID; }
            set { _MessageID = value; }
        }
        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"ReponseQueryData")]
    public class ReponseQueryData : IExtensible
    {
        public ReponseQueryData() { }

        private readonly List<DataInfo> _ListDataInfo = new List<DataInfo>();
        [ProtoMember(1, Name = @"ListDataInfo", DataFormat = DataFormat.Default)]
        public List<DataInfo> ListDataInfo
        {
            get { return _ListDataInfo; }
        }

        private uint _MessageID;
        [ProtoMember(2, IsRequired = true, Name = @"MessageID", DataFormat = DataFormat.TwosComplement)]
        public uint MessageID
        {
            get { return _MessageID; }
            set { _MessageID = value; }
        }
        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"DataInfo")]
    public class DataInfo : IExtensible
    {
        public DataInfo() { }

        private byte[] _data;
        [ProtoMember(1, IsRequired = true, Name = @"data", DataFormat = DataFormat.Default)]
        public byte[] data
        {
            get { return _data; }
            set { _data = value; }
        }
        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"RequestSecurityID")]
    public class RequestSecurityID : IExtensible
    {
        public RequestSecurityID() { }

        private readonly List<string> _SymbolName = new List<string>();
        [ProtoMember(1, Name = @"SymbolName", DataFormat = DataFormat.Default)]
        public List<string> SymbolName
        {
            get { return _SymbolName; }
        }

        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"ReponseSecurityID")]
    public class ReponseSecurityID : IExtensible
    {
        public ReponseSecurityID() { }

        private readonly List<SecrityIDInfo> _ListSecrityIDInfo = new List<SecrityIDInfo>();
        [ProtoMember(1, Name = @"ListSecrityIDInfo", DataFormat = DataFormat.Default)]
        public List<SecrityIDInfo> ListSecrityIDInfo
        {
            get { return _ListSecrityIDInfo; }
        }

        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"SecrityIDInfo")]
    public class SecrityIDInfo : IExtensible
    {
        public SecrityIDInfo() { }

        private string _SecurityID;
        [ProtoMember(1, IsRequired = true, Name = @"SecurityID", DataFormat = DataFormat.Default)]
        public string SecurityID
        {
            get { return _SecurityID; }
            set { _SecurityID = value; }
        }
        private string _Symbol;
        [ProtoMember(2, IsRequired = true, Name = @"Symbol", DataFormat = DataFormat.Default)]
        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }
        private string _SecurityExchange;
        [ProtoMember(3, IsRequired = true, Name = @"SecurityExchange", DataFormat = DataFormat.Default)]
        public string SecurityExchange
        {
            get { return _SecurityExchange; }
            set { _SecurityExchange = value; }
        }
        private string _TypeID;
        [ProtoMember(4, IsRequired = true, Name = @"TypeID", DataFormat = DataFormat.Default)]
        public string TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }
        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"ExceptionMessage")]
    public class ExceptionMessage : IExtensible
    {
        public ExceptionMessage() { }

        private int _ExceptionCode;
        [ProtoMember(1, IsRequired = true, Name = @"ExceptionCode", DataFormat = DataFormat.TwosComplement)]
        public int ExceptionCode
        {
            get { return _ExceptionCode; }
            set { _ExceptionCode = value; }
        }
        private string _ExceptionReason;
        [ProtoMember(2, IsRequired = true, Name = @"ExceptionReason", DataFormat = DataFormat.Default)]
        public string ExceptionReason
        {
            get { return _ExceptionReason; }
            set { _ExceptionReason = value; }
        }
        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"RequestQueryPlate")]
    public class RequestQueryPlate : IExtensible
    {
        public RequestQueryPlate() { }

        private string _plateID;
        [ProtoMember(1, IsRequired = true, Name = @"plateID", DataFormat = DataFormat.Default)]
        public string plateID
        {
            get { return _plateID; }
            set { _plateID = value; }
        }
        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"ReponseQueryPlate")]
    public class ReponseQueryPlate : IExtensible
    {
        public ReponseQueryPlate() { }

        private string _plateId;
        [ProtoMember(1, IsRequired = true, Name = @"plateId", DataFormat = DataFormat.Default)]
        public string plateId
        {
            get { return _plateId; }
            set { _plateId = value; }
        }
        private readonly List<PlateInfo> _listPlateInfo = new List<PlateInfo>();
        [ProtoMember(2, Name = @"listPlateInfo", DataFormat = DataFormat.Default)]
        public List<PlateInfo> listPlateInfo
        {
            get { return _listPlateInfo; }
        }

        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"PlateInfo")]
    public class PlateInfo : IExtensible
    {
        public PlateInfo() { }

        private string _symbol;
        [ProtoMember(1, IsRequired = true, Name = @"symbol", DataFormat = DataFormat.Default)]
        public string symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        private string _exchangeCode;
        [ProtoMember(2, IsRequired = true, Name = @"exchangeCode", DataFormat = DataFormat.Default)]
        public string exchangeCode
        {
            get { return _exchangeCode; }
            set { _exchangeCode = value; }
        }
        private string _securtiyID;
        [ProtoMember(3, IsRequired = true, Name = @"securtiyID", DataFormat = DataFormat.Default)]
        public string securtiyID
        {
            get { return _securtiyID; }
            set { _securtiyID = value; }
        }
        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"QueryQuoteDataRequest")]
    public class QueryQuoteDataRequest : IExtensible
    {
        public QueryQuoteDataRequest() { }

        private readonly List<QueryPlateInfo> _PlateInfos = new List<QueryPlateInfo>();
        [ProtoMember(1, Name = @"PlateInfos", DataFormat = DataFormat.Default)]
        public List<QueryPlateInfo> PlateInfos
        {
            get { return _PlateInfos; }
        }

        private readonly List<string> _Fields = new List<string>();
        [ProtoMember(2, Name = @"Fields", DataFormat = DataFormat.Default)]
        public List<string> Fields
        {
            get { return _Fields; }
        }

        private int _PageIndex;
        [ProtoMember(3, IsRequired = true, Name = @"PageIndex", DataFormat = DataFormat.TwosComplement)]
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }
        private int _PageSize;
        [ProtoMember(4, IsRequired = true, Name = @"PageSize", DataFormat = DataFormat.TwosComplement)]
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        private string _SortField;
        [ProtoMember(5, IsRequired = true, Name = @"SortField", DataFormat = DataFormat.Default)]
        public string SortField
        {
            get { return _SortField; }
            set { _SortField = value; }
        }
        private SortType _SortType;
        [ProtoMember(6, IsRequired = true, Name = @"SortType", DataFormat = DataFormat.TwosComplement)]
        public SortType SortType
        {
            get { return _SortType; }
            set { _SortType = value; }
        }
        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"QueryPlateInfo")]
    public class QueryPlateInfo : IExtensible
    {
        public QueryPlateInfo() { }

        private string _PlateID;
        [ProtoMember(1, IsRequired = true, Name = @"PlateID", DataFormat = DataFormat.Default)]
        public string PlateID
        {
            get { return _PlateID; }
            set { _PlateID = value; }
        }
        private QueryType _Type;
        [ProtoMember(2, IsRequired = true, Name = @"Type", DataFormat = DataFormat.TwosComplement)]
        public QueryType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private readonly List<FreqInfo> _FreqInfos = new List<FreqInfo>();
        [ProtoMember(3, Name = @"FreqInfos", DataFormat = DataFormat.Default)]
        public List<FreqInfo> FreqInfos
        {
            get { return _FreqInfos; }
        }

        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"FreqInfo")]
    public class FreqInfo : IExtensible
    {
        public FreqInfo() { }

        private string _MsgID;
        [ProtoMember(1, IsRequired = true, Name = @"MsgID", DataFormat = DataFormat.Default)]
        public string MsgID
        {
            get { return _MsgID; }
            set { _MsgID = value; }
        }
        private readonly List<int> _Freq = new List<int>();
        [ProtoMember(2, Name = @"Freq", DataFormat = DataFormat.TwosComplement)]
        public List<int> Freq
        {
            get { return _Freq; }
        }

        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"QueryQuoteDataResponse")]
    public class QueryQuoteDataResponse : IExtensible
    {
        public QueryQuoteDataResponse() { }

        private readonly List<QuoteDataInfo> _Data = new List<QuoteDataInfo>();
        [ProtoMember(1, Name = @"Data", DataFormat = DataFormat.Default)]
        public List<QuoteDataInfo> Data
        {
            get { return _Data; }
        }

        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [Serializable, ProtoContract(Name = @"QuoteDataInfo")]
    public class QuoteDataInfo : IExtensible
    {
        public QuoteDataInfo() { }

        private string _msgId;
        [ProtoMember(1, IsRequired = true, Name = @"msgId", DataFormat = DataFormat.Default)]
        public string msgId
        {
            get { return _msgId; }
            set { _msgId = value; }
        }
        private byte[] _data;
        [ProtoMember(2, IsRequired = true, Name = @"data", DataFormat = DataFormat.Default)]
        public byte[] data
        {
            get { return _data; }
            set { _data = value; }
        }
        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        { return Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [ProtoContract(Name = @"QueryType")]
    public enum QueryType
    {

        [ProtoEnum(Name = @"PlateID", Value = 1)]
        PlateID = 1,

        [ProtoEnum(Name = @"SecurityID", Value = 2)]
        SecurityID = 2
    }

    [ProtoContract(Name = @"SortType")]
    public enum SortType
    {

        [ProtoEnum(Name = @"None", Value = 0)]
        None = 0,

        [ProtoEnum(Name = @"Desc", Value = 1)]
        Desc = 1,

        [ProtoEnum(Name = @"Asec", Value = 2)]
        Asec = 2
    }

}