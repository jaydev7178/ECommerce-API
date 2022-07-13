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
    public class CategoryController:ApiController
    {
        [HttpPost]

        public HttpResponseMessage saveCategory(Category dataString)
        {
            try
            {
                if(string.IsNullOrEmpty(dataString.name))
                {
                    return Return.returnHttp("201", "please enter valid name");
                }
                string query;
                if (string.IsNullOrEmpty(dataString.id.ToString()))
                {
                    string strid = Database.getValue("select isnull(max(id)+1,1)from admin");
                    query = "insert into Category values(" + strid + ",'" + dataString.name + "',1)";
                }
                else
                {
                    query = "update category set name='" + dataString.name + "' where id="+dataString.id;
                }
                string output = Database.insertData(query);
                if(output!= "200")
                {
                    return Return.returnHttp("201", "some errored occured ");
                }
                return Return.returnHttp("200", "saved succesfully ");

            }
            catch (Exception e)
            {   

                return Return.returnHttp("201","some internal error occured");
            }
        }
        [HttpPost]
        public HttpResponseMessage getCategory(Category dataString)
        {
            try
            {
                string query = "select * from category where status =1";
                DataSet ds = new DataSet();
                ds = Database.getDataSet(query);
                if(ds == null)
                {
                    return Return.returnHttp("201", "record not found ");
                }
                DataTable dt = ds.Tables[0];
                List<Category> categorylist = new List<Category>();
                int count = dt.Rows.Count, i = 0;
                while (i<count)
                {
                    Category element = new Category();
                    element.id = Convert.ToInt32(dt.Rows[i][0].ToString());
                    element.name =(dt.Rows[i][1].ToString());
                    element.status = Convert.ToBoolean(dt.Rows[i][2].ToString());
                    categorylist.Add(element);
                    i++;
                }
                return Return.returnHttp("200",categorylist);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}