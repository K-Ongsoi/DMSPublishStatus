using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

/// <summary>
/// Summary description for DMSStatusWS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class DMSStatusWS : System.Web.Services.WebService
{
    
    [WebMethod]
    public String listAMM()
    {
        SAPHelper helper = new SAPHelper();
        List<Doc_Info> list = helper.getAMMDocuments();
        return JsonConvert.SerializeObject(list);
    }
}
