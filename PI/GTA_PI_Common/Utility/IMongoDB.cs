using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace GTA.PI.Utility
{
    public interface IMongoDB
    {
        /// <summary>
        /// 便于使用linq to mongodb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IMongoQueryable<T> AsQueryable<T>();

        /// <summary>
        /// 异步插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        void InsertManyAsync<T>(IEnumerable<T> objs);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        void InsertMany<T>(IEnumerable<T> objs);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="filter">删除的条件</param>
        void DeleteMany<T>(FilterDefinition<T> filter);

        ///// <summary>
        ///// 获取表里的指定字段
        ///// </summary>
        ///// <typeparam name="T">表名</typeparam>
        ///// <returns></returns>
        //IEnumerable<V> QueryNeedFields<T, V>(string jsonFilter, string jsonFields);
    }
}
