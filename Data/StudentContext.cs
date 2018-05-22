using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StudentApi.Model;

namespace StudentApi.Data
{
    public class StudentContext
    {
        private readonly IMongoDatabase _database = null;

        public StudentContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<StudentData> Students
        {
            get
            {
                return _database.GetCollection<StudentData>("StuData");
            }
        }
    }
}
