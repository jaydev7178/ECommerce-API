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
    public class DeliveryController : ApiController
    {
       

        [HttpPost]

        public HttpResponseMessage saveDelivery (Delivery datastring)
        {
            try
            {
                if(string.IsNullOrEmpty(datastring.CustomerId.ToString()))
                {
                    return Return.returnHttp("201", "please enter valid custimer id");
                }
                if (string.IsNullOrEmpty(datastring.ProductId.ToString()))
                {
                    return Return.returnHttp("201", "please enter valid product id");
                }
                if (string.IsNullOrEmpty(datastring.Delivered.ToString()))
                {
                    return Return.returnHttp("201", "please enter delivered ");
                }
                string query;
                if(string.IsNullOrEmpty(datastring.Id.ToString()))
                {
                    string strid = Database.getValue("select isNull(max((id)+1),1) from delivery");
                    query = "insert into delivery values(" + strid + "," + datastring.CustomerId + "," + datastring.ProductId + ",'" + datastring.Delivered + "',1)";
                }
                else
                {
                    query = "update delivery into customer_id=" + datastring.CustomerId + ",product_id=" + datastring.ProductId + ",delivered='" + datastring.Delivered + "'";
                }
                string output = Database.insertData(query);
                if(output != "200")
                {
                    return Return.returnHttp("201", " some error occured ");
                }
                return Return.returnHttp("200", "saved successfully");


            }
            catch (Exception e)
            {

                return Return.returnHttp("201","some error occured");
            }
        }
        [HttpPost]
        public HttpResponseMessage getDelivery (Delivery dataString)
        {
            try
            {
                string query = "select * from delivery where status =1";
                DataSet ds = new DataSet();
                ds = Database.getDataSet(query);
                if(ds== null)
                {
                    return Return.returnHttp("201", "no record found");
                }
                DataTable dt = ds.Tables[0];
                List<Delivery> deliverylist = new List<Delivery>();
                int count = dt.Rows.Count, i = 0;
                while (i < count) 
                {
                    Delivery element = new Delivery();
                    element.Id = Convert.ToInt32(dt.Rows[i][0].ToString());
                    element.CustomerId = Convert.ToInt32(dt.Rows[i][1].ToString());
                    element.ProductId = Convert.ToInt32(dt.Rows[i][2].ToString());
                    element.Delivered = Convert.ToBoolean(dt.Rows[i][3].ToString());
                    element.status = Convert.ToBoolean(dt.Rows[i][4].ToString());
                    deliverylist.Add(element);
                    i++;
                }
                return Return.returnHttp("200", deliverylist);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}