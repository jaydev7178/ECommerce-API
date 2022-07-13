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
    public class ProductController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage saveProduct(Product dataString)
        {
            try
            {
                if (string.IsNullOrEmpty(dataString.name))
                {
                    return Return.returnHttp("201", "invalid name..");
                }
                if (string.IsNullOrEmpty(dataString.quantity.ToString()))
                {
                    return Return.returnHttp("201", "invalid quantity");
                }
                if (string.IsNullOrEmpty(dataString.price.ToString()))
                {
                    return Return.returnHttp("201", "invalid price..");
                }
                if (string.IsNullOrEmpty(dataString.categoryId.ToString()))
                {
                    return Return.returnHttp("201", "please select valid categoery");
                }
                string query;
                if (string.IsNullOrEmpty(dataString.id.ToString()))
                {
                    string strid = Database.getValue("select  isnull(max (id)+1,1) from product ");
                    query = "insert into product values (" + strid + ",'" + dataString.name + "'," + dataString.quantity + "," + dataString.price + ",'" + dataString.description + "'," + dataString.categoryId + ",1)";
                }
                else
                {
                    query = "update product set name= '" + dataString.name + "',quantity = " + dataString.quantity + ",price = " + dataString.price + ",description = '" + dataString.description + "',category = " + dataString.categoryId + ",1 ";
                }
                string output = Database.insertData(query);
                if(output != "200")
                {
                    return Return.returnHttp("201", "some error occured");
                }
                return Return.returnHttp("200", "product saved successfully");
            }

            catch (Exception e)
            {
                return Return.returnHttp("201","uncsuccessfull");
            }
        }
        [HttpPost]
        public HttpResponseMessage getProduct(Product dataString)
        {
            try
            {
                    string query = "select * from product where status=1";
                DataSet ds = new DataSet();
                ds = Database.getDataSet(query);
                if(ds==null)
                {
                    return Return.returnHttp("201", "record not fond");
                }
                DataTable dt = ds.Tables[0];
                List<Product> productList = new List<Product>();
                int count = dt.Rows.Count, i = 0;
                while(i<count)
                {
                    Product element = new Product();
                    element.id = Convert.ToInt32(dt.Rows[i][0].ToString());
                    element.name = dt.Rows[i][1].ToString();
                    element.quantity = Convert.ToInt32(dt.Rows[i][2].ToString());
                    element.price = Convert.ToInt32(dt.Rows[i][3].ToString());
                    element.description = dt.Rows[i][4].ToString();
                    element.categoryId= Convert.ToInt32(dt.Rows[i][5].ToString());
                    element.status = Convert.ToBoolean(dt.Rows[i][6].ToString());
                    productList.Add(element);
                    i++;
                }
                return Return.returnHttp("200", productList);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        [HttpPost]
        public HttpResponseMessage enableProduct (Product dataString)
        {
            try
            {
                if (string.IsNullOrEmpty(dataString.id.ToString()))
                {
                    return Return.returnHttp("201", "please select product , its mandotary");
                }
                string query = "update admin set status=1 where id=" + dataString.id;
                string output = Database.insertData(query);
                if(output!="200")
                {
                    return Return.returnHttp("201", "some internal issue occured");
                }
                return Return.returnHttp("200", "Product enable succesfully");
            }
            catch (Exception e)
            {

                throw;
            }
        }
        [HttpPost]
        public HttpResponseMessage disableProduct(Product dataString)
        {
            try
                {
                if (string.IsNullOrEmpty(dataString.id.ToString()))
                {
                    return Return.returnHttp("201", "please select product , its mandotary");
                }
                string query = "update admin set status=0 where id=" + dataString.id;
                string output = Database.insertData(query);
                if (output != "200")
                {
                    return Return.returnHttp("201", "some internal issue occured");
                }
                return Return.returnHttp("200", "Product disable succesfully");
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}