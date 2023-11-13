using Azure.Core;
using BercaAPI.Context;
using BercaAPI.Models;
using BercaAPI.Repository.Interface;
using BercaAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.Xml;

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

        public Object GetStatusEmployeePerDepartment()
        {
            var result = context.Employees
                .GroupBy(e => e.department.Name)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    ActiveEmployeeCount = g.Count(emp => emp.Status == true),
                    ResignEmployeeCount = g.Count(emp => emp.Status == false),
                }).ToList();

            return result;
        }

        public Object GetEmployeeCountPerDepartment()
        {
            var result = context.Employees
                .GroupBy(e => e.department.Name)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    EmployeesCount = g.Count(),
                }).ToList();

            return result;
        }

        public Object GetDataPaging(JqueryDatatableParam dataTable)
        {
            int totalRecord = 0;
            int filterRecord = 0;
            
            var data = context.Departments.AsQueryable();

            //get total count of data in table
            totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(dataTable.SearchValue))
            {
                data = data.Where(x => x.Dept_Id.ToLower().Contains(dataTable.SearchValue.ToLower()) || x.Name.ToLower().Contains(dataTable.SearchValue.ToLower()));
            }

            // get total count of records after search
            filterRecord = data.Count();

            //sort data
            if (!string.IsNullOrEmpty(dataTable.SortColumn) && !string.IsNullOrEmpty(dataTable.SortColumnDirection)) data = data.OrderBy(dataTable.SortColumn + " " + dataTable.SortColumnDirection);
            //pagination
            var empList = data.Skip(dataTable.Skip).Take(dataTable.PageSize).ToList();

            var returnObj = new
            {
                draw = dataTable.Draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = empList
            };

            return returnObj;
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
            var data = context.Departments.FirstOrDefault(e => e.Dept_Id == Id);
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
