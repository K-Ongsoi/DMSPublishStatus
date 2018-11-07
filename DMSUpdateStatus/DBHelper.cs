using System;
using System.Collections.Generic;
using System.Text;

namespace DMSUpdateStatus
{
    public class DBHelper
    {
        {
    public bool foundInHistory(String docno, String partno, String version, String revision)
        {
            PublishStatDBDataContext db = new PublishStatDBDataContext();
            try
            {
                SAPDT_DOCSURFHIST histDoc = (from h in db.SAPDT_DOCSURFHISTs where h.hid_docno == docno && h.hid_partno == partno && h.hid_version == version && h.hid_revisionno == revision select h).FirstOrDefault();
                if (histDoc == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool foundInCurrent(String docno, String partno, String version, String revision)
        {
            PublishStatDBDataContext db = new PublishStatDBDataContext();
            try
            {
                SAPDT_DOCSURFSTATUS currDoc = (from d in db.SAPDT_DOCSURFSTATUS where d.dms_docno == docno && d.dms_partno == partno && d.dms_version == version && d.dms_revisionno == revision select d).FirstOrDefault();
                if (currDoc == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void updateCurrentDoc(Doc_Info doc)
        {
            PublishStatDBDataContext db = new PublishStatDBDataContext();
            SAPDT_DOCSURFSTATUS currDoc = (from d in db.SAPDT_DOCSURFSTATUS where d.dms_docno == doc.docNumber && d.dms_partno == doc.docPart && d.dms_version == doc.docVersion && d.dms_revisionno == doc.revisionNo select d).FirstOrDefault();
            if (currDoc != null)
            {
                currDoc.dms_status = doc.docStatus;
                if (doc.docStatus != null && doc.docStatus.Equals("DB"))
                {
                    if (currDoc.dms_distributedate == null)
                    {
                        try
                        {
                            DateTime distDate = DateTime.ParseExact(doc.distributionDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            currDoc.dms_distributedate = distDate;
                        }
                        catch (Exception)
                        {
                            currDoc.dms_distributedate = null;
                        }

                    }
                }

                try
                {
                    DateTime revDate = DateTime.ParseExact(doc.revisionDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    currDoc.dms_revisiondate = revDate;
                }
                catch (Exception)
                {
                    currDoc.dms_revisiondate = null;
                }

                try
                {
                    DateTime recvDate = DateTime.ParseExact(doc.receiveDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    currDoc.dms_receivedate = recvDate;
                }
                catch (Exception)
                {
                    currDoc.dms_receivedate = null;
                }
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void moveAndInsertDoc(Doc_Info doc)
        {
            PublishStatDBDataContext db = new PublishStatDBDataContext();
            SAPDT_DOCSURFSTATUS currDoc = (from d in db.SAPDT_DOCSURFSTATUS where d.dms_docno == doc.docNumber && d.dms_partno == doc.docPart && d.dms_version == doc.docVersion select d).FirstOrDefault();
            if (currDoc != null)
            {
                if (currDoc.dms_revisionno == doc.revisionNo)
                {
                    currDoc.dms_status = doc.docStatus;
                    if (doc.docStatus != null && doc.docStatus.Equals("DB"))
                    {
                        if (currDoc.dms_distributedate == null)
                        {
                            try
                            {
                                DateTime distDate = DateTime.ParseExact(doc.distributionDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                currDoc.dms_distributedate = distDate;
                            }
                            catch (Exception)
                            {
                                currDoc.dms_distributedate = null;
                            }

                        }
                    }
                    else
                    {
                        currDoc.dms_distributedate = null;
                    }

                    try
                    {
                        DateTime revDate = DateTime.ParseExact(doc.revisionDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        currDoc.dms_revisiondate = revDate;
                    }
                    catch (Exception)
                    {
                        currDoc.dms_revisiondate = null;
                    }

                    try
                    {
                        DateTime recvDate = DateTime.ParseExact(doc.receiveDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        currDoc.dms_receivedate = recvDate;
                    }
                    catch (Exception)
                    {
                        currDoc.dms_receivedate = null;
                    }
                    try
                    {
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    if (!foundInHistory(currDoc.dms_docno, currDoc.dms_partno, currDoc.dms_version, currDoc.dms_revisionno))
                    {
                        SAPDT_DOCSURFHIST docHist = new SAPDT_DOCSURFHIST();
                        docHist.hid_docno = currDoc.dms_docno;
                        docHist.hid_partno = currDoc.dms_partno;
                        docHist.hid_version = currDoc.dms_version;
                        docHist.hid_status = currDoc.dms_status;
                        docHist.hid_revisionno = currDoc.dms_revisionno;
                        docHist.hid_revisiondate = currDoc.dms_revisiondate;
                        docHist.hid_distributedate = currDoc.dms_distributedate;
                        docHist.hid_publisheddate = currDoc.dms_publisheddate;
                        docHist.hid_receivedate = currDoc.dms_receivedate;
                        try
                        {
                            db.SAPDT_DOCSURFHISTs.InsertOnSubmit(docHist);
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }

                    currDoc.dms_status = doc.docStatus;
                    currDoc.dms_revisionno = doc.revisionNo;
                    try
                    {
                        DateTime distDate = DateTime.ParseExact(doc.distributionDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        if (doc.docStatus != null && doc.docStatus.Equals("DB"))
                            currDoc.dms_distributedate = distDate;
                        else
                            currDoc.dms_distributedate = null;
                    }
                    catch (Exception)
                    {
                        doc.distributionDate = null;
                    }

                    try
                    {
                        DateTime revDate = DateTime.ParseExact(doc.revisionDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        currDoc.dms_revisiondate = revDate;
                    }
                    catch (Exception)
                    {
                        currDoc.dms_revisiondate = null;
                    }

                    try
                    {
                        DateTime recvDate = DateTime.ParseExact(doc.receiveDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        currDoc.dms_receivedate = recvDate;

                    }
                    catch (Exception)
                    {
                        currDoc.dms_receivedate = null;
                    }

                    currDoc.dms_publisheddate = null;
                    try
                    {
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            else
            {
                currDoc = new SAPDT_DOCSURFSTATUS();
                currDoc.dms_docno = doc.docNumber;
                currDoc.dms_partno = doc.docPart;
                currDoc.dms_version = doc.docVersion;
                currDoc.dms_status = doc.docStatus;
                currDoc.dms_revisionno = doc.revisionNo;

                try
                {
                    DateTime distDate = DateTime.ParseExact(doc.distributionDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    currDoc.dms_distributedate = distDate;
                }
                catch (Exception)
                {
                    doc.distributionDate = null;
                }

                try
                {
                    DateTime revDate = DateTime.ParseExact(doc.revisionDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    currDoc.dms_revisiondate = revDate;
                }
                catch (Exception)
                {
                    currDoc.dms_revisiondate = null;
                }

                try
                {
                    DateTime recvDate = DateTime.ParseExact(doc.receiveDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    currDoc.dms_receivedate = recvDate;
                }
                catch (Exception)
                {
                    currDoc.dms_receivedate = null;
                }
                try
                {
                    db.SAPDT_DOCSURFSTATUS.InsertOnSubmit(currDoc);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }

        public List<Doc_Info> getDMSDocStatus()
        {
            PublishStatDBDataContext db = new PublishStatDBDataContext();
            var docs = (from d in db.SAPDT_DOCSURFSTATUS select d);
            List<Doc_Info> list = new List<Doc_Info>();
            foreach (SAPDT_DOCSURFSTATUS doc in docs)
            {
                Doc_Info d = new Doc_Info();
                d.docNumber = doc.dms_docno;
                d.docPart = doc.dms_partno;
                d.docType = "OEM";
                d.docVersion = doc.dms_version;
                d.docStatus = doc.dms_status;
                d.revisionNo = doc.dms_revisionno;
                if (doc.dms_distributedate != null)
                    d.distributionDate = doc.dms_distributedate.Value.ToString("dd-MMM-yyyy");
                else
                    d.distributionDate = "-";

                if (doc.dms_revisiondate != null)
                    d.revisionDate = doc.dms_revisiondate.Value.ToString("dd-MMM-yyyy");
                else
                    d.receiveDate = "-";

                if (doc.dms_receivedate != null)
                    d.receiveDate = doc.dms_receivedate.Value.ToString("dd-MMM-yyyy");
                else
                    d.receiveDate = "-";

                if (doc.dms_publisheddate != null)
                {
                    d.publishDate = doc.dms_publisheddate.Value.ToString("dd-MMM-yyyy");
                    list.Add(d);
                }
                else
                {
                    d.publishDate = "-";
                    list.Add(d);
                    var latestDoc = (from l in db.SAPDT_DOCSURFHISTs where l.hid_docno == doc.dms_docno && l.hid_partno == doc.dms_partno && l.hid_version == doc.dms_version select l).OrderByDescending(x => x.hid_publisheddate).OrderByDescending(y => y.hid_revisionno);
                    foreach (SAPDT_DOCSURFHIST hist in latestDoc)
                    {
                        if (hist.hid_publisheddate != null)
                        {
                            Doc_Info h = new Doc_Info();
                            h.docNumber = hist.hid_docno;
                            h.docPart = hist.hid_partno;
                            h.docType = "OEM";
                            h.docVersion = hist.hid_version;
                            h.docStatus = hist.hid_status;
                            h.revisionNo = hist.hid_revisionno;

                            if (hist.hid_distributedate != null)
                            {
                                h.distributionDate = hist.hid_distributedate.Value.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                h.distributionDate = "-";
                            }

                            if (hist.hid_revisiondate != null)
                            {
                                h.revisionDate = hist.hid_revisiondate.Value.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                h.revisionDate = "-";
                            }

                            if (hist.hid_receivedate != null)
                            {
                                h.receiveDate = hist.hid_receivedate.Value.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                h.receiveDate = "-";
                            }

                            if (hist.hid_publisheddate != null)
                            {
                                h.publishDate = hist.hid_publisheddate.Value.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                h.publishDate = "-";
                            }

                            list.Add(h);
                            break;
                        }
                    }
                }
            }
            return list;
        }
    }
}
