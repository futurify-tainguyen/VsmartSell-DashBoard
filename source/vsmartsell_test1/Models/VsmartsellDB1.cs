using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vsmartsell_test1.Models
{
    public enum SortType { makh, kh1, kh2, ch1, ch2, loai1, loai2, goi1, goi2, batdau1, batdau2, ketthuc1, ketthuc2, hotro1, hotro2}
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; }

        [Required(ErrorMessage = " Tên khách hàng không được để trống.")]
        public string TenKH { get; set; }

        [Required(ErrorMessage = " Số điện thoại không được để trống.")]
        [StringLength(13)]
        [RegularExpression(@"^((\d{5}-)|(\d{4}-))?\d{3}-\d{3}$", ErrorMessage = "Số điện thoại không hợp lệ (vd: 12345-678-910).")]
        public string Phone { get; set; }

        [Required(ErrorMessage = " Email không được để trống.")]
        [RegularExpression(@"[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?",
            ErrorMessage = " Email không hợp lệ (vd: abc@email.com).")]
        public string Email { get; set; }

        public LoaiKhachHang LoaiKH { get; set; }
        
        public int MaGoi { get; set; }
        [ForeignKey("MaGoi")]
        public BangGia BangGia { get; set; }

        public decimal GiaGoi { get; set; }

        [Required(ErrorMessage = " Ngày đăng ký không được để trống.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayDangKy { get; set; }

        [Required(ErrorMessage = " Ngày hết hạn không được để trống.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayHetHan { get; set; }

        [Required(ErrorMessage = " Tên cửa hàng không được để trống.")]
        public string TenCH { get; set; }
        
        public string DiaChi { get; set; }
        public string HoTro { get; set; }
        public bool Archive { get; set; }
        public string Note { get; set; }
        //public ICollection<LichSuGD> DSLichSuGD { get; set; }
        public string Viewid { get; set; }
    }

    public enum LoaiKhachHang
    {
        [Display(Name="Loại 1")]
        Loai1,
        [Display(Name = "Loại 2")]
        Loai2,
        [Display(Name = "Loại 3")]
        Loai3,
        [Display(Name = "Loại 4")]
        Loai4
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

        public int ThoiHan { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
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
        public int MaGoi { get; set; }

        [Required]
        public string LoaiGoi { get; set; }

        [Required]
        public decimal GiaTien { get; set; }
    }

    public class NguoiDung
    {
        [Key]
        public string userid { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
    }

    public class NoticeMail
    {
        public int ID { get; set; }

        [Required]
        public string MailType { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = " Email không được để trống.")]
        [RegularExpression(@"[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?",
            ErrorMessage = " Email không hợp lệ (vd: abc@email.com).")]
        public string Email { get; set; }
    }

    public class MailInvoice
    {
        public int ID { get; set; }

        [Required(ErrorMessage="Hãy ghi tên người nhận mail vào.")]
        public string Name { get; set; }

        [Required(ErrorMessage = " Email không được để trống.")]
        [RegularExpression(@"[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?",
            ErrorMessage = " Email không hợp lệ (vd: abc@email.com).")]
        public string Email { get; set; }
    }

    public class MailNearEnd
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Hãy ghi tên người nhận mail vào.")]
        public string Name { get; set; }

        [Required(ErrorMessage = " Email không được để trống.")]
        [RegularExpression(@"[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?",
            ErrorMessage = " Email không hợp lệ (vd: abc@email.com).")]
        public string Email { get; set; }
    }

    public class MailAfterEnd
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Hãy ghi tên người nhận mail vào.")]
        public string Name { get; set; }

        [Required(ErrorMessage = " Email không được để trống.")]
        [RegularExpression(@"[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?",
            ErrorMessage = " Email không hợp lệ (vd: abc@email.com).")]
        public string Email { get; set; }
    }

    public class VsmartsellDBContext : DbContext
    {
        public DbSet<KhachHang> DSKhachHang { get; set; }
        public DbSet<LichSuGD> DSLichSuGD { get; set; }
        public DbSet<BangGia> DSGia { get; set; }
        public DbSet<NguoiDung> DSNguoiDung { get; set; }
        public DbSet<NoticeMail> DSNoticeMail { get; set; }
        public DbSet<MailInvoice> DSMailInvoice { get; set; }
        public DbSet<MailNearEnd> DSMailNearEnd { get; set; }
        public DbSet<MailAfterEnd> DSMailAfterEnd { get; set; }
        public VsmartsellDBContext()
            : base("DefaultConnection")
        {
        }
    }
}