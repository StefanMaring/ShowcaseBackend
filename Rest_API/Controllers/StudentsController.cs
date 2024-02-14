using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rest_API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase {
        private static List<Student> students = new List<Student>{};
        public StudentsController() {
            students.Add(new Student { Id = 1, Name = "John" });
            students.Add(new Student { Id = 2, Name = "Jane" });
        }

        [HttpGet]
        public IEnumerable<Student> Get() {
            return students;
        }

        [HttpGet("{id}")]
        public Student Get(int id) {
            return students.FirstOrDefault(s => s.Id == id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Student student) {
            students.Add(student);
            return Ok();
        }
    }
}
