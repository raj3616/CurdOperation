using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeeController : ApiController
    {
        readonly ConnectionClass con = new ConnectionClass();

        public static SqlCommand cmd = null;
        private string query = string.Empty;
        public HttpResponseMessage Get()
        {
            try
            {
                query = "Exec SelectForEmp";
                con.Running();
                cmd = new SqlCommand(query, con.cs);
                cmd.ExecuteReader();
                con.Stop();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string post(Employee Emp)
        {
            string FileData = "";

            try
            {

                //recieve formdata, create base64string and prefix and save in variable  FileData

                query = @"Insert into tblEmployee values('"+Emp.EmployeeName+ "','"+Emp.Department+"','"+Emp.DateOfJoining+"','"+Emp.PhotoFileName+"','"+Emp.FileData+"')";

                con.Running();
                cmd = new SqlCommand(query, con.cs);
                int i = cmd.ExecuteNonQuery();
                con.Stop();
                if (i > 0)
                {
                    return "Added Successfully:"+i;
                }
                return "Falied to Insert!";

            }
            catch (Exception)
            {
                return "Falied to Insert Add!";
            }

        }
        public string put(Employee Emp)
        {
            try
            {
                query = @"Update tblEmployee set 
                            EmployeeName = '" + Emp.EmployeeName + @"',
                            Department = '" + Emp.Department + @"',
                            DateOfJoining = '" + Emp.DateOfJoining + @"',
                            PhotoFileName  = '" + Emp.PhotoFileName +"' where EmployeeId = '"+Emp.EmployeeId+"'";
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
        public string Delete(int id)
        {
            try
            {

                query = @"Delete from tblEmployee where EmployeeId = '" +id+ "'";
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


        [Route("api/Employee/GetFileDataByEmpIds/{EmpId}")]
        [HttpGet]
        public HttpResponseMessage GetFileDataByEmpId([FromUri]string EmpId)
        {
            try
            {
                query = "select FilePrefix,FileData from tblEmployee where EmployeeId = " + EmpId;
                con.Running();
                cmd = new SqlCommand(query, con.cs);
                cmd.ExecuteReader();
                con.Stop();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("api/Employee/GetAllDepartmentNames")]
        [HttpGet]
        public HttpResponseMessage GetAllDepartmentName()
        {
            try
            {
                query = @"Select DepartmentName from tblDepartment";
                con.Running();
                cmd = new SqlCommand(query, con.cs);
                cmd.ExecuteNonQuery();
                con.Stop();
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("api/Employee/saveFile")]
        public string saveFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalpath = HttpContext.Current.Server.MapPath("~/Photos/" + filename);

                postedFile.SaveAs(physicalpath);

                return filename;
            }
            catch (Exception)
            {
                return "AnonymousFile.png";
            }
        }
    }
}
