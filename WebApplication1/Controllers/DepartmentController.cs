using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ConnectionClass
    {
        public SqlConnection cs = new SqlConnection(ConfigurationManager.ConnectionStrings["DBEmployee"].ConnectionString);
        public void Running()
        {
            try 
            { 
                cs.Open();
            }
            catch (Exception ex)
            {
            Console.WriteLine(ex.Message);  
            }
        }
        public void Stop()
        {
            try 
            { 
                if (cs != null)
                {
                   
                    cs.Close();
                    
                }
            }
            catch (Exception ex) {
            Console.WriteLine(ex.Message);  
            }
        }
    }

    public class DepartmentController : ApiController
    {
        private readonly ConnectionClass con = new ConnectionClass();
        
        public static SqlCommand cmd = null;
        private string query = string.Empty;
        public HttpResponseMessage Get()
        {
            try
            {
                query = "Select * From tblDepartment";
                con.Running();
                cmd = new SqlCommand(query, con.cs);
                cmd.ExecuteNonQuery();
                con.Stop();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return Request.CreateResponse(HttpStatusCode.OK,dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string post(Department dept )
        {
            try
            {
                query = @"Insert into tblDepartment values('"+dept.DepartmentName+"')";
                con.Running();
                cmd = new SqlCommand(query,con.cs);
                cmd.ExecuteNonQuery();
                con.Stop();
              
                return "Added Successfully";
            }
            catch (Exception )
            {
                return "Falied to Insert Add!";
            }
            
        }
        public string put(Department dept)
        {
            try
            {
                query = @"Update tblDepartment set DepartmentName='" + dept.DepartmentName + "' where DepartmentId = '"+dept.DepartmentId+"'";
                con.Running();
                cmd = new SqlCommand(query, con.cs);
                cmd.ExecuteNonQuery();
                con.Stop();
                return "Updated Successfully";
            }
            catch (Exception )
            {
                return "Falied to Updated!";
            }

        }
        public string Delete(int Id)
        {
            try
            {
                query = @"Delete from tblDepartment where DepartmentId = '"+Id+"'";
                con.Running();
                cmd = new SqlCommand(query, con.cs);
                cmd.ExecuteNonQuery();
                con.Stop();

                return "Deleted Successfully";
            }
            catch (Exception)
            {
                return "Falied to Deleted!";
            }

        }


    }
}
