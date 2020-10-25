using MySql.Data.MySqlClient;
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
    public partial class Form1 : Form
    {
        string strcoon = "Server=localhost;Database=erpproject;Uid=root;Pwd=1234;Charset=utf8";
        string strConn = "Server=localhost;Database=erpproject;Uid=root;Pwd=1234;Charset=utf8";
        public static string userId;
        public static string userPw;
        
        public Form1()
        {
            InitializeComponent();

        }
        private string loginCheck(string name, string number)
        {
            string rank = "";
            using (MySqlConnection conn = new MySqlConnection(strcoon))
            {
                conn.Open();
                string sql = "SELECT manage FROM employee WHERE empName='" + name + "' AND empNumber='" + number + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read() == true)
                {
                    rank = rdr["manage"].ToString();
                }
                else
                {
                    rank = "3";
                }
                rdr.Close();
            }
            return rank;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Form2 form2 = new Form2();
            form2.Owner = this;
            form2.Show();

            //string empName = idTextBox.Text;
            //string empNumber = pwTextBox.Text;

            //if (loginCheck(empName, empNumber) == "0")
            //{
            //    this.Visible = false;
            //    Form2 form2 = new Form2();
            //    form2.Owner = this;
            //    form2.Show();
            //}
            //else if (loginCheck(empName, empNumber) == "1")
            //{
            //    this.Visible = false;
            //    Form3 form3 = new Form3();
            //    form3.Owner = this;
            //    form3.Show();
            //}
            //else
            //{
            //    MessageBox.Show("아이디 또는 비밀번호 잘못되었습니다.");
            //}
        }
        //로그인 쿼리
        private string loginCheck(string name, string number)
        {
            string rank = "";
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "SELECT manage FROM employee WHERE empName='" + name + "' AND empNumber='" + number + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read()==true)
                {
                    userId = name;
                    userPw = number;
                    rank = rdr["manage"].ToString();
                }
                else
                {
                    rank = "3";
                }
                rdr.Close();
            }
            return rank;
        }
        //버튼 클릭 이벤트
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string empName = idTextBox.Text;
            string empNumber = pwTextBox.Text;
            //switch case 사용가능
            if(loginCheck(empName, empNumber) == "0")
            {
                Form2.userId = userId;
                Form2.userPw = userPw;
                this.Visible = false;
                Form2 form2 = new Form2();
                form2.Owner = this;
                form2.Show();
            }else if(loginCheck(empName, empNumber) == "1")
            {
                this.Visible = false;
                Form3 form3 = new Form3();
                form3.Owner = this;
                form3.Show();
            }
            else
            {
                MessageBox.Show("아이디 또는 비밀번호 잘못되었습니다.");
            }
        }
        //자동완성 체크박스
        private void checkBoxAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            idTextBox.Text = userId;
            pwTextBox.Text = userPw;
        }
    }
}
