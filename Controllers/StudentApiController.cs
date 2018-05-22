using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Interfaces;
using StudentApi.Model;
using StudentApi.Infrastructure;
using System;
using System.Collections.Generic;

namespace StudentApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _StudentRepository;

        public StudentsController(IStudentRepository StudentRepository)
        {
            _StudentRepository = StudentRepository;
        }

        [NoCache]
        [HttpGet]
        public async Task<IEnumerable<StudentData>> Get()
        {
            return await _StudentRepository.GetAllStudents();
        }

        // GET api/Students/5
        [HttpGet("{id}")]
        public async Task<StudentData> Get(string id)
        {
            return await _StudentRepository.GetStudent(id) ?? new StudentData();
        }

        // POST api/Students
        [HttpPost]
        public void Post([FromBody] StudentParam Student) => _StudentRepository.InsertStudent(student: new StudentData
        {
            StudentId = Student.StudentId,
            FirstName = Student.FirstName,
            LastName = Student.LastName,
            Age = Student.Age,
            Grade = Student.Grade,
            Gender = Student.Gender,
            CreatedOn = DateTime.Now,
            UpdatedOn = DateTime.Now,
        });

        // PUT api/Students/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string UpdatedStudent)
        {
            _StudentRepository.UpdateStudentData(id, UpdatedStudent);
        }

        // DELETE api/Students/23243423
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _StudentRepository.RemoveStudent(id);
        }
    }
}
