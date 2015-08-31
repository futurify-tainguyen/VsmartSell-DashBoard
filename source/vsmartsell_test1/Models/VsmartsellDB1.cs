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

        [Required(ErrorMessage = "Tên khách hàng không được để trống.")]
        public string TenKH { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [StringLength(13)]
        [RegularExpression(@"^((\d{5}-)|(\d{4}-))?\d{3}-\d{3}$", ErrorMessage = "Số điện thoại không hợp lệ (vd: xxxx-xxx-xxx).")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [RegularExpression(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", ErrorMessage = "Email không hợp lệ (vd: abc@email.com).")]
        public string Email { get; set; }

        public string LoaiKH { get; set; }
        
        public string LoaiGoi { get; set; }
        [ForeignKey("LoaiGoi")]
        
        public BangGia BangGia { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayDangKy { get; set; }

        [Required(ErrorMessage = "Ngày hết hạn không được để trống.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayHetHan { get; set; }

        [Required(ErrorMessage = "Tên cửa hàng không được để trống.")]
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
        public bool Paid { get; set; }
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