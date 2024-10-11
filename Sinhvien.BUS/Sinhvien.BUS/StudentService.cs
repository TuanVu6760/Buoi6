using Sinhvien.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Sinhvien.BUS
{

        public class StudentService
    {
        public List<SinhVien>GetAll()
        {
            SinhVienModel context = new SinhVienModel();
            return context.SinhViens.ToList();
        }
        public List<SinhVien> GetAllHasNoMajor(int facultyID)
        {
            SinhVienModel context = new SinhVienModel();
            return context.SinhViens.Where(p => p.Mã_Chuyên_ngành == null && p.MaKhoa == facultyID).ToList();
        }

        public SinhVien FindById(string studentID)
        {
            SinhVienModel context = new SinhVienModel();
            return context.SinhViens.FirstOrDefault(p => p.MSSV == studentID);
        }

        public void InsertUpdate(SinhVien s)
        {
            SinhVienModel context = new SinhVienModel();

            // Kiểm tra xem sinh viên đã tồn tại hay chưa
            var existingStudent = context.SinhViens.FirstOrDefault(x => x.MSSV == s.MSSV);

            if (existingStudent != null)
            {
                // Cập nhật sinh viên đã tồn tại
                context.Entry(existingStudent).CurrentValues.SetValues(s);
            }
            else
            {
                // Thêm sinh viên mới
                context.SinhViens.Add(s);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            context.SaveChanges();
        }
        public void Delete(string studentID)
        {
            SinhVienModel context = new SinhVienModel();

            // Tìm sinh viên trong cơ sở dữ liệu dựa vào MSSV
            var student = context.SinhViens.FirstOrDefault(x => x.MSSV == studentID);

            if (student != null)
            {
                // Nếu sinh viên tồn tại, tiến hành xóa
                context.SinhViens.Remove(student);
                context.SaveChanges();
            }
            else
            {
                // Nếu sinh viên không tồn tại, có thể báo lỗi hoặc thông báo không tìm thấy
                throw new Exception("Sinh viên không tồn tại.");
            }
        }


    }
}
