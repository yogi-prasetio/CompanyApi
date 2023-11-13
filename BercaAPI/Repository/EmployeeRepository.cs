using BercaAPI.Context;
using BercaAPI.Models;
using BercaAPI.Repository.Interface;
using BercaAPI.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BercaAPI.Repository
{
    public class EmployeeRepository : EmployeeInterface
    {
        private readonly MyContext context;
        private readonly string domain = "@berca.co.id";
        public EmployeeRepository(MyContext context) 
        { 
            this.context = context;
        }

        public IEnumerable<object> Get()
        {
            var result =context.Employees.Include(d => d.department).ToList()
                .Select(data => new EmployeeDepartmentVM
                {
                    NIK = data.NIK,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Address = data.Address,
                    Status = data.Status,
                    Department_Id = data.Department_Id,
                    Department_Name = data.department.Name
                });
            return result;
        }

        public Employee Get(string NIK)
        {
            return context.Employees.Find(NIK);
        }

        public int Insert(EmployeeVM employee)
        {
            var check = CheckPhoneNumber(employee.NIK, employee.PhoneNumber);

            if (check)
            {
                return -1;
            }
            else
            {
                Employee emp = new Employee
                {
                    NIK = GenerateNIK(),
                    FirstName = employee.FirstName,
                    Email = GenerateEmail(employee.FirstName, employee.LastName),
                    LastName = employee.LastName,
                    PhoneNumber = employee.PhoneNumber,
                    Address = employee.Address,
                    Status = true,
                    Department_Id = employee.Department_Id,
                };
                context.Employees.Add(emp);
                var result = context.SaveChanges();
                return result;
            }
        }

        public int Update(EmployeeVM employee)
        {
            var check = CheckPhoneNumber(employee.NIK, employee.PhoneNumber);

            if (check)
            {
                return -1;
            }
            else
            {
                var data = context.Employees.AsNoTracking().SingleOrDefault(e => e.NIK == employee.NIK);
                string emailOld = data.Email;

                var email = GenerateEmail(employee.FirstName, employee.LastName);
                if (emailOld != email)
                {
                    Employee emp = new Employee
                    {
                        NIK = employee.NIK,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = email,
                        PhoneNumber = employee.PhoneNumber,
                        Address = employee.Address,
                        Status = employee.Status,
                        Department_Id = employee.Department_Id
                    };
                    context.Entry(emp).State = EntityState.Modified;
                    return context.SaveChanges();
                }
                else
                {
                    Employee emp = new Employee
                    {
                        NIK = employee.NIK,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = emailOld,
                        PhoneNumber = employee.PhoneNumber,
                        Address = employee.Address,
                        Status = employee.Status,
                        Department_Id = employee.Department_Id
                    };
                    context.Entry(emp).State = EntityState.Modified;
                    return context.SaveChanges();
                }
            }
        }

        public int Delete(string NIK)
        {
            var employee = context.Employees.Find(NIK);
            employee.Status = false;
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }

        public string GenerateNIK()
        {
            string date = DateTime.Now.ToString("ddMMyy");
            var last = context.Employees.OrderByDescending(e => e.NIK).FirstOrDefault();
            if (last == null)
            {
                return date + "001";
            }
            else
            {
                string lastId = last.NIK.Substring(last.NIK.Length - 3);
                int numberId = int.Parse(lastId) + 1;
                return date + numberId.ToString("000");
            }
        }
        public string GenerateEmail(string firstName, string lastName)
        {
            string baseEmail = $"{firstName.ToLower()}.{lastName.ToLower()}";
            string generatedEmail = $"{baseEmail}{domain}";
            int counter = 1;

            // Check if the generated username already exists in the database
            while (context.Employees.Any(u => u.Email == generatedEmail))
            {
                generatedEmail = $"{baseEmail}{counter:D3}{domain}"; // Append a three-digit number
                counter++;

                // To avoid infinite loops, you can add a maximum number of retries here.
                if (counter > 999)
                {
                    throw new Exception("Unable to generate a unique username.");
                }
            }
            return generatedEmail;
        }

        public bool CheckPhoneNumber(string nik, string phone)
        {
            var result = context.Employees.AsNoTracking().SingleOrDefault(e => e.PhoneNumber == phone);

            if (result == null)
            {
                return false;
            }
            else if(result.NIK == nik)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IEnumerable<object> GetActiveEmloyee()
        {
            var result = context.Employees.Include(d => d.department).ToList().Where(e => e.Status == true)
                .Select(a => new EmployeeDataVM
                {
                    NIK = a.NIK,
                    FulName = a.FirstName + " " + a.LastName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Address = a.Address,
                    DepartmentName = a.department.Name
                });

            return result;
        }

        public IEnumerable<object> GetResignEmloyee()
        {
            var result = context.Employees.Join(context.Departments,
                                    emp => emp.Department_Id,
                                    dept => dept.Dept_Id,
                                    (emp, dept) => new { emp, dept }
                                   ).Where(e => e.emp.Status == false)
                                   .Select(r => new EmployeeDataVM
                                   {
                                       NIK = r.emp.NIK,
                                       FulName = r.emp.FirstName + " " + r.emp.LastName,
                                       Email = r.emp.Email,
                                       PhoneNumber= r.emp.PhoneNumber,
                                       Address = r.emp.Address,
                                       DepartmentName = r.dept.Name
                                   }).ToList();
            return result;
        }

        public IEnumerable<object> GetActiveEmloyee(string dept_id)
        {
            var checkDeptId = context.Departments.Find(dept_id);
            if (checkDeptId == null)
            {
                return null;
            }
            var activeEmployee = context.Employees.Include(d => d.department).ToList()
                .Where(e => e.Status == true && e.Department_Id == dept_id)
                .Select(a => new EmployeeDataVM
            {
                NIK = a.NIK,
                FulName = a.FirstName + " " + a.LastName,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                Address = a.Address,
                DepartmentName = a.department.Name
            });
            return activeEmployee;
        }

        public IEnumerable<object> GetResignEmloyee(string dept_id)
        {
            var result = context.Employees.Join(context.Departments,
                                    emp => emp.Department_Id,
                                    dept => dept.Dept_Id,
                                    (emp, dept) => new { emp, dept }
                                   ).Where(e => e.emp.Status == false && e.emp.Department_Id == dept_id)
                                   .Select(r => new EmployeeDataVM
                                   {
                                       NIK = r.emp.NIK,
                                       FulName = r.emp.FirstName + " " + r.emp.LastName,
                                       Email = r.emp.Email,
                                       PhoneNumber = r.emp.PhoneNumber,
                                       Address = r.emp.Address,
                                       DepartmentName = r.dept.Name
                                   }).ToList();
            return result;
        }

        public IEnumerable<object> GetActiveEmployeeCount()
        {
            var deptCount = context.Employees.Where(e => e.Status == true)
                .GroupBy(e => e.department.Name)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    EmployeesCount = g.Count(),
                }).ToList();
            return deptCount;
        }

        public IEnumerable<object> GetResignEmployeeCount()
        {
            var result = context.Employees.Join(context.Departments,
                                    emp => emp.Department_Id,
                                    dept => dept.Dept_Id,
                                    (emp, dept) => new { emp, dept }
                                   ).Where(e => e.emp.Status == false)
                                   .GroupBy(g => g.dept.Name, (g) => new { g })
                                   .Select(g => new EmployeeCountVM
                                   {
                                       DepartmentName = g.Key,
                                       EmployeesCount = g.Count()
                                   }).ToList();
            return result;
        }
    }
}
