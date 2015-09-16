using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using vsmartsell_test1.Models;
using Microsoft.AspNet.Identity;

namespace vsmartsell_test1.Controllers
{
    public class vsmartsellController : Controller
    {
        private VsmartsellDBContext db = new VsmartsellDBContext();

        //lay danh sach user
        public ActionResult GetListUser()
        {
            var ListUser = from m in db.DSNguoiDung
                           select m;
            return Json(new { ListUser = ListUser }, JsonRequestBehavior.AllowGet);
        }

        // lay danh sach lich su giao dich cua 1 khach hang
        public ActionResult GetListGD(int? id)
        {
            var ListGD = from m in db.DSLichSuGD
                         where m.MaKH == id
                         orderby m.MaGD descending
                         select m;
            return Json(new { ListGD = ListGD }, JsonRequestBehavior.AllowGet);
        }
        // lay 10 lich su giao dich, id: ma khach hang (0 = all), filter: loc du lieu (-1 = all, 0 = chua thanh toan, 1 = da thanh toan)
        public ActionResult GetList10GD(int id = 0, int page = 1, string sorttype = "magd2", string search = "", int filter = -1)
        {
            var ListGD = from m in db.DSLichSuGD
                         join p in db.DSNguoiDung on m.NguoiThu equals p.userid into t
                         from p in t.DefaultIfEmpty()
                         select new { gds = m, name = p == null ? ("NULL") : (p.firstname + " " + p.lastname) };
            if (id > 0)
            {
                ListGD = ListGD.Where(m => m.gds.MaKH == id);
            }
            var ListGDPaid = ListGD.Where(m => m.gds.Paid == true);
            decimal tongtien = 0;
            if (ListGDPaid.Count() > 0)
            {
                tongtien = ListGDPaid.Sum(m => m.gds.SoTien);
            }
            switch (filter)
            {
                case 0:
                    ListGD = ListGD.Where(m => m.gds.Paid == false); break;
                case 1:
                    ListGD = ListGD.Where(m => m.gds.Paid == true); break;
            }
            ListGD = ListGD.Where(m => m.name.Contains(search));
            switch (sorttype)
            {
                case "magd1":
                    ListGD = ListGD.OrderBy(m => m.gds.MaGD); break;
                case "magd2":
                    ListGD = ListGD.OrderByDescending(m => m.gds.MaGD); break;
                case "tien1":
                    ListGD = ListGD.OrderBy(m => m.gds.SoTien); break;
                case "tien2":
                    ListGD = ListGD.OrderByDescending(m => m.gds.SoTien); break;
                case "name1":
                    ListGD = ListGD.OrderBy(m => m.name); break;
                case "name2":
                    ListGD = ListGD.OrderByDescending(m => m.name); break;
                case "tao1":
                    ListGD = ListGD.OrderBy(m => m.gds.NgayGD); break;
                case "tao2":
                    ListGD = ListGD.OrderByDescending(m => m.gds.NgayGD); break;
                case "het1":
                    ListGD = ListGD.OrderBy(m => m.gds.NgayHetHan); break;
                case "het2":
                    ListGD = ListGD.OrderByDescending(m => m.gds.NgayHetHan); break;
            }
            var count = ListGD.Count();
            var numpage = (count - 1) / 10 + 1;
            var List10GD = ListGD.Skip((page - 1) * 10).Take(10);
            return Json(new { List10GD = List10GD, numpage = numpage , tongtien = tongtien}, JsonRequestBehavior.AllowGet);
        }


