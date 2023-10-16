using BercaAPI.Models;
using BercaAPI.ViewModel;

namespace BercaAPI.Repository.Interface
{
    interface EmployeeInterface
    {
        IEnumerable<object> Get();
        Employee Get(string NIK);
        int Insert(EmployeeVM employee);
        int Update(EmployeeVM employee);
        int Delete(string NIK);

        IEnumerable<object> GetActiveEmloyee();
        IEnumerable<object> GetResignEmloyee();

        IEnumerable<object> GetActiveEmloyee(string dept_id);
        IEnumerable<object> GetResignEmloyee(string dept_id);

        IEnumerable<object> GetActiveEmployeeCount();
        IEnumerable<object> GetResignEmployeeCount();
    }
}
