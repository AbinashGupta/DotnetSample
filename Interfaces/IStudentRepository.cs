using System.Collections.Generic;
using System.Threading.Tasks;
using StudentApi.Model;

namespace StudentApi.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentData>> GetAllStudents();
        Task<StudentData> GetStudent(string id);

        // add new note document
        Task InsertStudent(StudentData student);

        // remove a single document / note
        Task<bool> RemoveStudent(string id);

        // update just a single document / note
        Task<bool> UpdateStudentData(string id, string UpdatedStudent);

        // creates a sample index
        Task<string> CreateIndex();
    }
}
