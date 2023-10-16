using BercaAPI.Context;
using BercaAPI.Models;
using BercaAPI.Repository.Interface;
using BercaAPI.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BercaAPI.Repository
{
    public class DepartmentRepository : DepartmentInterface
    {
        private readonly MyContext context;
        public DepartmentRepository(MyContext context)
        {
            this.context = context;
        }

        public IEnumerable<Department> Get()
        {
            return context.Departments.ToList();
        }

        public Department Get(string Id)
        {
            return context.Departments.Find(Id);
        }

        public int Insert(DepartmentVM department)
        {
            Department dept = new Department {
                Dept_Id = GenerateDeptId(),
                Name = department.Name,
            };
            context.Departments.Add(dept);
            var result = context.SaveChanges();
            return result;
        }

        public int Update(DepartmentVM department)
        {
            Department dept = new Department
            {
                Dept_Id = department.DepartmentID,
                Name = department.Name,
            };
            context.Entry(dept).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }

        public int Delete(string Id)
        {
            var data = context.Employees.FirstOrDefault(e => e.Department_Id == Id);
            if (data != null)
            {
                return -1;
            }
            else
            {
                var entity = context.Departments.Find(Id);
                context.Remove(entity);
                var result = context.SaveChanges();
                return result;
            }
        }
        public string GenerateDeptId()
        {
            var last = context.Departments.OrderByDescending(e => e.Dept_Id).FirstOrDefault();
            if (last == null)
            {
                return "D001";
            }
            else
            {
                string lastId = last.Dept_Id.Substring(last.Dept_Id.Length - 3);
                int numberId = int.Parse(lastId) + 1;
                return "D" + numberId.ToString("000");
            }
        }
    }
}
