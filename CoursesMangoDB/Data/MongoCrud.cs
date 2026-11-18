using CoursesMangoDB.Models;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CoursesMangoDB.Data
{
    public class MongoCrud
    {
        private IMongoDatabase db;

        public MongoCrud(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"];
            var client = new MongoClient(connectionString);

            db = client.GetDatabase(databaseName);
        }


        //Create -ADD
        public async Task<Courses> AddCourse(string collectionToUse, Courses course)
        {
            var collection = db.GetCollection<Courses>(collectionToUse);
            await collection.InsertOneAsync(course);
            return course;
        }

        //Get ALL
        public async Task<List<Courses> > GetAllCourses(string c)
        {
            var collection = db.GetCollection<Courses>(c);
            var courses = await collection.AsQueryable().ToListAsync();
            return courses;
        }

        //GetByID
        public async Task<Courses> GetCourse(string id, string collection)
        {
            var c = db.GetCollection<Courses>(collection);
            var course = await c.Find(x => x.Id == id).FirstOrDefaultAsync();
          
            return course;
        }
        
        //update 
        public async Task<Courses> UpdateCoures(string CollectionToUse, string id, Courses updateCoures)
        {
            var collection = db.GetCollection<Courses>(CollectionToUse);
            updateCoures.Id = id;

            var result = await collection.ReplaceOneAsync(x => x.Id == id, updateCoures);

            return result.MatchedCount == 0 ? null : updateCoures;
        }


        //Delete
        public async Task<string> DeleteCourse( string c, string id)
        {
            var collection = db.GetCollection<Courses>(c);
            await collection.DeleteOneAsync(r => r.Id == id);
            return "Courses was deleted...";
        }
        
    }
}
