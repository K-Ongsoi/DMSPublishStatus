using System;

namespace DMSUpdateStatus
{
    class Program
    {
        static void Main(string[] args)
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
        }
    }
}
