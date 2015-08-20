﻿using System;
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

        // lay danh sach lich su giao dich cua 1 khach hang
        public ActionResult GetListGD(int? id)
        {
            var ListGD = from m in db.DSLichSuGD
                         where m.MaKH == id
                         select m;
            return Json(new { ListGD = ListGD }, JsonRequestBehavior.AllowGet);
        }
        // lay 1 lich su giao dich tham chieu theo MaGD
        public ActionResult GetGD(int? id)
        {
            var lsgd = from m in db.DSLichSuGD
                       where m.MaGD == id
                       select m;
            return Json( lsgd, JsonRequestBehavior.AllowGet);
        }


        // them data vao danh sach lich su giao dich
        public ActionResult UpdateHistory(int makh, DateTime ngaygd, DateTime ngayhethan, int sotien, string nguoithu)
        {
            if (ModelState.IsValid)
            {
                LichSuGD newls = new LichSuGD();
                newls.MaKH = makh;
                newls.NgayGD = ngaygd;
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

        // lay danh sach tat ca khach hang
        public ActionResult GetListKH()
        {
            var ListKH = from m in db.DSKhachHang
                         select m;
            return Json(new { ListKH = ListKH }, JsonRequestBehavior.AllowGet);
        }

        // lay danh sach 10 khach hang tuy theo so trang
        public ActionResult GetList10KH(int id, string sorttype)
        {
            //id is page number
            var ListKH = from m in db.DSKhachHang
                         where m.Archive == false
                         select m;
            if (sorttype == null)
            {
                ListKH = ListKH.OrderBy(m => m.MaKH);
            }
            else if (sorttype == "kh1")
            {
                ListKH = ListKH.OrderBy(m => m.TenKH);
            }
            else if (sorttype == "kh2")
            {
                ListKH = ListKH.OrderByDescending(m => m.TenKH);
            }
            else if (sorttype == "loai1")
            {
                ListKH = ListKH.OrderBy(m => m.LoaiKH);
            }
            else if (sorttype == "loai2")
            {
                ListKH = ListKH.OrderByDescending(m => m.LoaiKH);
            }
            else if (sorttype == "goi1")
            {
                ListKH = ListKH.OrderBy(m => m.LoaiGoi);
            }
            else if (sorttype == "goi2")
            {
                ListKH = ListKH.OrderByDescending(m => m.LoaiGoi);
            }
            else if (sorttype == "batdau1")
            {
                ListKH = ListKH.OrderBy(m => m.NgayDangKy);
            }
            else if (sorttype == "batdau2")
            {
                ListKH = ListKH.OrderByDescending(m => m.NgayDangKy);
            }
            else if (sorttype == "ketthuc1")
            {
                ListKH = ListKH.OrderBy(m => m.NgayHetHan);
            }
            else if (sorttype == "ketthuc2")
            {
                ListKH = ListKH.OrderByDescending(m => m.NgayHetHan);
            }
            else if (sorttype == "ch1")
            {
                ListKH = ListKH.OrderBy(m => m.TenCH);
            }
            else if (sorttype == "ch2")
            {
                ListKH = ListKH.OrderByDescending(m => m.TenCH);
            }
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