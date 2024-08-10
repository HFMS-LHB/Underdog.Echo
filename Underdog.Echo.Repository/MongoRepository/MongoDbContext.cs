using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.Echo.Common.Helper;

namespace Underdog.Echo.Repository.MongoRepository
{

    public class MongoDbContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoDbContext()
        {
            var client = new MongoClient(AppSettings.app(new string[] { "Mongo", "ConnectionString" }));
            _database = client.GetDatabase(AppSettings.app(new string[] { "Mongo", "Database" }));
        }

        public IMongoDatabase Db
        {
            get { return _database; }
        }

        //public IMongoCollection<TEntity> Query
        //{
        //    get
        //    {
        //        return _database.GetCollection<TEntity>(nameof(TEntity));
        //    }
        //}
    }
}
