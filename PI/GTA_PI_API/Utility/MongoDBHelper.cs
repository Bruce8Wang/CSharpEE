using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace GTA.PI.API.Utility
{
    internal class MongoDBHelper
    {
        private static IMongoDatabase db;
        static MongoDBHelper()
        {
            //string connectionString = "mongodb://10.1.135.81:27017?ConnectTimeout=30000;ConnectionLifetime=300000;MinimumPoolSize=8;MaximumPoolSize=256;Pooled=true";
            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["ConnectStr"].Trim());
            db = client.GetDatabase(ConfigurationManager.AppSettings["DBName"].Trim());
        }
        public static IMongoDatabase Current { get { return db; } }

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
        /// 获取表里的指定字段的所有数据
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <returns></returns>
        public static List<V> QueryNeedFields<T, V>(FilterDefinition<T> jsonFilter, string jsonFields)
        {
            //获取MyObject集合
            IMongoCollection<T> col = db.GetCollection<T>(typeof(T).Name);
            FindOptions<T, V> opt = new FindOptions<T, V>()
            {
                Projection = new JsonProjectionDefinition<T, V>(jsonFields)
            };
            IAsyncCursor<V> cursor = col.FindSync(jsonFilter, opt);
            List<V> results = new List<V>();
            while (cursor.MoveNext())
            {
                results.AddRange(cursor.Current);
            }
            return results;
        }
    }
}
