using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sinhvien.BUS;
using Sinhvien.DAL.Entities;
using static System.Net.Mime.MediaTypeNames;


namespace Buoi6
{
    public partial class Form1 : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly KhoaService khoaService = new KhoaService();

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Lấy danh sách sinh viên
                var listStudents = studentService.GetAll();
                BindGrid(listStudents);

                // Lấy danh sách khoa và gán vào ComboBox
                var listKhoa = khoaService.GetAll();
                comboBox1.DataSource = listKhoa;
                comboBox1.DisplayMember = "TenKhoa";  // Hiển thị tên khoa
                comboBox1.ValueMember = "MaKhoa";  // Giá trị lưu lại sẽ là mã khoa
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void BindGrid(List<SinhVien> listStudents)
        {
            // Clear old rows
            dataGridView2.Rows.Clear();

            foreach (var item in listStudents)
            {
                // Add a new row
                int index = dataGridView2.Rows.Add();

                // Map each SinhVien property to the appropriate DataGridView column
                dataGridView2.Rows[index].Cells[0].Value = item.MSSV;  // Assuming MSSV is in the first column
                dataGridView2.Rows[index].Cells[1].Value = item.HoTen; // Student's name in the second column
                dataGridView2.Rows[index].Cells[2].Value = item.Khoa != null ? item.Khoa.TenKhoa : "N/A";  // Department name
                dataGridView2.Rows[index].Cells[3].Value = item.DTB;   // GPA in the fourth column
                dataGridView2.Rows[index].Cells[4].Value = item.Chuyên_ngành != null ? item.Chuyên_ngành.Tên_Chuyên_Ngành : "";  // Major name
               
                // Check and display the Avatar file name or "Không có ảnh" if null/empty
                dataGridView2.Rows[index].Cells[5].Value = !string.IsNullOrEmpty(item.Avarta) ? item.Avarta : "Không có ảnh";
            }
        }



        private void ShowAvatar(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                pictureBox1.Image = null; // Không có ảnh để hiển thị
            }
            else
            {
                // Xây dựng đường dẫn đầy đủ đến hình ảnh
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "Images", imageName);

                if (File.Exists(imagePath)) // Kiểm tra xem file có tồn tại không
                {
                    pictureBox1.Image = System.Drawing.Image.FromFile(imagePath); // Tải và hiển thị ảnh
                }
                else
                {
                    pictureBox1.Image = null; // Không tìm thấy file, xóa PictureBox
                }
            }
        }




        private void btnThem_Click(object sender, EventArgs e)
        {
            SaveStudent();
        }
        private void SaveStudent()
        {
            try
            {
                // Tạo đối tượng SinhVien từ dữ liệu người dùng nhập
                SinhVien student = new SinhVien()
                {
                    MSSV = txtMSSV.Text,
                    HoTen = txtHoten.Text,
                    MaKhoa = (int)comboBox1.SelectedValue,
                    DTB = string.IsNullOrEmpty(txtDTB.Text) ? null : (double?)Convert.ToDouble(txtDTB.Text),
                    Avarta = pictureBox1.Image != null ? Path.GetFileName(pictureBox1.ImageLocation) : null
                };

                // Gọi phương thức InsertUpdate để thêm hoặc cập nhật sinh viên
                studentService.InsertUpdate(student);

                // Thông báo thành công
                MessageBox.Show("Lưu thành công!");

                // Làm mới danh sách sinh viên sau khi thêm mới
                RefreshStudentList();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }


        

        private void RefreshStudentList()
        {
            try
            {
                // Lấy danh sách sinh viên từ database
                var listStudents = studentService.GetAll();

                // Gọi hàm BindGrid để cập nhật DataGridView
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải danh sách sinh viên: {ex.Message}");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy MSSV của sinh viên cần xóa từ TextBox
                string mssv = txtMSSV.Text;

                // Gọi hàm Delete để xóa sinh viên
                studentService.Delete(mssv);

                // Thông báo thành công
                MessageBox.Show("Xóa thành công!");

                // Cập nhật lại danh sách sinh viên trên DataGridView
                RefreshStudentList();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<SinhVien>();
            if (this.checkBox1.Checked)
                listStudents = studentService.GetAllHasNoMajor(1);
            else
                listStudents = studentService.GetAll();
            BindGrid(listStudents);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.jfif";
            openFileDialog.Title = "Chọn một ảnh";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Lấy phần mở rộng file (vd: .jpg, .png)
                string fileExtension = Path.GetExtension(openFileDialog.FileName);

                // Lấy mã sinh viên từ TextBox
                string studentID = txtMSSV.Text;

                // Đặt tên file theo định dạng {studentID}.{fileExtension}
                string fileName = $"{studentID}{fileExtension}";

                // Đường dẫn tới thư mục Images
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagesFolderPath = Path.Combine(parentDirectory, "Images");
                string imagePath = Path.Combine(imagesFolderPath, fileName);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }

                // Sao chép ảnh vào thư mục với tên mới
                File.Copy(openFileDialog.FileName, imagePath, true);

                // Hiển thị ảnh trong PictureBox
                pictureBox1.Image = System.Drawing.Image.FromFile(imagePath);

                // Cập nhật tên file vào cơ sở dữ liệu
                SaveAvatarToDatabase(studentID, fileName);
            }
        }

        // Hàm lưu tên ảnh vào cơ sở dữ liệu
        private void SaveAvatarToDatabase(string studentID, string avatarFileName)
        {
            // Code sử dụng Entity Framework để cập nhật thông tin vào database
            using (var db = new SinhVienModel())
            {
                var student = db.SinhViens.Find(studentID);
                if (student != null)
                {
                    student.Avarta = avatarFileName;  // Cập nhật tên file avatar
                    db.SaveChanges();  // Lưu thay đổi vào CSDL
                }
            }
        }

    }
}

