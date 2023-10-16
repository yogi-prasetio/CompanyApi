using BercaAPI.Models;
using BercaAPI.Repository;
using BercaAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BercaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private DepartmentRepository departmentRepository;

        public DepartmentController(DepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
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