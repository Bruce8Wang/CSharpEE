using GTA.Dsp.ProtocolHandler;
using System.ComponentModel.Composition;

namespace SyncDataJob.ProtoObject
{
    /// <summary>
    /// 取数二开寻址类
    /// </summary>
    [Export(typeof(IProtoAnalyse))]
    public class RequestDataProtoAnalyser : GoogleProtoAnalyseBase
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public override byte TypeID
        {
            get { return 10; }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public override string Ver
        {
            get { return "1.0"; }
        }

        /// <summary>
        /// 初始化Key
        /// </summary>
        protected override void InitBusinessKey()
        {
            base.Add(0x0001, typeof(RequestQueryData));//请求查询数据
            base.Add(0x0002, typeof(ReponseQueryData));//查询数据结果返回
            base.Add(0x0003, typeof(RequestSecurityID));//请求SecurityId列表
            base.Add(0x0004, typeof(ReponseSecurityID));//SecurityId列表查询
            base.Add(0x0005, typeof(RequestQueryPlate));//查询板块信息
            base.Add(0x0006, typeof(ReponseQueryPlate));//获取版本信息
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="busniessID"></param>
        /// <returns></returns>
        public override object ProtoBodySerialize(object data, ushort busniessID)
        {
            switch (busniessID)
            {
                case 0x0001:
                    return Serializer.Serialize<RequestQueryData>(data as RequestQueryData);
                case 0x0002:
                    return Serializer.Serialize<ReponseQueryData>(data as ReponseQueryData);
                case 0x0003:
                    return Serializer.Serialize<RequestSecurityID>(data as RequestSecurityID);
                case 0x0004:
                    return Serializer.Serialize<ReponseSecurityID>(data as ReponseSecurityID);
                case 0x0005:
                    return Serializer.Serialize<RequestQueryPlate>(data as RequestQueryPlate);
                case 0x0006:
                    return Serializer.Serialize<ReponseQueryPlate>(data as ReponseQueryPlate);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="busniessID"></param>
        /// <returns></returns>
        public override object ProtoBodyDeSerialize(object data, ushort busniessID)
        {
            switch (busniessID)
            {
                case 0x0001:
                    return Serializer.Deserialize<RequestQueryData>(data);
                case 0x0002:
                    return Serializer.Deserialize<ReponseQueryData>(data);
                case 0x0003:
                    return Serializer.Deserialize<RequestSecurityID>(data);
                case 0x0004:
                    return Serializer.Deserialize<ReponseSecurityID>(data);
                case 0x0005:
                    return Serializer.Deserialize<RequestQueryPlate>(data);
                case 0x0006:
                    return Serializer.Deserialize<ReponseQueryPlate>(data);
                default:
                    return null;
            }
        }
    }
}
