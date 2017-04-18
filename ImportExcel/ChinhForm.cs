using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Entity.Migrations;

namespace ImportExcel
{
    public partial class ChinhForm : Form
    {
        public ChinhForm()
        {
            InitializeComponent();
        }
        private ImportExcelEntities entities = new ImportExcelEntities();

        private void LoadData()
        {
            var list = entities.FilterTables.ToList();
            var newList = list.Where(m => DateTime.ParseExact(m.Ngay, "dd-MM-yyyy", CultureInfo.InvariantCulture).Date > DateTime.Today.AddDays(-8).Date).ToList();
            dataGridView1.DataSource = newList;
            dataGridView1.Columns["Id"].Visible = false;
        }

        private void ChinhForm_Load(object sender, EventArgs e)
        {
            datepickerNgay.Format = DateTimePickerFormat.Custom;
            datepickerNgay.CustomFormat = "dd-MM-yyyy";

            dpBeginDate.Format = DateTimePickerFormat.Custom;
            dpBeginDate.CustomFormat = "dd-MM-yyyy";

            dpEndDate.Format = DateTimePickerFormat.Custom;
            dpEndDate.CustomFormat = "dd-MM-yyyy";

            groupImport.Enabled = false;
            LoadData();

            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";
            var items = new[] {
                new { Text = "Tìm theo ngày", Value = "0" },
                new { Text = "Tìm theo khoảng ngày", Value = "1" },
            };
            comboBox1.DataSource = items;
            comboBox1.SelectedIndex = 0;

            dpEndDate.Enabled = false;

        }

        private void radioTyping_CheckedChanged(object sender, EventArgs e)
        {
            if(radioTyping.Checked)
            {
                groupTyping.Enabled = true;
                groupImport.Enabled = false;
            }
            else
            {
                groupTyping.Enabled = false;
                groupImport.Enabled = true;
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                var filterTable = new FilterTable
                {
                    Ngay = datepickerNgay.Value.ToString("dd-MM-yyyy"),
                    Gio = txtHour.Text,
                    Chinh = txtChinh.Text,
                    Doi=txtDoi.Text,
                    A1 = txtA1.Text,
                    A2 = txtA2.Text,
                    B1 = txtB1.Text,
                    B2 = txtB2.Text,
                    Ghichu = txtGhichu.Text,
                    Noidung = txtNoidung.Text,
                    Masokenh = txtMasokenh.Text,
                    Nu = txtNu.Text
                };
                entities.FilterTables.Add(filterTable);
                entities.SaveChanges();
                MessageBox.Show(this, "Thêm mới thành công", "Thành công");
                LoadData();
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            catch
            {
                MessageBox.Show(this, "Thêm mới thất bại", "Lỗi!!!");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var newngay = datepickerNgay.Value.ToString("dd-MM-yyyy");
                    var id = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
                    var tempObject = entities.FilterTables.Where(m => m.Id.ToString() == id).FirstOrDefault();
                    //var filterTable = new FilterTable
                    //{
                    tempObject.Ngay = newngay;
                    tempObject.Gio = txtHour.Text;
                    tempObject.Chinh = txtChinh.Text;
                    tempObject.Doi = txtDoi.Text;
                    tempObject.A1 = txtA1.Text;
                    tempObject.A2 = txtA2.Text;
                    tempObject.B1 = txtB1.Text;
                    tempObject.B2 = txtB2.Text;
                    tempObject.Ghichu = txtGhichu.Text;
                    tempObject.Noidung = txtNoidung.Text;
                    tempObject.Masokenh = txtMasokenh.Text;
                    tempObject.Nu = txtNu.Text;
                    entities.FilterTables.AddOrUpdate(tempObject);
                    entities.SaveChanges();
                    MessageBox.Show(this, "Cập nhật thành công", "Thành công");
                    LoadData();
                }
                else
                {
                    MessageBox.Show(this, "Cần chọn 1 dòng để sửa", "Lỗi!!!");
                }
                
                //};
            }
            catch
            {
                MessageBox.Show(this, "Cập nhật thất bại", "Thành công");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var id = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
                    var tempObject = entities.FilterTables.Find(Convert.ToInt32(id));
                    entities.FilterTables.Remove(tempObject);
                    entities.SaveChanges();
                    MessageBox.Show(this, "Đã xóa bản ghi", "Thành công");
                    LoadData();
                    btnReset.PerformClick();
                }
                else
                {
                    MessageBox.Show(this, "Chưa chọn trường để xóa", "Alerts");
                }
            }
            catch
            {
                MessageBox.Show(this, "Không thể xóa bản ghi", "Lỗi");
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        { 
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                var row = dataGridView1.Rows[e.RowIndex];
                var ngay = row.Cells["Ngay"].Value.ToString();
                DateTime dt = DateTime.ParseExact(ngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                datepickerNgay.Value = dt;
                txtHour.Text = row.Cells["Gio"].Value.ToString();
                txtChinh.Text = row.Cells["Chinh"].Value.ToString();
                txtDoi.Text = row.Cells["Doi"].Value.ToString();
                txtNoidung.Text = row.Cells["Noidung"].Value.ToString();
                txtA1.Text = row.Cells["A1"].Value.ToString();
                txtA2.Text = row.Cells["A2"].Value.ToString();
                txtB1.Text = row.Cells["B1"].Value.ToString();
                txtB2.Text = row.Cells["B2"].Value.ToString();
                txtGhichu.Text = row.Cells["Ghichu"].Value.ToString();
                txtNu.Text = row.Cells["Nu"].Value.ToString();
                txtMasokenh.Text = row.Cells["Masokenh"].Value.ToString();

                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                //datepickerNgay.Enabled = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtHour.Text = txtChinh.Text = txtDoi.Text = txtNoidung.Text = txtA1.Text = txtA2.Text = txtB1.Text =
                txtB2.Text = txtGhichu.Text = txtNu.Text = txtMasokenh.Text = "";
            datepickerNgay.Value = DateTime.Today;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            dataGridView1.ClearSelection();
        }
        

        private void txtSearch_Click(object sender, EventArgs e)
        {
            var list = entities.FilterTables.ToList();
            if (comboBox1.SelectedValue.ToString() == "0")
            {
                var tempList = list.Where(m => DateTime.ParseExact(m.Ngay,"dd-MM-yyyy",CultureInfo.InvariantCulture).Date == dpBeginDate.Value.Date);
                if(tempList!=null&&tempList.Count()>0)
                    dataGridView1.DataSource = tempList.ToList();
                else
                    dataGridView1.DataSource = null;
            }
            else
            {
                var tempList = list.Where(m => DateTime.ParseExact(m.Ngay, "dd-MM-yyyy", CultureInfo.InvariantCulture) >= dpBeginDate.Value.Date && DateTime.ParseExact(m.Ngay, "dd-MM-yyyy", CultureInfo.InvariantCulture).Date <= dpEndDate.Value.Date);
                if (tempList != null && tempList.Count() > 0)
                    dataGridView1.DataSource = tempList.ToList();
                else
                    dataGridView1.DataSource = null;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue.ToString() == "0")
                dpEndDate.Enabled = false;
            else
                dpEndDate.Enabled = true;
        }

        private void ABC()
        {
            var list = new List<int>(new int[] { 325, 2, 3,34, 1, 35,7677 });
            var min = list[0];
            int index = 0;
            foreach(var item in list)
            {
                if (item <= min)
                {
                    min = item;
                    index = list.IndexOf(item);
                }
            }
            var FDFDF = "";
        }

    }
}