        // them data vao danh sach lich su giao dich
        public ActionResult UpdateHistory(int makh, DateTime ngaygd, DateTime ngayhethan, decimal sotien, string nguoithu, int thoihan, decimal cuocphi, decimal tiengiam, string note, bool paid)
        {
            if (ModelState.IsValid)
            {
                LichSuGD newls = new LichSuGD();
                newls.MaKH = makh;
                newls.NgayGD = ngaygd;
                newls.NgayHetHan = ngayhethan;
                newls.SoTien = sotien;
                newls.NguoiThu = User.Identity.GetUserId();
                newls.CuocPhi = cuocphi;
                newls.ThoiHan = thoihan;
                newls.TienGiam = tiengiam;
                newls.Note = note;
                newls.Paid = paid;
                db.DSLichSuGD.Add(newls);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }

        public ActionResult Paid(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gd = db.DSLichSuGD.Find(id);
            if (gd == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                gd.Paid = true;
                db.Entry(gd).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }

        // GET: /Vsmartsell/
        // lay thong tin 1 khach hang
        public ActionResult GetKH(int id)
        {
            var khachhang = from m in db.DSKhachHang
                         where m.MaKH == id
                         select m;
            return Json(new { khachhang = khachhang  }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGiaTien(string loaigoi)
        {
            var GiaTien = from m in db.DSGia
                          where m.LoaiGoi == loaigoi
                          select m;
            return Json(new { GiaTien = GiaTien }, JsonRequestBehavior.AllowGet);
        }

        // lay danh sach gia tien cua cac loai goi
        public ActionResult GetListGiaTien()
        {
            var ListGiaTien = from m in db.DSGia
                          select m;
            return Json(new { ListGiaTien = ListGiaTien }, JsonRequestBehavior.AllowGet);
        }

        // them 1 loai goi
        public ActionResult AddLoaiGoi(string loaigoi, decimal giatien)
        {
            if (ModelState.IsValid)
            {
                foreach(var goi in db.DSGia)
                {
                    if (goi.LoaiGoi == loaigoi)
                    {
                        return Json(new {error = "Đã có tên loại gói này, vui lòng dùng tên khác."}, JsonRequestBehavior.AllowGet );
                    }
                }
                BangGia newgoi = new BangGia();
                newgoi.LoaiGoi = loaigoi;
                newgoi.GiaTien = giatien;
                db.DSGia.Add(newgoi);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }

        //chinh sua loai goi / gia goi
        public ActionResult EditGiaGoi(string loaigoi, decimal newgiatien)
        {
            var newgoi = db.DSGia.Find(loaigoi);
            if (ModelState.IsValid)
            {
                newgoi.GiaTien = newgiatien;
                db.Entry(newgoi).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }

        //xoa 1 loai goi
        public ActionResult DelLoaiGoi(string loaigoi)
        {
            var error = "";
            var khs = from m in db.DSKhachHang
                      where m.Archive == false
                      select m;
            foreach (var kh in khs)
            {
                if (kh.LoaiGoi == loaigoi)
                {
                    error += "MaKH: " + kh.MaKH + ", TenKH: " + kh.TenKH + ".\n";
                }
            }
            if (error != "")
            {
                error += "\nDanh sách trên là những khách hàng đang sử dụng loại gói này.\nHãy chỉnh sửa loại gói của họ sang loại gói khác trước khi xóa.";
                return Json(new { error }, JsonRequestBehavior.AllowGet);
            }
            var goi = db.DSGia.Find(loaigoi);
            if (goi == null)
            {
                return HttpNotFound();
            }
            db.DSGia.Remove(goi);
            db.SaveChanges();
            return Json(true);
        }

        //lay danh sach gui mai
        public ActionResult ListNoticeMail()
        {
            var a = from m in db.DSNoticeMail
                    select m;
            return Json(new { ListMail = a }, JsonRequestBehavior.AllowGet);
        }

        //them email vao danh sach gui mail invoice
        public ActionResult AddNoticeMail(string mailtype, string name, string email)
        {
            if (ModelState.IsValid)
            {
                NoticeMail newmail = new NoticeMail();
                newmail.MailType = mailtype;
                newmail.Name = name;
                newmail.Email = email;
                db.DSNoticeMail.Add(newmail);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        //sua email trong danh sach gui mail invoice
        public ActionResult EditNoticeMail(int? id, string name, string email)
        {
            var newmail = db.DSNoticeMail.Find(id);
            if (newmail == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                newmail.Name = name;
                newmail.Email = email;
                db.Entry(newmail).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        //xoa email khoi danh sach gui mail invoice
        public ActionResult DelNoticeMail(int? id)
        {
            var mail = db.DSNoticeMail.Find(id);
            if (mail == null)
            {
                return HttpNotFound();
            }
            db.DSNoticeMail.Remove(mail);
            db.SaveChanges();
            return Json(true);
        }

        // lay danh sach tat ca khach hang
        public ActionResult GetListKH()
        {
            var ListKH = from m in db.DSKhachHang
                         select m;
            return Json(new { ListKH = ListKH }, JsonRequestBehavior.AllowGet);
        }

        // lay danh sach 10 khach hang tuy theo so trang
        public ActionResult GetList10KH(int id, SortType sorttype = SortType.makh, string search = "")
        {
            //id is page number
            var ListKH = from m in db.DSKhachHang
                         where m.Archive == false
                         select m;    
            ListKH = ListKH.Where(m => m.TenKH.Contains(search));
            switch (sorttype)
            {
                case SortType.makh:
                    ListKH = ListKH.OrderBy(m => m.MaKH); break;
                case SortType.kh1:
                    ListKH = ListKH.OrderBy(m => m.TenKH); break;
                case SortType.kh2:
                    ListKH = ListKH.OrderByDescending(m => m.TenKH); break;
                case SortType.ch1:
                    ListKH = ListKH.OrderBy(m => m.TenCH); break;
                case SortType.ch2:
                    ListKH = ListKH.OrderByDescending(m => m.TenCH); break;
                case SortType.loai1:
                    ListKH = ListKH.OrderBy(m => m.LoaiKH); break;
                case SortType.loai2:
                    ListKH = ListKH.OrderByDescending(m => m.LoaiKH); break;
                case SortType.goi1:
                    ListKH = ListKH.OrderBy(m => m.LoaiGoi); break;
                case SortType.goi2:
                    ListKH = ListKH.OrderByDescending(m => m.LoaiGoi); break;
                case SortType.batdau1:
                    ListKH = ListKH.OrderBy(m => m.NgayDangKy); break;
                case SortType.batdau2:
                    ListKH = ListKH.OrderByDescending(m => m.NgayDangKy); break;
                case SortType.ketthuc1:
                    ListKH = ListKH.OrderBy(m => m.NgayHetHan); break;
                case SortType.ketthuc2:
                    ListKH = ListKH.OrderByDescending(m => m.NgayHetHan); break;
                case SortType.hotro1:
                    ListKH = ListKH.OrderBy(m => m.HoTro); break;
                case SortType.hotro2:
                    ListKH = ListKH.OrderByDescending(m => m.HoTro); break;
            }
            var count = ListKH.Count();
            var numpage = (count - 1) / 10 + 1;
            var List10KH = ListKH.Skip((id-1)*10).Take(10);
            return Json(new { List10KH = List10KH, numpage = numpage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult api()
        {
            return View();
        }

        [Authorize(Roles = "Admin,CanEdit,User")]
        public ActionResult index()
        {
            var count = db.DSKhachHang.Count(m => m.Archive == false);
            ViewBag.ReturnUrl = Url.Action("index");
            return View();
        }

        [Authorize(Roles = "Admin,CanEdit,User")]
        public ActionResult controlpanel()
        {
            return View();
        }

        // thay doi trang thai archive cua 1 khach hang
        [HttpPost]
        public ActionResult EditArchive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var khachhang = db.DSKhachHang.Find(id);
            if (khachhang == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                khachhang.Archive = !khachhang.Archive;
                db.Entry(khachhang).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        // GET: /Vsmartsell/Details/5
        [Authorize(Roles = "Admin,CanEdit,User")]
        public ActionResult details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachhang = db.DSKhachHang.Find(id);
            if (khachhang == null)
            {
                return HttpNotFound();
            }
            var listgoi = from m in db.DSGia
                          orderby m.LoaiGoi
                          select m.LoaiGoi;
            ViewBag.listgoi = listgoi;
            ViewBag.makh = id;
            return View(khachhang);
        }

        // POST: /Vsmartsell/Details/5 , edit in details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult details([Bind(Include = "MaKH,TenKH,Phone,Email,LoaiKH,LoaiGoi,GiaGoi,NgayDangKy,NgayHetHan,TenCH,DiaChi,HoTro,Archive,Note,Viewid")] KhachHang khachhang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachhang).State = EntityState.Modified;
                db.SaveChanges();
                TempData["userMsg"] = "Chỉnh sửa thông tin khách hàng thành công.";
                return RedirectToAction("details");
            }
            var listgoi = from m in db.DSGia
                          orderby m.LoaiGoi
                          select m.LoaiGoi;
            ViewBag.listgoi = listgoi;
            ViewBag.makh = khachhang.MaKH;
            return View(khachhang);
        }

        // GET: /Vsmartsell/Create
        [Authorize(Roles = "Admin,CanEdit,User")]
        public ActionResult create()
        {
            var listgoi = from m in db.DSGia
                          orderby m.LoaiGoi
                          select m.LoaiGoi;
            var ListKH = from n in db.DSKhachHang
                         select n.LoaiKH;
            ViewBag.ListKH = ListKH;
            ViewBag.listgoi = listgoi;
            return View("details");
        }

        // POST: /Vsmartsell/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult create([Bind(Include = "MaKH,TenKH,Phone,Email,LoaiKH,LoaiGoi,NgayDangKy,NgayHetHan,TenCH,DiaChi,HoTro,Archive,Note,Viewid")] KhachHang khachhang)
        {
            if (ModelState.IsValid)
            {
                db.DSKhachHang.Add(khachhang);
                db.SaveChanges();
                TempData["userMsg"] = "Tạo mới khách hàng thành công.";
                return RedirectToAction("details", new { id = khachhang.MaKH });
            }
            var listgoi = from m in db.DSGia
                          orderby m.LoaiGoi
                          select m.LoaiGoi;
            ViewBag.listgoi = listgoi;
            return View("details", khachhang);
        }

        public ActionResult ThanhToan(LichSuGD lsgd)
        {
            if (ModelState.IsValid)
            {
                db.DSLichSuGD.Add(lsgd);
                db.SaveChanges();
                return RedirectToAction("details");
            }

            return View(lsgd);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
