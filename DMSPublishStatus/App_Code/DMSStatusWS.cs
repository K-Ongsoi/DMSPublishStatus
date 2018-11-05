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
        DBHelper db = new DBHelper();
        List<Doc_Info> docs = new List<Doc_Info>();
        List<Doc_Info> list = helper.getAMMDocuments();
        foreach (Doc_Info doc in list)
        {
            if (doc.revisionNo == null || doc.revisionNo.Trim().Length == 0)
            {
                docs.Add(doc);
            }
            else
            {
                if (!db.foundInCurrent(doc.docNumber, doc.docPart, doc.docVersion, doc.revisionNo))
                {
                    db.moveAndInsertDoc(doc);
                }
                else
                {
                    db.updateCurrentDoc(doc);
                }
            }
        }

        List<Doc_Info> dbDocs = db.getDMSDocStatus();
        foreach (Doc_Info d in dbDocs)
        {
            docs.Add(d);
        }
        return JsonConvert.SerializeObject(docs);
    }
}
