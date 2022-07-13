using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ecommerce.Controllers
{
    public class AdminController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage login(login dataString)
        {
            try
            {
                if (string.IsNullOrEmpty(dataString.userName))
                {
                    return Return.returnHttp("201", "please enter valid name");
                }

                if (string.IsNullOrEmpty(dataString.password))
                {
                    return Return.returnHttp("201", "please enter valid password");
                }

                string query;
                string strid = Database.getValue("select id from admin where email = '"+dataString.userName+"' and password='"+dataString.password+"'");
                
                if (string.IsNullOrEmpty(strid))
                {
                    return Return.returnHttp("201", "Incorrect username password");
                }
                return Return.returnHttp("201", strid);
            }
            catch (Exception e)
            {
                return Return.returnHttp("201", "some error occured");
            }
        }


        [HttpPost]
        public HttpResponseMessage saveAdmin(Admin dataString)
        {
            try
            {
                if (string.IsNullOrEmpty(dataString.name))
                {
                    return Return.returnHttp("201", "please enter valid name");
                }

                if (string.IsNullOrEmpty(dataString.email))
                {
                    return Return.returnHttp("201", "please enter valid email");
                }

                if (string.IsNullOrEmpty(dataString.password))
                {
                    return Return.returnHttp("201", "please enter valid password");
                }

                string query;
                if (string.IsNullOrEmpty(dataString.id.ToString()))
                {
                    string strid = Database.getValue("select isNull(max (id)+1,1) from admin");
                    query = " insert into admin values(" + strid + ",'" + dataString.name + "','" + dataString.email + "','" + dataString.password + "',1)";
                }
                else
                {
                    query = "update admin set name='" + dataString.name + "',email ='" + dataString.email + "' where id=" + dataString.id;
                }
                string output = Database.insertData(query);
                if (output != "200")
                {
                    return Return.returnHttp("201", "some error occured");
                }
                return Return.returnHttp("200", "admin saved successfully");
            }
            catch (Exception e)
            {
                return Return.returnHttp("201", "some error occured");
            }
        }

        [HttpPost]
        
            public HttpResponseMessage getAdmin (Admin dataString)
        {
            try
            {
                string query = "select * from admin where status=1";
                DataSet ds = new DataSet();
                ds = Database.getDataSet(query);
                if(ds==null)
                {
                    return Return.returnHttp("201", "record not found");
                }
                DataTable dt = ds.Tables[0];
                List<Admin> adminList= new List<Admin>();
                int count = dt.Rows.Count,i=0;
                while (i < count)
                {
                    Admin element = new Admin();
                    element.id = Convert.ToInt32(dt.Rows[i][0].ToString());
                    element.name = dt.Rows[i][1].ToString();
                    element.email = dt.Rows[i][2].ToString();
                    element.status = Convert.ToBoolean(dt.Rows[i][4].ToString ());
                    adminList.Add(element);
                    i++;
                }
                return Return.returnHttp("200", adminList);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpPost]
        public HttpResponseMessage enableAdmin(Admin dataString)
        {
            try
            {
                if(String.IsNullOrEmpty(dataString.id.ToString()))
                {
                    return Return.returnHttp("201", "Please select admin, it's mandatory");
                }
                string query = "update admin set status=1 where id=" + dataString.id;
                string output = Database.insertData(query);
                if(output!="200")
                {
                    return Return.returnHttp("201", "Some internal issue occured.");
                }
                return Return.returnHttp("200", "Admin enabled Successfully.");
            }
            catch (Exception e)
            {

                throw;
            }
        }
        [HttpPost]
        public HttpResponseMessage disableAdmin(Admin dataString)
        {
            try
            {
                if(String.IsNullOrEmpty(dataString.id.ToString()))
                {
                    return Return.returnHttp("201", "Please select admin, it's mandatory");
                }
                string query = "update admin set status=0 where id=" + dataString.id;
                string output = Database.insertData(query);
                if(output!="200")
                {
                    return Return.returnHttp("201", "Some internal issue occured.");
                }
                return Return.returnHttp("200", "Admin Disable Successfully.");
            }
            catch (Exception e)
            {

                throw;
            }
        }

     }
}