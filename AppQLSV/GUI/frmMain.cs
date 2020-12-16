using AppQLSV.DAL;
using AppQLSV.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Globalization;

namespace AppQLSV
{
    public partial class FormMain : Form
    {
        String MaLop;
        public FormMain()
        {
            InitializeComponent();
            gridLopHoc.AutoGenerateColumns = false;
            gridSinhVien.AutoGenerateColumns = false;
            LoadDanhSachLopHoc();
        }

        void LoadDanhSachLopHoc()
        {
            //Coi như bài tập tại lớp
            AppQLSVDBContext db = new AppQLSVDBContext();
            var ls = db.Classrooms.OrderBy(e => e.Name).ToList();
            bdsLopHoc.DataSource = ls;
            gridLopHoc.DataSource = bdsLopHoc;
        }
        void LoadDanhSachSinhVien()
        {
            //Coi như bài tập tại lớp
            AppQLSVDBContext db = new AppQLSVDBContext();
            var ls = db.Students.Where(t => t.IDClassroom == MaLop).ToList();
            bdsSinhVien.DataSource = ls;
            gridSinhVien.DataSource = bdsSinhVien;
        }
        private void btnThemLop_Click(object sender, EventArgs e)
        {
            var f = new frmLopChiTiet();
            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachLopHoc();
            }
        }

        private void btnXoaLop_Click(object sender, EventArgs e)
        {
            var lopDangChon = bdsLopHoc.Current as Classroom;
            if (lopDangChon != null)
            {
                var rs = MessageBox.Show(
                    "Bạn có thực sự muốn xóa không?",
                    "Chú ý",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);
                if (rs == DialogResult.OK)
                {
                    //Xóa lớp đang chọn
                    AppQLSVDBContext db = new AppQLSVDBContext();
                    var lop = db.Classrooms.Where(t => t.ID == lopDangChon.ID).FirstOrDefault();
                    if (lop != null)
                    {
                        db.Classrooms.Remove(lop);
                        db.SaveChanges();
                        LoadDanhSachLopHoc();
                    }
                }

            }
        }

