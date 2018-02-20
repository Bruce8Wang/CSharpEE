using MongoDB.Bson;

namespace GTA.PI.Models
{
    public class MyObject
    {
        public ObjectId _id; // 这个对应了 MongoDB.Bson.ObjectId (插入新数据不需要加这个字段，用于查询的)
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Sex { set; get; }
    }
}
