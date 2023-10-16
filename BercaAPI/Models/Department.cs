using System.ComponentModel.DataAnnotations;

namespace BercaAPI.Models
{
    public class Department
    {
        [Key]
        public string Dept_Id { get; set; }
        public string Name { get; set;  }

        public ICollection<Employee> Employees { get; set; }
    }
}
