using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using StudentApi.Interfaces;
using StudentApi.Model;
using MongoDB.Bson;

namespace StudentApi.Data
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context = null;

        public StudentRepository(IOptions<Settings> settings)
        {
            _context = new StudentContext(settings);
        }

        public async Task<IEnumerable<StudentData>> GetAllStudents()
        {
            try
            {
                return await _context.Students.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after internal or internal id
        //
        public async Task<StudentData> GetStudent(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Students
                                .Find(student => student.StudentId == id || student.InternalId == internalId)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public async Task InsertStudent(StudentData student)
        {
            try
            {
                await _context.Students.InsertOneAsync(student);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveStudent(string id)
        {
            try
            {
                DeleteResult actionResult = await _context.Students.DeleteOneAsync(
                     Builders<StudentData>.Filter.Eq("StudentId", id));

                return actionResult.IsAcknowledged 
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateStudentData(string id, string UpdatedStudent)
        {
            var filter = Builders<StudentData>.Filter.Eq(s => s.StudentId, id);
            var update = Builders<StudentData>.Update
                            .Set(s => s.FirstName, UpdatedStudent)
                            .CurrentDate(s => s.UpdatedOn);

            try
            {
                UpdateResult actionResult = await _context.Students.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateStudent(string id, StudentData student)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.Students
                                                .ReplaceOneAsync(n => n.StudentId.Equals(id)
                                                                , student
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // it creates a compound index (first using UserId, and then Body)
        // MongoDb automatically detects if the index already exists - in this case it just returns the index details
        public async Task<string> CreateIndex()
        {
            try
            {
                return await _context.Students.Indexes
                                           .CreateOneAsync(Builders<StudentData>
                                                                .IndexKeys
                                                                .Ascending(student => student.StudentId)
                                                                .Ascending(student => student.FirstName));
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
