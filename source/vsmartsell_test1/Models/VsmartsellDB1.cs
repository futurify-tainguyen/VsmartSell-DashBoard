using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsmartsell_test1.Models
{
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string LoaiKH { get; set; }
        public string LoaiGoi { get; set; }
        [ForeignKey("LoaiGoi")]
        public BangGia BangGia { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayDangKy { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayHetHan { get; set; }
        public string TenCH { get; set; }
        public string DiaChi { get; set; }
        public string HoTro { get; set; }
        public bool Archive { get; set; }
        public string Note { get; set; }
        //public ICollection<LichSuGD> DSLichSuGD { get; set; }
        public string Viewid { get; set; }
    }

    public class LichSuGD
    {
        [Key]
        public int MaGD { get; set; }
        public int MaKH { get; set; }
        [ForeignKey("MaKH")]
        public KhachHang KhachHang { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayGD { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public int ThoiHan { get; set; }
        public DateTime NgayHetHan { get; set; }
        public decimal CuocPhi { get; set; }
        public decimal TienGiam { get; set; }
        public decimal SoTien { get; set; }
        public string NguoiThu { get; set; }
        public string Note { get; set; }
    }

    public class BangGia
    {
        [Key]
        public string LoaiGoi { get; set; }

        public decimal GiaTien { get; set; }
    }

    public class VsmartsellDBContext : DbContext
    {
        public DbSet<KhachHang> DSKhachHang { get; set; }
        public DbSet<LichSuGD> DSLichSuGD { get; set; }
        public DbSet<BangGia> DSGia { get; set; }
        public VsmartsellDBContext()
            : base("DefaultConnection")
        {
        }
    }
}