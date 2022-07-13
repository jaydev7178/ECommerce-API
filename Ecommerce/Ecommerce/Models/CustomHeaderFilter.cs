
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Ecommerce.Models
{

    public class CustomHeaderFilter : ActionFilterAttribute
    {
        //public static Logger logger = LogManager.GetCurrentClassLogger();
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string controllerNm = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string actionNm = actionContext.ActionDescriptor.ActionName;

            //logger.Info("ActionName=" + actionNm + ", controllerName =" + controllerNm);
            if (actionNm != "login" && controllerNm != "Web" && controllerNm!= "EmployeeOTP" && actionNm!= "getClientCompanyBrief" && actionNm!= "generateCreditLedgerInvoice"&& actionNm!= "generateOTP" && actionNm!= "getActiveCountryList" && actionNm!= "registerApplicationUsers")
            {
                string token = actionContext.Request.Headers.GetValues("token").FirstOrDefault();
                //logger.Info("ActionName=" + actionNm + ", controllerName =" + controllerNm + ", token =" + token);
                string validated = Function.ValidateToken(token);

                if (!validated.StartsWith("INVALID"))
                    base.OnActionExecuting(actionContext);
                else
                {

                }
            }

        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            string controllerNm = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string actionNm = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            int? id;

            

            if (actionNm != "login" && controllerNm != "Web" && controllerNm != "EmployeeOTP" && actionNm != "getClientCompanyBrief" && actionNm != "generateCreditLedgerInvoice" && actionNm != "generateOTP" && actionNm != "getActiveCountryList" && actionNm != "registerApplicationUsers")
            {
                actionExecutedContext.Response.Content.Headers.Add("Access-Control-Expose-Headers", "token");
                string token = actionExecutedContext.Request.Headers.GetValues("token").FirstOrDefault();
                //string loginas = actionExecutedContext.Request.Headers.GetValues("loginas").FirstOrDefault();
                //if (controllerNm == "AppAdmin")
                //{

                string validateId = Function.ValidateToken( token);
                string[] strarr = validateId.Split(':');
                string loginas = strarr[0];
                id = Convert.ToInt32(strarr[1]);
                //sid = Int32.Parse(createdById);
                actionExecutedContext.Response.Content.Headers.Add("token", Function.CreateToken(loginas, id));
                //actionExecutedContext.Response.Content.Headers.Add("loginas", loginas);
                //    }
                //    else
                //    {
                //        id = Int32.Parse(Function.ValidateToken("client", token));
                //        actionExecutedContext.Response.Content.Headers.Add("token", Function.CreateToken("client", id));
                //    }
            }

        }
    }

}