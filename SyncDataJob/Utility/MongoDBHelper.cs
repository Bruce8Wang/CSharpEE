using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace SyncDataJob.Utility
{
    class MongoDBHelper
    {
        // 创建数据库链接
        //private static MongoServer server;
        // 获得数据库cnblogs
        private static IMongoDatabase db;

        static MongoDBHelper()
        {
            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["ConnectStr"].Trim());
            db = client.GetDatabase(ConfigurationManager.AppSettings["DBName"].Trim());
        }

        /// <summary>
        /// 便于使用linq to mongodb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IMongoQueryable<T> AsQueryable<T>()
        {
            return db.GetCollection<T>(typeof(T).Name).AsQueryable<T>();
        }

        /// <summary>
        /// 异步插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        public static void InsertManyAsync<T>(IEnumerable<T> objs)
        {
            //获得MyObject集合
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            //执行插入操作
            col.InsertManyAsync(objs);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        public static void InsertMany<T>(IEnumerable<T> objs)
        {
            //获得MyObject集合,如果数据库中没有，先新建一个
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            //执行插入操作
            col.InsertMany(objs);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enity"></param>
        public static void InsertOne<T>(T enity)
        {
            //获得MyObject集合,如果数据库中没有，先新建一个
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            //执行插入操作
            col.InsertOne(enity);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="filter">删除的条件</param>
        public static void DeleteMany<T>(FilterDefinition<T> filter)
        {
            //获取MyObject集合
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            //执行删除操作
            col.DeleteMany(filter);
        }

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="filter">删除的条件</param>
        public static void DeleteManyAsync<T>(FilterDefinition<T> filter)
        {
            //获取MyObject集合
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            //执行删除操作
            col.DeleteManyAsync(filter);
        }

        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="enity"></param>
        /// <returns></returns>
        public static T FindOneAndReplace<T>(System.Linq.Expressions.Expression<System.Func<T, bool>> filter, T enity)
        {
            //获取MyObject集合
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            return col.FindOneAndReplace(filter, enity);
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        public static void CreatManyIndexAsync<T>(IEnumerable<CreateIndexModel<T>> models)
        {
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            col.Indexes.DropAll();
            col.Indexes.CreateManyAsync(models);
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        public static void CreateOneIndexAsync<T>(IndexKeysDefinition<T> keys)
        {
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            col.Indexes.DropAll();
            col.Indexes.CreateOneAsync(keys);
        }

        /// <summary>
        /// 返回件数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static long Count<T>()
        {
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            return col.Count("{}");
        }
    }
}
