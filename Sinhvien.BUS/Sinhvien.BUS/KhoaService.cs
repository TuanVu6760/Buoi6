using Sinhvien.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinhvien.BUS
{
    public class KhoaService
    {
        public List<Khoa> GetAll()
        {
            SinhVienModel context = new SinhVienModel();
            return context.Khoas.ToList();
        }
    }
}
