using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using vsmartsell_test1.Models;

namespace vsmartsell_test1.Controllers
{
    public class vsmartsellController : Controller
    {
        private VsmartsellDBContext db = new VsmartsellDBContext();
        public ActionResult GetListGD(int? id)
        {
            var ListGD = from m in db.DSLichSuGD
                         where m.MaKH == id
                         select m;
            return Json(new { ListGD = ListGD }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateHistory(int makh, DateTime ngayhethan, int sotien, string nguoithu)
        {
            if (ModelState.IsValid)
            {
                LichSuGD newls = new LichSuGD();
                newls.MaKH = makh;
                newls.NgayGD = DateTime.Now;
                newls.NgayHetHan = ngayhethan;
                newls.SoTien = sotien;
                newls.NguoiThu = nguoithu;
                db.DSLichSuGD.Add(newls);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        // GET: /Vsmartsell/
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
        public ActionResult GetListGiaTien()
        {
            var ListGiaTien = from m in db.DSGia
                          select m;
            return Json(new { ListGiaTien = ListGiaTien }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListKH()
        {
            var ListKH = from m in db.DSKhachHang
                         select m;
            return Json(new { ListKH = ListKH }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList10KH(int id)
        {
            //id is page number
            var ListKH = from m in db.DSKhachHang
                         where m.Archive == false
                         orderby m.MaKH
                         select m;
            var List10KH = ListKH.Skip((id-1)*10).Take(10);
            return Json(new { List10KH = List10KH }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult api()
        {
            return View();
        }
        public ActionResult index()
        {
            var count = db.DSKhachHang.Count(m => m.Archive == false);
            ViewBag.numpage = (count - 1) / 10 + 1;
            return View();
        }

        //change archive bool
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
        public ActionResult details([Bind(Include = "MaKH,TenKH,Phone,Email,LoaiKH,LoaiGoi,NgayDangKy,NgayHetHan,TenCH,DiaChi,HoTro,Archive,Note")] KhachHang khachhang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachhang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index");
            }

            return View(khachhang);
        }

        // GET: /Vsmartsell/Create
  
        public ActionResult create()
        {
            var listgoi = from m in db.DSGia
                          orderby m.LoaiGoi
                          select m.LoaiGoi;
            var ListKH = from n in db.DSKhachHang
                         select n.LoaiKH;
            ViewBag.ListKH = ListKH;
            ViewBag.listgoi = listgoi;
            return View();
        }

        // POST: /Vsmartsell/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult create([Bind(Include = "MaKH,TenKH,Phone,Email,LoaiKH,LoaiGoi,NgayDangKy,NgayHetHan,TenCH,DiaChi,HoTro,Archive,Note")] KhachHang khachhang)
        {
            if (ModelState.IsValid)
            {
                db.DSKhachHang.Add(khachhang);
                db.SaveChanges();
                return RedirectToAction("index");
            }

            return View(khachhang);
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
