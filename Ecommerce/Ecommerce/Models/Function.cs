using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Ecommerce.Models
{
    public class Function
    {

        public static string CreateToken(string loginAs, int? id)
        {
            try
            {
                byte[] time = BitConverter.GetBytes(DateTime.Now.AddMonths(1).ToBinary());
                byte[] key = System.Guid.NewGuid().ToByteArray();
                string timeToken = Convert.ToBase64String(time.Concat(key).ToArray());

                string token = timeToken.Substring(0, 32 / 4) + loginAs + timeToken.Substring(32 / 4, 32 / 2) + String.Format("{0:D7}", id) + timeToken.Substring(3 * (32 / 4));

                byte[] asciiBytes = Encoding.ASCII.GetBytes(token);
                for (int i = 0; i < asciiBytes.Length; i++)
                {
                    ++asciiBytes[i];
                    ++asciiBytes[i];
                }
                var cookie = System.Text.Encoding.Default.GetString(asciiBytes);

                return cookie;
            }
            catch
            {
                return "201";
            }
        }

        public static string ValidateToken(string token)
        {
            try
            {
                if (token != null)
                {
                    byte[] asciiBytes = Encoding.ASCII.GetBytes(token);
                    for (int i = 0; i < asciiBytes.Length; i++)
                    {
                        --asciiBytes[i];
                        --asciiBytes[i];
                    }

                    string str = System.Text.Encoding.Default.GetString(asciiBytes);

                    string timeTokenP1 = str.Substring(0, 32 / 4);
                    string tokenP2 = str.Substring(32 / 4);

                    string loginAs = "";

                    if (tokenP2.StartsWith("admin"))
                        loginAs = "admin";
                    else if (tokenP2.StartsWith("client"))
                        loginAs = "client";
                    else if (tokenP2.StartsWith("employee"))
                        loginAs = "employee";
                    else if (tokenP2.StartsWith("customer"))
                        loginAs = "customer";
                    else if (tokenP2.StartsWith("applicationuser"))
                        loginAs = "applicationuser";
                    else
                        return "INVALID:USER";


                    str = tokenP2;
                    str = str.Replace(loginAs, "");
                    string timeTokenP2 = str.Substring(0, 32 / 2);

                    string loginId = str.Replace(timeTokenP2, "").Substring(0, 7);
                    string timeTokenP3 = str.Replace(loginId, "");

                    string timeToken = timeTokenP1 + timeTokenP2 + timeTokenP3;
                    //loginId = loginId.Replace("0", "");
                    loginId = Convert.ToInt32(loginId) + "";
                    int? exsists = 0;
                    if (tokenP2.StartsWith("admin"))
                        exsists = Convert.ToInt16(Database.getValue("select count(*) from app_admin_users where id=" + loginId));
                    else if (tokenP2.StartsWith("client"))
                        exsists = Convert.ToInt16(Database.getValue("select count(*) from app_client_users where id=" + loginId));
                    else if (tokenP2.StartsWith("employee"))
                        exsists = Convert.ToInt16(Database.getValue("select count(*) from employee where id=" + loginId));
                    else if (tokenP2.StartsWith("customer"))
                        exsists = Convert.ToInt16(Database.getValue("select count(*) from client_customers where id=" + loginId));
                    else if (tokenP2.StartsWith("applicationuser"))
                        exsists = Convert.ToInt16(Database.getValue("select count(*) from application_users where id=" + loginId));

                    if (exsists <= 0)
                        return "INVALID:USER";


                    byte[] data = Convert.FromBase64String(timeToken);
                    DateTime time = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                    if (time < DateTime.Now)
                        return "INVALID:TIMEOUT";
                    else
                        return loginAs + ":" + loginId;

                }
                else
                    return "INVALID:NOTOKEN";
            }
            catch
            {
                return "INVALID:ERROR";
            }
        }

        #region number into word conversion

        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range    
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }

        public static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "rupees and";// just to separate whole numbers from points/cents    
                        endStr = "Paisa " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }

        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
        #endregion

        public static string OTP(int[] abc)
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                res.Append(valid[abc[i]]);
            }
            return res.ToString();
        }

        public string sendEmail(string email, string subject, string msg, string fileName)
        {
            try
            {
                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("support@cuttingedgeinfotech.com", "Cutting Edge Infotech");
                Msg.To.Add(new MailAddress(email));
                Msg.Subject = subject;

                if (fileName != null && fileName != "")
                {
                    //Attachment att = new Attachment(@"C:\Users\Rahul\Desktop\9 To 11 Batch Asp\DemoRaj\test.txt");
                    //Msg.Attachments.Add(att);
                }

                Msg.Body = msg;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "cuttingedgeinfotech.com";
                smtp.Port = 25; //25 or 587
                smtp.Credentials = new System.Net.NetworkCredential("support@cuttingedgeinfotech.com", "cei@1234#");
                smtp.EnableSsl = false;
                smtp.Timeout = 50000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(Msg);
                Msg = null;
            }
            catch (Exception e1)
            {
                return "201" + e1.Message;
            }
            return "200";
        }

        public static DataSet ReadExcelData(string filename, string sheetName)
        {
            OleDbConnection oledbConn = new OleDbConnection();
            try
            {
                string path = System.IO.Path.GetFullPath(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/" + filename));
                if (Path.GetExtension(path) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=2\"");
                }
                else if (Path.GetExtension(path) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=1;';");
                }
                else if (Path.GetExtension(path) == ".xlsm")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0 Macro;HDR=YES\";");
                }
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand(); ;
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();

                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + sheetName + "$]";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds);
                ds.Dispose();
                oledbConn.Close();
                return ds;
            }
            catch
            {
                if (oledbConn == null)
                    return null;
                else
                    oledbConn.Close();
                return null;
            }
        }

        public static string CreatePassword(int[] abc)
        {
            const string valid = "abcdefghjkmnopqrstuvwxyzABCDEFGHJKMNOPQRSTUVWXYZ234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < 8; i++)
            {
                res.Append(valid[abc[i]]);
            }
            return res.ToString();
        }

    }
}