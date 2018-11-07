using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMSUpdateStatus
{
    public class SAPHelper
    {
        private RfcDestination rfcDestination;

        public List<Doc_Info> getAMMDocuments()
        {
            List<Doc_Info> result = new List<Doc_Info>();
            try
            {
                if (rfcDestination == null)
                {
                    rfcDestination = RfcDestinationManager.GetDestination("TEP");
                }

                RfcRepository rfcRepo = rfcDestination.Repository;
                IRfcFunction createFunc = rfcRepo.CreateFunction("ZPM_LIST_AMM");
                createFunc.Invoke(rfcDestination);
                IRfcTable retTab = createFunc.GetTable("RETTAB");
                foreach (IRfcStructure row in retTab)
                {
                    Doc_Info doc = new Doc_Info();
                    doc.docType = row.GetString("DOKAR");
                    doc.docNumber = row.GetString("DOKNR");
                    doc.docPart = row.GetString("DOKTL");
                    doc.docVersion = row.GetString("DOKVR");
                    doc.docStatus = row.GetString("DOKST");
                    String s_distDate = row.GetString("DISTDATE");
                    try
                    {
                        DateTime distDate = DateTime.ParseExact(s_distDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                        doc.distributionDate = distDate.ToString("dd-MMM-yyyy");
                    }
                    catch (Exception)
                    {
                        doc.distributionDate = s_distDate;
                    }
                    doc.revisionNo = row.GetString("REVISION");

                    String s_revDate = row.GetString("REVISEDATE");
                    try
                    {
                        DateTime revDate = DateTime.ParseExact(s_revDate, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        doc.revisionDate = revDate.ToString("dd-MMM-yyyy");
                    }
                    catch (Exception)
                    {
                        doc.revisionDate = s_revDate;
                    }

                    String s_recvDate = row.GetString("RECVDATE");
                    try
                    {
                        DateTime recvDate = DateTime.ParseExact(s_recvDate, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        doc.receiveDate = recvDate.ToString("dd-MMM-yyyy");
                    }
                    catch (Exception)
                    {
                        doc.receiveDate = s_recvDate;
                    }
                    result.Add(doc);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Get MRI characteristics values error: " + ex.Message);
            }
        }
    }
}
