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
    public class CustomerController:ApiController
    {
        [HttpPost]

        public  HttpResponseMessage saveCustomer (Customer dataString)
        {
            try
            {
                if(string.IsNullOrEmpty (dataString.name))
                {
                    return Return.returnHttp("201", "please enter your name...");
                }
                if(string.IsNullOrEmpty(dataString.email))
                {
                    return Return.returnHttp("201", "please enter valid email");
                }
                if (string.IsNullOrEmpty(dataString.password))
                {
                    return Return.returnHttp("201", "please enter your valid password...");
                }
                if (string.IsNullOrEmpty(dataString.mobileNo))
                {
                    return Return.returnHttp("201", "please enter valid mobileNo");
                }
                if (string.IsNullOrEmpty(dataString.address))
                {
                    return Return.returnHttp("201", "please enter your valid address ...");
                }

                string query;
                if(string.IsNullOrEmpty(dataString.id.ToString()))
                {
                    string strid = Database.getValue("select isNull(max (id)+1,1) from customer");
                    query = "insert into customer values(" + strid + ",'" + dataString.name + "','" + dataString.email + "','" + dataString.password + "','" + dataString.gender + "','" + dataString.mobileNo + "','" + dataString.address + "' ,1)";
                }
                else
                {
                    query= "update customer set name='"+dataString.name+ "',email='" + dataString.email + "',gender ='" + dataString.gender + "',mobile_no ='" + dataString.mobileNo + "',address='" + dataString.address + "' where id="+dataString.id;
                }

                string output = Database.insertData(query);
                if (output != "200")
                {
                    return Return.returnHttp("201", "some error occured");
                }
                return Return.returnHttp("200", "customer saved succesfully");
            }
            catch (Exception e)
            {
                return Return.returnHttp("201","some error occured");
            }
        }
        [HttpPost]
        public HttpResponseMessage getCustomer (Customer dataString)
        {
            try
            {
                string query = "select * from customer where status=1";
                DataSet ds = new DataSet();
                ds = Database.getDataSet(query);
                if(ds==null)
                {
                    return Return.returnHttp("201", "no record found");
                }
                DataTable dt = ds.Tables[0];
                List<Customer> customerlist = new List<Customer>();
                int count = dt.Rows.Count, i = 0;
                while(i<count)
                {
                    Customer element = new Customer();
                    element.id = Convert.ToInt32(dt.Rows[i][0].ToString());
                    element.name = dt.Rows[i][1].ToString();
                    element.email = dt.Rows[i][2].ToString();                 
                    element.gender= dt.Rows[i][4].ToString();
                    element.mobileNo = dt.Rows[i][5].ToString();
                    element.address= dt.Rows[i][6].ToString();
                    element.status =Convert.ToBoolean( dt.Rows[i][7].ToString());
                    customerlist.Add(element);
                    i++;
                }
                return Return.returnHttp("200", customerlist);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}