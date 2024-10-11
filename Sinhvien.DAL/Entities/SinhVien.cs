namespace Sinhvien.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SinhVien")]
    public partial class SinhVien
    {
        [Key]
        [StringLength(20)]
        public string MSSV { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

        public int? MaKhoa { get; set; }

        public double? DTB { get; set; }

        [Column("Mã Chuyên ngành")]
        public int? Mã_Chuyên_ngành { get; set; }

        [StringLength(255)]
        public string Avarta { get; set; }

        public virtual Chuyên_ngành Chuyên_ngành { get; set; }

        public virtual Khoa Khoa { get; set; }
    }
}
