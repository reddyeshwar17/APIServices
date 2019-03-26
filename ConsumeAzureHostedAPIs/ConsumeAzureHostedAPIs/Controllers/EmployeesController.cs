using ConsumeAzureHostedAPIs.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ConsumeAzureHostedAPIs.Controllers
{
    public class EmployeesController : Controller
    {
        Employee employee = new Employee();
        // GET: Employees
        public ActionResult Index(string userName, string password)
        {
            employee.UserName = userName;
            employee.Password = password;
            return View(employee);
        }

        public static ActionResult CallAzureAPI()
        {
            // One way
            var sasToken = Accesstoken.CreateToken(ConfigurationManager.AppSettings["SbUrl"], ConfigurationManager.AppSettings["SbSASKeyName"], ConfigurationManager.AppSettings["SbSASKeyValue"]);
            var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["SbUrl"] + "/" + "testserver" + "/" + "testmethod");
            //request = System.Text.RegularExpressions.Regex.Replace(request.ToString(), "MachinePlaceHolder", environment);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, sasToken);
            //var poCreateRequests = new List<PurchaseOrder>() {
            //      new PurchaseOrder()
            //     {
            //      }

            //poCreateRequests.RemoveAll(x => (x.CompanyCode == null || x.CompanyCode == "") && (x.ProcurementType == null || x.ProcurementType == "") && x.POAmount == 0 && (x.PropertyGroupId == "" || x.PropertyGroupId == null)
            //&& (x.PropertyGroupName == null || x.PropertyGroupName == "") && (x.PropertyDimensionId == "" || x.PropertyDimensionId == null) && (x.VendorAccount == "" || x.VendorAccount == null) && (x.PurchaseOrderId == null || x.PurchaseOrderId == ""));
            string objectToSend = "";
            //string objectToSend = JsonConvert.SerializeObject(poCreateRequests, Formatting.None
            // , new JsonSerializerSettings()
            // {
            //     DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            //     DateTimeZoneHandling = DateTimeZoneHandling.Utc
            // });
            var byteArray = Encoding.UTF8.GetBytes(objectToSend);
            request.ContentLength = byteArray.Length;
            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            HttpWebResponse response = null;

            //try
            //{
            //    response = request.GetResponse() as HttpWebResponse;
            //    // Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Status code not 200 " + response.StatusCode);
            //}
            //catch (Exception ex)
            //{
            //    // MessageBox.Show(ex.Message);
            //    //return null;
            //}

            //string content;

            //using (var reader = new StreamReader(response.GetResponseStream(), new UTF8Encoding(false)))
            //{
            //    content = reader.ReadToEnd();
            //}
            // var result = JsonConvert.DeserializeObject<List<CreatePOResponse>>(content);            
            return null;
        }

        //2nd way of calling azure hosted services
        [NonAction]
        [ActionName("Test")]
        private async static void TestMethod()
        {

            //Microsoft.IdentityModel.Clients.ActiveDirectory;
            AuthenticationResult result = null;
            string domainName = "microsoft.com"; //v-esde@microsoft.com, domain is after @ symbol, @practiceazure.com

            string adInstance = "https://login.microsoftonline.com/{0}";
            string clientID = "UYE21367632136DASDJKASDJ"; //Azure AD 
            string resourceID = "https://scmax-test.azurewebsites.net";

            string authority = string.Format(CultureInfo.InvariantCulture, adInstance, domainName);

            //Loginbtn_click
            AuthenticationContext context = new AuthenticationContext(authority);
            result = await context.AcquireTokenAsync(resourceID, clientID, new Uri("http://localhost"), new PlatformParameters(PromptBehavior.Always)); //PromptBehavior.Always

            //get values_click
            HttpClient client = new HttpClient();

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",Accesstoken);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage responseMessage = await client.GetAsync(resourceID + "api/Values");
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = await responseMessage.Content.ReadAsStringAsync();
                //show content
            }

            //Old way of calling Odata api 

            /*
                        ECOProxy.AXDataModel context = new ECOProxy.AXDataModel(new Uri(svcUrl));
                        context.Credentials = CredentialCache.DefaultCredentials;
                        //context.Credentials = new NetworkCredential(serviceAccount, serviceAccountPwd);
                        SCMPortal.ECOProxy.MS.Live.SCM.Common.Contracts.SparePartSignoutDetail[] sparePartSignout =
                            {
                            new SCMPortal.ECOProxy.MS.Live.SCM.Common.Contracts.SparePartSignoutDetail()
                        {
                            ParentAssetTagNumber = parentAssetTagNumber,
                            GoodPartVendorNumber = goodVendorPartNumber,
                            GoodPartMsfNumber = goodMsfPartNumber,
                            IsAdvancedRMA=isAdvancedRMA,
                            IsOutOfStock=isOutOfStock,

                            ItemOnHand = new SCMPortal.ECOProxy.MS.Live.SCM.Common.Contracts.ItemOnHandDetails
                            {
                                SerialNo = serialNo,
                                Location = location,
                                Qty = qty
                            }
                            }
                        };
                        OperationParameter[] param =
                        {
                            new BodyOperationParameter("warrantyType",warrantyType),
                            new BodyOperationParameter("ticketNumber",ticketNumber),
                            new BodyOperationParameter("requestId",requestId),
                            new BodyOperationParameter("partDetails",sparePartSignout)
                        };
                        List<string> warrantyResponse = new List<string>();
                        try
                        {
                            var response = context.Execute<SCMPortal.ECOProxy.MS.Live.SCM.Common.Contracts.WarrantyResult>(
                                     new Uri(sparesSignOutURL), "POST", true, param).ToList();
                            var warranty = response != null && response.Count == 1 ? response.FirstOrDefault() : null;
                            */
        }
    }
}