        private void btnSuaLop_Click(object sender, EventArgs e)
        {
            var lopDangChon = bdsLopHoc.Current as Classroom;
            if (lopDangChon != null)
            {
                var f = new frmLopChiTiet(lopDangChon);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadDanhSachLopHoc();
                }
            }
        }

        private void bdsLopHoc_CurrentChanged(object sender, EventArgs e)
        {
            var lopDangChon = bdsLopHoc.Current as Classroom;
            MaLop = lopDangChon.ID;
            if (lopDangChon != null)
            {
                var db = new AppQLSVDBContext();
                var dsSV = db.Students.Where(t => t.IDClassroom == lopDangChon.ID).ToList();
                bdsSinhVien.DataSource = dsSV;
                gridSinhVien.DataSource = bdsSinhVien;
            }
        }

        private void gridLopHoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void btnXoaSV_Click(object sender, EventArgs e)
        {
            var SVDangChon = bdsSinhVien.Current as Student;
            if (SVDangChon != null)
            {
                var rs = MessageBox.Show(
                    "Bạn có thật sự muốn xóa không?",
                    "Chú ý",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning

                    );
                if (rs == DialogResult.OK)
                {
                    AppQLSVDBContext db = new AppQLSVDBContext();
                    var SV = db.Students.Where(t => t.ID == SVDangChon.ID).FirstOrDefault();
                    if (SV != null)
                    {
                        db.Students.Remove(SV);
                        db.SaveChanges();
                        LoadDanhSachSinhVien();
                    }
                }
            }

        }

        private void btnThemSV_Click(object sender, EventArgs e)
        {
            var f = new frmChiTietHS(MaLop);
            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachSinhVien();
            }
        }

        private void btnSuaSV_Click(object sender, EventArgs e)
        {
            var SVDangChon = bdsSinhVien.Current as Student;
            if (SVDangChon != null)
            {
                var f = new frmChiTietHS(SVDangChon);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadDanhSachSinhVien();
                }
            }
        }


        private void btnImport_Click_1(object sender, EventArgs e)
        {
            //string namefile;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Excel Files (.xls*)|*.xls*|All Files (*.*)|*.*";
            dlg.Multiselect = false;
            String kn, filePath;
            DialogResult dlResult = dlg.ShowDialog();
            if (dlResult == DialogResult.OK)
            {
                filePath = dlg.FileName;
                if (filePath.Equals(""))
                {
                    var rs = MessageBox.Show(
                            "Bạn chưa chọn file",
                            "Chú ý",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                            );
                    return;
                }
                if (!File.Exists(filePath))
                {
                    var rs = MessageBox.Show(
                            "Không thể mở File",
                            "Chú ý",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                            );
                    return;
                }
                string excelcon;
                if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                {
                    excelcon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=1'";
                }
                if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                {
                    kn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=1'";
                }
                else
                {
                    kn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1'";
                }
                using (OleDbConnection conn = new OleDbConnection(kn))
                {
                    conn.Open();
                    OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter
                    ("select * from[Sheet2$]", conn);
                    DataSet excelDataSet = new DataSet();
                    objDA.Fill(excelDataSet, "Classrooms");
                    AppQLSVDBContext db = new AppQLSVDBContext();
                    DataTable dt = excelDataSet.Tables["Classrooms"];
                    // In kết quả ra Console
                    var i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (i != 0)
                        {
                            var id = row[0].ToString();
                            var kt_id = db.Classrooms.Where(t => t.ID == id).FirstOrDefault();
                            if (kt_id == null)
                            {
                                var room = new Classroom
                                {
                                    ID = id = row[0].ToString(),
                                    Name = row[1].ToString(),
                                    Room = row[2].ToString()
                                };
                                db.Classrooms.Add(room);
                            }
                            else
                            {
                                var rs = MessageBox.Show(
                                        "Dữ liệu có sự trùng lặp khóa chính!",
                                        "Chú ý",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning
                                        );
                                return;
                            }

                        }
                        i++;
                    }
                    db.SaveChanges();
                    LoadDanhSachLopHoc();
                }

            }
        }

        private void btnImportSV_Click(object sender, EventArgs e)
        {
            //string namefile;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Excel Files (.xls*)|*.xls*|All Files (*.*)|*.*";
            dlg.Multiselect = false;
            String kn, filePath;
            DialogResult dlResult = dlg.ShowDialog();
            if (dlResult == DialogResult.OK)
            {
                filePath = dlg.FileName;
                if (filePath.Equals(""))
                {
                    var rs = MessageBox.Show(
                            "Bạn chưa chọn file",
                            "Chú ý",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                            );
                    return;
                }
                if (!File.Exists(filePath))
                {
                    var rs = MessageBox.Show(
                            "Không thể mở File",
                            "Chú ý",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                            );
                    return;
                }
                string excelcon;
                if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                {
                    excelcon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=1'";
                }
                if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                {
                    kn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=1'";
                }
                else
                {
                    kn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1'";
                }
                using (OleDbConnection conn = new OleDbConnection(kn))
                {
                    conn.Open();
                    OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter
                    ("select * from[Sheet1$]", conn);
                    DataSet excelDataSet = new DataSet();
                    objDA.Fill(excelDataSet, "Classrooms");
                    AppQLSVDBContext db = new AppQLSVDBContext();
                    gridSinhVien.DataSource = excelDataSet;
                    // In kết quả ra Console
                   /* var i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (i != 0)
                        {
                            var id = row[0].ToString();
                            var kt_id = db.Students.Where(t => t.ID == id).FirstOrDefault();
                            if (kt_id == null)
                            {
                                String day = row[3].ToString();
                                DateTime DoB;
                                if (DateTime.TryParse(day, out DoB)) {
                                    var SV = new Student
                                    {
                                        ID = id = row[0].ToString(),
                                        FirstName = row[1].ToString(),
                                        LastName = row[2].ToString(),
                                        DateOfBirth = DoB,
                                        PlaceOfBirth = row[4].ToString(),
                                        Gender = Convert.ToInt32(row[5].ToString()),
                                        IDClassroom = row[6].ToString()

                                    };

                                    db.Students.Add(SV);

                                }
                                else
                                {
                                    var rs = MessageBox.Show(
                                            "Tồn tại phần tử có ngày sinh không hợp lệ!",
                                            "Chú ý",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning
                                            );
                                    return;
                                }
                            }
                            else
                            {
                                var rs = MessageBox.Show(
                                "Dữ liệu có sự trùng lặp khóa chính!",
                                "Chú ý",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
                                return;
                            }

                        }
                        i++;*/
                 /*   }
                    db.SaveChanges();*/
                    LoadDanhSachLopHoc();
                }

            }
        }
    }
}
    