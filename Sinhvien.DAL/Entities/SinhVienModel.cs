using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Sinhvien.DAL.Entities
{
    public partial class SinhVienModel : DbContext
    {
        public SinhVienModel()
            : base("name=Model12")
        {
        }

        public virtual DbSet<Chuyên_ngành> Chuyên_ngành { get; set; }
        public virtual DbSet<Khoa> Khoas { get; set; }
        public virtual DbSet<SinhVien> SinhViens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chuyên_ngành>()
                .HasMany(e => e.SinhViens)
                .WithOptional(e => e.Chuyên_ngành)
                .HasForeignKey(e => new { e.MaKhoa, e.Mã_Chuyên_ngành });

            modelBuilder.Entity<Khoa>()
                .HasMany(e => e.Chuyên_ngành)
                .WithRequired(e => e.Khoa)
                .HasForeignKey(e => e.Mã_Khoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MSSV)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
