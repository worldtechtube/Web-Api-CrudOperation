using StudentApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace StudentApi.Controllers
{
    [RoutePrefix("Api/Student")]
    public class StudentController : ApiController
    {
        // configure connection string according to your system
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-M3EM8R3\MSSQLSERVER01;Initial Catalog=StudentDB;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        [HttpGet]
        [Route("AllStudentsDetails")]
        public JsonResult<List<Models.StudentClass>> GetAllStudents()
        {
            SqlCommand cmd = new SqlCommand();
            var studList = new List<StudentClass>();
            cmd.Connection = con;
            cmd.Connection.Open();
            IDataRecord dr1;
            cmd.CommandText = "Select * from student";
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                studList.Add(new StudentClass
                {
                    id = Convert.ToInt32(dr.GetValue(0)),
                    name = dr.GetValue(1).ToString(),
                    email = dr.GetValue(2).ToString(),
                    phone = dr.GetValue(3).ToString(),
                    gender = dr.GetValue(4).ToString(),
                    course = dr.GetValue(5).ToString(),
                    sendUpdates = Convert.ToBoolean(dr.GetValue(6).ToString()),
                    address = new AddressClass
                    {
                        street = dr.GetValue(7).ToString(),
                        city = dr.GetValue(8).ToString(),
                        state = dr.GetValue(9).ToString()
                    },
                    bloodGroup = dr.GetValue(10).ToString()
                }) ;

            }
            // This is a temporary flow for demo
            var throwSomeError = false;
            if (throwSomeError)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return Json<List<Models.StudentClass>>(studList);                       
        }
        [HttpGet]
        [Route("GetStudent")]
        public JsonResult<Models.StudentClass> GetStudent(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Connection.Open();
            cmd.CommandText = "Select * from student where id="+id;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                var stud = new StudentClass
                {
                    id = Convert.ToInt32(dr.GetValue(0)),
                    name = dr.GetValue(1).ToString(),
                    email = dr.GetValue(2).ToString(),
                    phone = dr.GetValue(3).ToString(),
                    gender = dr.GetValue(4).ToString(),
                    course = dr.GetValue(5).ToString(),
                    sendUpdates = Convert.ToBoolean(dr.GetValue(6).ToString()),
                    address = new AddressClass
                    {
                        street = dr.GetValue(7).ToString(),
                        city = dr.GetValue(8).ToString(),
                        state = dr.GetValue(9).ToString()
                    },
                    bloodGroup = dr.GetValue(10).ToString()
                };
                return Json(stud);
            }                
            return null;
        }
        [HttpPost]
        [Route("AddStudent")]
        public StudentClass InsertStudent(Models.StudentClass student)
        {
            con.Open();
            cmd = new SqlCommand(
                "insert into student(" +
                "name," +
                " email," +
                " phone," +
                " gender," +
                " course," +
                " bloodGroup," +
                " sendUpdates," +
                " street," +
                " city," +
                " state)" +
                " output INSERTED.ID " +
                "values('" + student.name+ "'," +
                " '" + student.email + "'," +
                " '" + student.phone + "'," +
                " '" + student.gender + "'," +
                " '" + student.course + "', '"
                + student.bloodGroup + "', " + 
                Convert.ToInt16(student.sendUpdates) + 
                ", '" + student.address.street + "', '" 
                + student.address.city + "', '" 
                + student.address.state + "')", con);
            Int32 newId = (Int32)cmd.ExecuteScalar();
            con.Close();
            student.id = newId;
            return newId > 0 ? student : null; 
        }
        [HttpPut]
        [Route("EditStudent")]
        public StudentClass UpdateStudent(Models.StudentClass student)
        {
            con.Open();
            cmd = new SqlCommand("update student set " +
                "name = '" + student.name + "'," +
                " email = '" + student.email + "'," +
                " phone = '" + student.phone + "'," +
                " gender = '" + student.gender + "'," +
                " course = '" + student.course + "'," +
                " bloodGroup = '" + student.bloodGroup + "'," +
                " sendUpdates = " + Convert.ToInt16(student.sendUpdates) + "," +
                " street = '" + student.address.street + "'," +
                " city = '" + student.address.city + "'," +
                " state = '" + student.address.state + "'" +
                " where id = " + student.id, con);
            int x = cmd.ExecuteNonQuery();
            con.Close();
            return x == 1 ? student : null;            
        }
        [HttpDelete]
        [Route("DeleteStudent")]
        public bool DeleteStudent(int id)
        {
            con.Open();
            cmd = new SqlCommand("delete from student where id="+id, con);
            int x = cmd.ExecuteNonQuery();
            con.Close();
            bool status = x == 1 ? true : false;
            return status;
        }
    }
}
