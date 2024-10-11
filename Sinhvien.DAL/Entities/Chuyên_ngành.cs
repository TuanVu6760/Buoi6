namespace Sinhvien.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chuyên ngành")]
    public partial class Chuyên_ngành
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chuyên_ngành()
        {
            SinhViens = new HashSet<SinhVien>();
        }

        [Key]
        [Column("Mã Khoa", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Mã_Khoa { get; set; }

        [Key]
        [Column("Mã Chuyên Ngành", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Mã_Chuyên_Ngành { get; set; }

        [Column("Tên Chuyên Ngành")]
        [Required]
        [StringLength(255)]
        public string Tên_Chuyên_Ngành { get; set; }

        public virtual Khoa Khoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVien> SinhViens { get; set; }
    }
}
