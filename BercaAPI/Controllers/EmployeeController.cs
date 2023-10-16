using BercaAPI.Models;
using BercaAPI.Repository;
using BercaAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace BercaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private EmployeeRepository employeeRepository;

        public EmployeeController(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var emp = employeeRepository.Get();
                if (emp == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            try
            {
                var emp = employeeRepository.Get(NIK);
                if (emp == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Employee Not Found", emp);
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Insert(EmployeeVM employee)
        {
            try
            {
                var result = employeeRepository.Insert(employee);
                if (result > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Add data successfully");
                }
                else if (result < 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotAcceptable, "Sorry, phone number is registered");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Add data failed");
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        public ActionResult Update(EmployeeVM employee)
        {
            try
            {
                var result = employeeRepository.Update(employee);
                if (result > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Update data successfully");
                }
                else if (result < 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotAcceptable, "Sorry, phone number is registered");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotModified, "Update data failed");
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            try
            {
                var delete = employeeRepository.Delete(NIK);
                if (delete > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Delete successfully");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Employee Not Found");
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("ActiveEmployee")]
        public ActionResult GetActiveEmployee()
        {
            try
            {
                var emp = employeeRepository.GetActiveEmloyee();
                if (emp == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Active Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("ResignEmployee")]
        public ActionResult GetResignEmployee()
        {
            try
            {
                var emp = employeeRepository.GetResignEmloyee();
                if (emp == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Resign Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("ActiveEmployee/{DeptId}")]
        public ActionResult GetActiveEmployee(string DeptId)
        {
            try
            {
                var emp = employeeRepository.GetActiveEmloyee(DeptId);
                if (emp.IsNullOrEmpty())
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Active Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("ResignEmployee/{DeptId}")]
        public ActionResult GetResignEmployee(string DeptId)
        {
            try
            {
                var emp = employeeRepository.GetResignEmloyee(DeptId);
                if (emp.IsNullOrEmpty())
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Resign Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("ActiveEmployeeCount")]
        public ActionResult GetActiveEmployeeCount()
        {
            try
            {
                var emp = employeeRepository.GetActiveEmployeeCount();
                if (emp.IsNullOrEmpty())
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Active Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("ResignEmployeeCount")]
        public ActionResult GetResignEmployeeCount()
        {
            try
            {
                var emp = employeeRepository.GetResignEmployeeCount();
                if (emp.IsNullOrEmpty())
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Resign Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("Coba")]
        public ActionResult TestCore()
        {
            return Ok("Core Success");
        }
    }
}
