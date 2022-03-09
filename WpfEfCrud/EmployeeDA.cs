using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;

namespace WpfEfCrud
{
    static class EmployeeDA
    {
        private static readonly Northwind db = new Northwind();
        public static List<Employee> GetEmployees()
        {
            return db.Employees.ToList();
        }

        public static int AddEmployee(Employee e)
        {
            try
            {
                db.Employees.Add(e);
                db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                db.Entry(e).State = EntityState.Detached;
                return -1;
            }
        }
        public static int EditEmployee(Employee e)
        {
            try
            {
                db.Entry(e).State = EntityState.Modified;
                db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                db.Entry(e).State = EntityState.Unchanged;
                return -1;
            }
        }
        public static int DeleteEmployee(Employee e)
        {
            try
            {
                db.Entry(e).State = EntityState.Deleted;
                db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                db.Entry(e).State = EntityState.Unchanged;
                return -1;
            }
        }


    }
}
