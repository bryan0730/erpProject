using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProjectERP
{
    public partial class Form2 : Form
    {
        string strcoon = "Server=localhost;Database=erpproject;Uid=root;Pwd=1234;Charset=utf8";
        public Form2()
        {
            InitializeComponent();
            initComboBox_enroll();
            initComboBox_update();
            initcomboBox_Search();

            initDataSet();
        }
        
        private void initDataSet()
        {
            DataSet ds = GetData();
            dataGridViewEmpList.DataSource = ds.Tables[0];
            dataGridViewSearch.DataSource = ds.Tables[0];
        }

        private DataSet GetData()
        {
            MySqlConnection conn = new MySqlConnection(strcoon);
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM employee", conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        // 사원 -> 검색 Data
        private DataSet SearchData(string sql)
        {
            MySqlConnection conn = new MySqlConnection(strcoon);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        // 등록 부서, 직급 init comboBox
        private void initComboBox_enroll()
        {
            comboBoxEmpDivision.Items.Clear();
            comboBoxEmpClass.Items.Clear();
            string[] divisionData = { };
            using (MySqlConnection conn = new MySqlConnection(strcoon))
            {

                conn.Open();
                string sql_div = "SELECT * FROM division";
                MySqlCommand cmd_div = new MySqlCommand(sql_div, conn);
                MySqlDataReader rdr_div = cmd_div.ExecuteReader();
                while (rdr_div.Read())
                {
                    comboBoxEmpDivision.Items.Add(rdr_div["divName"].ToString());
                }
                rdr_div.Close();

                string sql_class = "SELECT * FROM class";
                MySqlCommand cmd_class = new MySqlCommand(sql_class, conn);
                MySqlDataReader rdr_class = cmd_class.ExecuteReader();
                while (rdr_class.Read())
                {
                    comboBoxEmpClass.Items.Add(rdr_class["className"].ToString());
                }
                rdr_class.Close();
            }
        }

        // 수정 부서, 직급 init comboBox
        private void initComboBox_update()
        {
            comboBoxUpdateDiv.Items.Clear();
            comboBoxUpdateClass.Items.Clear();
            string[] divisionData = { };
            using (MySqlConnection conn = new MySqlConnection(strcoon))
            {

                conn.Open();
                string sql_div = "SELECT * FROM division";
                MySqlCommand cmd_div = new MySqlCommand(sql_div, conn);
                MySqlDataReader rdr_div = cmd_div.ExecuteReader();
                while (rdr_div.Read())
                {
                    comboBoxUpdateDiv.Items.Add(rdr_div["divName"].ToString());
                }
                rdr_div.Close();

                string sql_class = "SELECT * FROM class";
                MySqlCommand cmd_class = new MySqlCommand(sql_class, conn);
                MySqlDataReader rdr_class = cmd_class.ExecuteReader();
                while (rdr_class.Read())
                {
                    comboBoxUpdateClass.Items.Add(rdr_class["className"].ToString());
                }
                rdr_class.Close();
            }
        }

        // 사원 -> 검색 -> comboBox set
        private void initcomboBox_Search()
        {
            comboBoxSearch.Items.Clear();
            comboBoxSearch.Items.Add("부서별");
            comboBoxSearch.Items.Add("이름별");
            comboBoxSearch.Items.Add("나이별");
        }

        // 사원 -> 등록 -> 등록
        private void buttonEmpEnroll_Click(object sender, EventArgs e)
        {
            string empName = textBoxEmpName.Text;
            int empAge = Convert.ToInt32(textBoxEmpAge.Text);
            string empNumber = textBoxEmpNum.Text;
            string empAddress = textBoxEmpAddress.Text;
            string empClass = comboBoxEmpClass.Text;
            string empDivision = comboBoxEmpDivision.Text;

            using (MySqlConnection conn = new MySqlConnection(strcoon))
            {
                conn.Open();
                string query = "INSERT INTO employee(empName,empAge,empNumber,empAddress,classSeq,divisionSeq) " +
                    "values(" + "'" + empName + "' ," + "'" + empAge + "' ," + "'" + empNumber + "' ," + "'" + empAddress + "' ," +
                    "(select classSeq from class where className = '"+ empClass+"')," + 
                    "(select divSeq from division where divName = '"+empDivision+"'))";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            dataGridViewEmpList.Refresh();
        }

        // 사원 -> 수정 -> 사원검색
        private void buttonEmpSearch_Click(object sender, EventArgs e)
        {
            string empNumber = textBoxSearchEmp.Text;
            using (MySqlConnection conn = new MySqlConnection(strcoon))
            {
                conn.Open();
                string sql = "SELECT emp.empAddress, emp.empNumber, emp.empName, emp.empAge, c.className, d.divName " +
                    "FROM employee as emp " +
                    "join class as c on c.classSeq = emp.classSeq " +
                    "join division as d on d.divSeq = emp.divisionSeq "+
                    "where emp.empNumber ='" + empNumber + "' ";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                textBoxUpdateNumber.Text = rdr["empNumber"].ToString();
                textBoxUpdateName.Text = rdr["empName"].ToString();
                textBoxUpdateAge.Text = rdr["empAge"].ToString();
                textBoxUpdateAddress.Text = rdr["empAddress"].ToString();
                comboBoxUpdateDiv.Text = rdr["divName"].ToString();
                comboBoxUpdateClass.Text = rdr["className"].ToString();
                rdr.Close();
            }
        }

        // 사원 -> 수정 -> 수정
        private void buttonEmpUpdate_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(strcoon))
            {
                //empName, empAge, empAddress
                conn.Open();
                string sql = "update employee set empNumber = '" + textBoxUpdateNumber.Text + "', empName = '" + textBoxUpdateName.Text + "', " +
                    "empAge = '" + textBoxUpdateAge.Text + "', empAddress = '" + textBoxUpdateAddress.Text + "', classSeq =" +
                    "(select classSeq from class where className = '" + comboBoxUpdateClass.Text + "'), " + "divisionSeq = " +
                    "(select divSeq from division where divName = '" + comboBoxUpdateDiv.Text + "')"+
                    "where empNumber = '"+ textBoxSearchEmp.Text+"'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        // 사원 -> 검색 -> 검색
        private void buttonEmpOnlySearch_Click(object sender, EventArgs e)
        {
            string comboBoxText = comboBoxSearch.Text;
            string textBoxSearchText = textBoxSearch.Text;
            switch (comboBoxText)
            {
                // division
                case "부서별":
                    string sql_div = "select * from employee where " +
                        "divisionSeq = (select divSeq from division where divName = " +
                        "'"+textBoxSearchText+"')";
                    DataSet ds_div = SearchData(sql_div);
                    dataGridViewSearch.DataSource = ds_div.Tables[0];
                    break;
                case "이름별":
                    string sql_name= "select * from employee where empName = " +
                        "'" + textBoxSearchText + "'";
                    DataSet ds_name = SearchData(sql_name);
                    dataGridViewSearch.DataSource = ds_name.Tables[0];
                    break;
                case "나이별":
                    string sql_age = "select * from employee where empAge = " +
                        "'" + textBoxSearchText + "'";
                    DataSet ds_age = SearchData(sql_age);
                    dataGridViewSearch.DataSource = ds_age.Tables[0];
                    break;
                default:
                    break;
            }
            //MessageBox.Show(comboBoxSearch.Text);
            //DataSet ds = SearchData();
            //dataGridViewSearch.DataSource = ds.Tables[0];
        }
        // 사원 -> 삭제 -> 삭제
        private void buttonEmpDelete_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount =
                dataGridViewEmpList.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < selectedRowCount; i++)
                {
                    //sb.Append("Row: ");
                    //sb.Append(dataGridViewEmpList.SelectedRows[i].Cells[0].Value.ToString());
                    //sb.Append(dataGridViewEmpList.SelectedRows[i].Index.ToString());

                    dataGridViewEmpList.Rows.Remove(dataGridViewEmpList.Rows[i]);
                    dataGridViewEmpList.Refresh();

                    string number = dataGridViewEmpList.SelectedRows[i].Cells[0].Value.ToString();
                    using (MySqlConnection conn = new MySqlConnection(strcoon))
                    {
                        conn.Open();
                        string query = "delete from employee where empSeq=" + "'" + number + "'";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.ExecuteNonQuery();
                    }

                    sb.Append(Environment.NewLine);
                }
                
                sb.Append("Total: " + selectedRowCount.ToString());
                MessageBox.Show(sb.ToString(), "Selected Rows");
            }
        }

    }
}
