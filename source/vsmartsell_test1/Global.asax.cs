using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using vsmartsell_test1.Models;
using System.Data.Entity;
using System.Data.SqlClient;

namespace vsmartsell_test1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string DummyPageUrl = "http://localhost:3469/home/index";
        private const string DummyCacheItemKey = "dummy";
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterCacheEntry();
        }

        // Register a cache entry which expires in 1 minute and gives us a callback.
        private void RegisterCacheEntry()
        {
            // Prevent duplicate key addition
            if (null != HttpContext.Current.Cache[DummyCacheItemKey]) return;

            HttpContext.Current.Cache.Add(DummyCacheItemKey, "Test", null, DateTime.MaxValue,
                //TimeSpan.FromMinutes(1),
                TimeSpan.FromDays(1),
                CacheItemPriority.NotRemovable,
                new CacheItemRemovedCallback(CacheItemRemovedCallback));
        }

        // Callback method which gets invoked whenever the cache entry expires.
        public void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            // Do the service works
            //DoWork();

            // We need to register another cache item which will expire again in one
            // minute. However, as this callback occurs without any HttpContext, we do not
            // have access to HttpContext and thus cannot access the Cache object. The
            // only way we can access HttpContext is when a request is being processed which
            // means a webpage is hit. So, we need to simulate a web page hit and then 
            // add the cache item.
            HitPage();
        }

        // Hits a local webpage in order to add another expiring item in cache
        private void HitPage()
        {
            WebClient client = new WebClient();
            client.DownloadData(DummyPageUrl);
        }

        // Asynchronously do the 'service' works
        private void DoWork()
        {
            DateTime today = DateTime.Now;
            VsmartsellDBContext db = new VsmartsellDBContext();
            SendMailForInvoice(today,db);
            SendMailForNearEnd(today, db);
            SendMailAfterEnd(today, db);
            db.Dispose();
        }

        private void SendMailForInvoice(DateTime today, VsmartsellDBContext db)
        {
                var gds = from p in db.DSLichSuGD
                          join m in db.DSKhachHang on p.MaKH equals m.MaKH
                          where p.Paid == false && (DbFunctions.TruncateTime(p.NgayGD) < DbFunctions.TruncateTime(today))
                          select new { dsgd = p, tenkh = m.TenKH};
                gds = gds.Where(a => (DbFunctions.DiffDays(a.dsgd.NgayGD, today) % 7) == 0);
                var nguoithus = from p in db.DSNguoiDung
                        join m in gds on p.userid equals m.dsgd.NguoiThu
                        orderby p.userid
                        select new { info = p};
                var listnguoithu = nguoithus.Distinct().ToList();
                var size = gds.Count();
                //string msg = "";
                string Mail_from = "vsmartselldashboard@gmail.com";
                string password = "futurify0404";
                string Mail_subject = "Nhắc nhở về hóa đơn chưa thanh toán (" + today.ToLongDateString() + ")";
                if (size != 0)
                {
                    foreach (var nguoithu in listnguoithu)
                    {
                        string msg = "";
                        foreach (var gd in gds)
                        {
                            if (nguoithu.info.userid == gd.dsgd.NguoiThu)
                            {
                                msg += "Mã hóa đơn: " + gd.dsgd.MaGD + "  ---  Khách hàng: " + gd.tenkh.ToUpper() + "  ---  Ngày tạo hóa đơn: " + gd.dsgd.NgayGD.ToString("dd/MM/yyyy") +
                                    "  ---  http://localhost:3469/vsmartsell/details/" + gd.dsgd.MaKH + " \n";
                            }
                        }
                        string Mail_to = nguoithu.info.email;
                        string Mail_body = msg + "\nDanh sách trên là các hóa đơn đã được " + nguoithu.info.firstname.ToUpper() + " " + nguoithu.info.lastname.ToUpper() +
                            " tạo nhưng chưa được thanh toán.\nThư này để nhắc nhở việc thu tiền các khách hàng có tên trong danh sách trên.\nThân.";
                        MailMessage MM = new MailMessage(Mail_from, Mail_to, Mail_subject, Mail_body);
                        foreach (var mail in db.DSNoticeMail)
                        {
                            if (mail.MailType == "invoice")
                            {
                                MM.To.Add(mail.Email);
                            }
                        }
                        SmtpClient SC = new SmtpClient();
                        SC.Host = "Smtp.gmail.com";
                        SC.Port = 587;
                        SC.Credentials = new System.Net.NetworkCredential(Mail_from, password);
                        SC.EnableSsl = true;
                        SC.Send(MM);
                    }
                }
        }

        private void SendMailForNearEnd(DateTime today, VsmartsellDBContext db)
        {
            var khs = from p in db.DSKhachHang
                      where p.Archive == false && (DbFunctions.TruncateTime(p.NgayHetHan) > DbFunctions.TruncateTime(today))
                      select p;
            khs = khs.Where(a => DbFunctions.DiffDays(today, a.NgayHetHan) == 7);
            var size = khs.Count();
            string msg = "";
            if (size != 0)
            {
                foreach (var kh in khs)
                {
                    msg += "Khách hàng: " + kh.TenKH.ToUpper() + "  ---  Ngày hết hạn: " + kh.NgayHetHan.ToString("dd/MM/yyyy") +
                        "  ---  http://localhost:3469/vsmartsell/details/" + kh.MaKH + " \n";
                }
                var mails = from m in db.DSNoticeMail
                            where m.MailType == "nearend"
                            select m;
                var size2 = mails.Count();
                if (size2 > 0)
                {
                    string Mail_from = "vsmartselldashboard@gmail.com";
                    string password = "futurify0404";
                    MailMessage MM = new MailMessage();
                    MM.From = new MailAddress(Mail_from);
                    foreach (var mail in mails)
                    {
                        MM.To.Add(mail.Email);
                    }
                    string Mail_subject = "Nhắc nhở về những khách hàng sắp hết hạn (" + today.ToLongDateString() + ")";
                    string Mail_body = msg + "\nDanh sách trên là những khách hàng sắp hết hạn sử dụng trong vòng 7 ngày.\nThư này để nhắc nhở việc hỏi thăm những khách hàng trên có nhu cầu muốn sử dụng thêm không.\nThân.";
                    MM.Subject = Mail_subject;
                    MM.Body = Mail_body;
                    SmtpClient SC = new SmtpClient();
                    SC.Host = "Smtp.gmail.com";
                    SC.Port = 587;
                    SC.Credentials = new System.Net.NetworkCredential(Mail_from, password);
                    SC.EnableSsl = true;
                    SC.Send(MM);
                }
            }
        }

        private void SendMailAfterEnd(DateTime today, VsmartsellDBContext db)
        {
            var khs = from p in db.DSKhachHang
                      where p.Archive == false && (DbFunctions.TruncateTime(p.NgayHetHan) < DbFunctions.TruncateTime(today))
                      select p;
            khs = khs.Where(a => (DbFunctions.DiffDays(a.NgayHetHan, DbFunctions.AddDays(today, -1)) % 7) == 0);
            var size = khs.Count();
            string msg = "";
            if (size != 0)
            {
                foreach (var kh in khs)
                {
                    msg += "Khách hàng: " + kh.TenKH.ToUpper() + "  ---  Ngày hết hạn: " + kh.NgayHetHan.ToString("dd/MM/yyyy") +
                        "  ---  http://localhost:3469/vsmartsell/details/" + kh.MaKH + " \n";
                }
                var mails = from m in db.DSNoticeMail
                            where m.MailType == "afterend"
                            select m;
                var size2 = mails.Count();
                if (size2 > 0)
                {
                    string Mail_from = "vsmartselldashboard@gmail.com";
                    string password = "futurify0404";
                    MailMessage MM = new MailMessage();
                    MM.From = new MailAddress(Mail_from);
                    foreach (var mail in mails)
                    {
                        MM.To.Add(mail.Email);
                    }
                    string Mail_subject = "Nhắc nhở về những khách hàng đã hết hạn (" + today.ToLongDateString() + ")";
                    string Mail_body = msg + "\nDanh sách trên là những khách hàng đã hết hạn sử dụng.\nThư này để nhắc nhở việc hỏi thăm những khách hàng trên có nhu cầu muốn tiếp tục sử dụng nữa không.\nThân.";
                    MM.Subject = Mail_subject;
                    MM.Body = Mail_body;
                    SmtpClient SC = new SmtpClient();
                    SC.Host = "Smtp.gmail.com";
                    SC.Port = 587;
                    SC.Credentials = new System.Net.NetworkCredential(Mail_from, password);
                    SC.EnableSsl = true;
                    SC.Send(MM);
                }
            }
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            // If the dummy page is hit, then it means we want to add another item in cache
            if (HttpContext.Current.Request.Url.ToString() == DummyPageUrl)
            {
                // Add the item in cache and when succesful, do the work.
                RegisterCacheEntry();
            }
        }
    }
}
