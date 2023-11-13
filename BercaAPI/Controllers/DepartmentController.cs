using BercaAPI.Context;
using BercaAPI.Models;
using BercaAPI.Repository;
using BercaAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace BercaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private DepartmentRepository departmentRepository;
        private readonly MyContext context;
        public DepartmentController(DepartmentRepository departmentRepository, MyContext context)
        {
            this.departmentRepository = departmentRepository;
            this.context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var dept = departmentRepository.Get();
                if (dept == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Department Success", dept);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("EmployeStatusPerDepartment")]
        public ActionResult EmployeStatusPerDepartment()
        {
            try
            {
                var result = departmentRepository.GetStatusEmployeePerDepartment();

                return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Data Success", result);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("EmployePerDepartment")]
        public ActionResult EmployePerDepartment()
        {
            try
            {
                var result = departmentRepository.GetEmployeeCountPerDepartment();

                return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Data Success", result);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("Paging")]
        public ActionResult GetPaging()
        {
            try
            {
                int totalRecord = 0;
                int filterRecord = 0;

                var draw = Request.Form["draw"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                //var data = departmentRepository.Get().AsQueryable();

                ////get total count of data in table
                //totalRecord = data.Count();
                //// search data when search value found
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    data = data.Where(x => x.Dept_Id.ToLower().Contains(searchValue.ToLower()) || x.Name.ToLower().Contains(searchValue.ToLower()));
                //}

                //// get total count of records after search
                //filterRecord = data.Count();

                ////sort data
                //if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) data = data.OrderBy(sortColumn + " " + sortColumnDirection);
                ////pagination
                //var empList = data.Skip(skip).Take(pageSize).ToList();

                //var returnObj = new
                //{
                //    draw = draw,
                //    recordsTotal = totalRecord,
                //    recordsFiltered = filterRecord,
                //    data = empList
                //};

                //return Ok(returnObj);
                var dataTable = new JqueryDatatableParam
                {
                    Draw = draw,
                    SortColumn = sortColumn,
                    SortColumnDirection = sortColumnDirection,
                    SearchValue = searchValue,
                    PageSize = pageSize,
                    Skip = skip,
                };
                var data = departmentRepository.GetDataPaging(dataTable);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }        

        [HttpGet("{ID}")]
        public ActionResult Get(string ID)
        {
            try
            {
                var dept = departmentRepository.Get(ID);
                if (dept == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Department Not Found");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Department Success", dept);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Insert(DepartmentVM department)
        {
            try
            {
                var result = departmentRepository.Insert(department);
                if (result > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Add data successfully");
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
        public ActionResult Update(DepartmentVM department)
        {
            try
            {
                var result = departmentRepository.Update(department);
                if (result > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Update data successfully");
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
        [HttpDelete("{ID}")]
        public ActionResult Delete(string ID)
        {
            try
            {
                var delete = departmentRepository.Delete(ID);
                if (delete > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Delete successfully");
                }
                else if (delete < 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotModified, "Delete failed, because there is still employee data in this department!");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Department Not Found");
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}