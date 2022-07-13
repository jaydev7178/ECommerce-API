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
    public class CartController :ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveCart(Cart datastring)
        {
            try
            {
                if(string.IsNullOrEmpty(datastring.customerId.ToString()))
                {
                    return Return.returnHttp("201", "invalid login id");
                }
                if(string.IsNullOrEmpty(datastring.productId.ToString()))
                {
                    return Return.returnHttp("201", "invalid product id..");
                }
                if(string.IsNullOrEmpty(datastring.quantity.ToString()))
                {
                    return Return.returnHttp("201", "invalid quantity");
                }
                string query;
                if(string.IsNullOrEmpty(datastring.id.ToString()))
                {
                    string strid = Database.getValue("select isNull(max (id)+1,1) from cart");
                    query = "insert into cart values(" + strid + "," + datastring.productId + "," + datastring.customerId + "," + datastring.quantity + ",1)";
                    
                }
                else
                {
                    query = "update cart set product_id =" + datastring.productId + ",customer_id=" + datastring.customerId + ",quantity=" + datastring.quantity + ",1";
                }

                string output = Database.insertData(query);
                if(output !="200")
                {
                    return Return.returnHttp("201", "some error occured");
                }
                return Return.returnHttp("201", "Successful");
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public HttpResponseMessage getCart(Cart dataString)
        {
            try
            {
                string query = "select * from cart where status =1";
                DataSet ds = new DataSet();
                ds = Database.getDataSet(query);
                if(ds == null)
                {
                    return Return.returnHttp("201", "record not found");
                }
                DataTable dt = ds.Tables[0];
                List<Cart> cartlist = new List<Cart>();
                int count = dt.Rows.Count, i = 0;
                while (i<count )
                {
                    Cart element = new Cart();
                    element.id = Convert.ToInt32(dt.Rows[i][0].ToString());
                    element.productId = Convert.ToInt32(dt.Rows[i][1].ToString());
                    element.customerId = Convert.ToInt32(dt.Rows[i][2].ToString());
                    element.quantity = Convert.ToInt32(dt.Rows[i][3].ToString());
                    element.status = Convert.ToBoolean(dt.Rows[i][4].ToString());
                    cartlist.Add(element);
                    i++;
                }
                return Return.returnHttp("200", cartlist);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        [HttpPost]
        public HttpResponseMessage enableCart(Cart dataString)
        {
            try
            {
                if (string.IsNullOrEmpty(dataString.id.ToString()))
                {
                    return Return.returnHttp("201", "please select Cart,its mandotary");
                }
                string query = "update Cart set status = 1 where id=" + dataString.id;
                string output = Database.insertData(query);
                if(output !="200")
                {
                    return Return.returnHttp("201", "some internal issue occured ");
                }
                return Return.returnHttp("200", "Cart enable successfully");
            }
            catch (Exception e)
            {

                throw;
            }
        }
        [HttpPost]
        public HttpResponseMessage disableCart(Cart dataString)
        {
            try
            {
                if (string.IsNullOrEmpty(dataString.id.ToString()))
                {
                    return Return.returnHttp("201", "please select Cart,its mandotary");
                }
                string query = "update Cart set status = 0 where id=" + dataString.id;
                string output = Database.insertData(query);
                if (output != "200")
                {
                    return Return.returnHttp("201", "some internal issue occured ");
                }
                return Return.returnHttp("200", "Cart disable  successfully");
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}