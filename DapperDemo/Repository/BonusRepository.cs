using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection db;

        public BonusRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public List<Employee> GetEmployeeWithCompany(int id)
        {
            var sql = "SELECT E.*, C.* FROM Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId";
            if (id != 0)
            {
                sql += " WHERE E.CompanyId = @Id ";
            }

            var employee = db.Query<Employee, Company, Employee>(sql,(empl,comp) =>
            {
                empl.Company = comp;
                return empl;
            },new { id }, splitOn: "CompanyId");

            return employee.ToList();
        }
    }
}
