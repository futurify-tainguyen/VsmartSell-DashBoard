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
    public class HistoryController : Controller
    {
        private VsmartsellDBContext db = new VsmartsellDBContext();

        // GET: /History/
       
        public ActionResult Index()
        {
            var dslichsugd = db.DSLichSuGD.Include(l => l.KhachHang);
            return View(dslichsugd.ToList());
        }

        // GET: /History/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichSuGD lichsugd = db.DSLichSuGD.Find(id);
            if (lichsugd == null)
            {
                return HttpNotFound();
            }
            return View(lichsugd);
        }

        // GET: /History/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.DSKhachHang, "MaKH", "TenKH");
            return View();
        }

        // POST: /History/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MaGD,MaKH,NgayGD,NgayHetHan,SoTien,NguoiThu")] LichSuGD lichsugd)
        {
            if (ModelState.IsValid)
            {
                db.DSLichSuGD.Add(lichsugd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.DSKhachHang, "MaKH", "TenKH", lichsugd.MaKH);
            return View(lichsugd);
        }

        // GET: /History/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichSuGD lichsugd = db.DSLichSuGD.Find(id);
            if (lichsugd == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.DSKhachHang, "MaKH", "TenKH", lichsugd.MaKH);
            return View(lichsugd);
        }

        // POST: /History/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MaGD,MaKH,NgayGD,NgayHetHan,SoTien,NguoiThu")] LichSuGD lichsugd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lichsugd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.DSKhachHang, "MaKH", "TenKH", lichsugd.MaKH);
            return View(lichsugd);
        }

        // GET: /History/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichSuGD lichsugd = db.DSLichSuGD.Find(id);
            if (lichsugd == null)
            {
                return HttpNotFound();
            }
            return View(lichsugd);
        }

        // POST: /History/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LichSuGD lichsugd = db.DSLichSuGD.Find(id);
            db.DSLichSuGD.Remove(lichsugd);
            db.SaveChanges();
            return RedirectToAction("Index");
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
