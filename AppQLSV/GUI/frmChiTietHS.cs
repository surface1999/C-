using AppQLSV.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppQLSV.GUI
{
    public partial class frmChiTietHS : Form
    {
        String MaLop;
        Student SinhVien;
        public frmChiTietHS()
        {
            InitializeComponent();
            this.Text = "Thêm mới sinh viên";
        }

        public frmChiTietHS(Student SinhVien)
        {
            AppQLSVDBContext db = new AppQLSVDBContext();
            var phong = db.Classrooms.OrderBy(e => e.Name).ToList();
            InitializeComponent();
            this.Text = "Chỉnh sửa sinh viên";
            this.SinhVien = SinhVien;
            txtTen.Text = this.SinhVien.FirstName;
            txtHo.Text = this.SinhVien.LastName;
            dtpNgaySinh.Value = this.SinhVien.DateOfBirth;
            txtNoiSinh.Text = this.SinhVien.PlaceOfBirth;
            if(this.SinhVien.Gender == 1)
            {
                rdbNam.Checked = true;
            }
            else
            {
                rdbNu.Checked = true;
            }
        }
        public frmChiTietHS(String MaLop)
        {
            InitializeComponent();
            this.Text = "Thêm mới sinh viên";
            this.MaLop = MaLop;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnDongY_Click(object sender, EventArgs e)
        {
            var TenSV = txtTen.Text;
            var Ho = txtHo.Text;
            var NgaySinh = dtpNgaySinh.Value;
            var NoiSinh = txtNoiSinh.Text;
            var Gender = rdbNam.Checked == true ? 1 : 0;
            AppQLSVDBContext db = new AppQLSVDBContext();
            if(SinhVien == null){
                var SV = new Student
                {
                    ID = Guid.NewGuid().ToString(),
                    FirstName = TenSV,
                    LastName = Ho,
                    DateOfBirth = NgaySinh,
                    PlaceOfBirth = NoiSinh,
                    Gender = Gender,
                    IDClassroom = MaLop
                };
                db.Students.Add(SV);
                db.SaveChanges();
                DialogResult = DialogResult.OK;
            }
            else{ 
            var sv = db.Students.Where(t => t.ID == SinhVien.ID).FirstOrDefault();
            sv.FirstName = TenSV;
            sv.LastName = Ho;
            sv.DateOfBirth = NgaySinh;
            sv.PlaceOfBirth = NoiSinh;
            sv.Gender = Gender;
            db.SaveChanges();
            DialogResult = DialogResult.OK;
            }
        }

        private void rdbNu_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void rdbNam_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtHo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
        }
    }
}
