using BercaAPI.Models;
using BercaAPI.ViewModel;

namespace BercaAPI.Repository.Interface
{
    interface DepartmentInterface
    {
        IEnumerable<Department> Get();
        Object GetDataPaging(JqueryDatatableParam body);
        Department Get(string Id);
        int Insert(DepartmentVM department);
        int Update(DepartmentVM department);
        int Delete(string Id);        
    }
}
