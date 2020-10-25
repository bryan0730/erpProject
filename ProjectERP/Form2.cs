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
        string strConn = "Server=localhost;Database=erpproject;Uid=root;Pwd=1234;Charset=utf8";
        public static string userId;
        public static string userPw;
        public Form2()
        {
            InitializeComponent();
            initComboBox_enroll();
            initComboBox_update();
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
        }

        // 사원 -> 수정 -> 검색
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

        // 사원 -> 검색
        private void buttonEmpOnlySearch_Click(object sender, EventArgs e)
        {

        }

        
        

        //부서 - 등록 : gridView에 값 뿌려주려고 컬럼값 가져오는 쿼리 아직 미완성
        private List<string> getColumnName()
        {
            List<string> colList = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "SHOW COLUMNS FROM division";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    colList.Add(rdr["Field"].ToString());
                }
                rdr.Close();
            }
            return colList;
        }
        //사원 - 등록 : 로그아웃 버튼인데 체크박스 자동값 불러오기 테스트로 만들어본거임
        private void button10_Click_1(object sender, EventArgs e)
        {
            Form1.userId = userId;
            Form1.userPw = userPw;
            this.Visible = false;
            Form1 form1 = new Form1();
            form1.Owner = this;
            form1.Show();
        }
    }
}
