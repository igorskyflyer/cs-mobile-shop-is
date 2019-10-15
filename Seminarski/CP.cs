using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Media;
using Microsoft.VisualBasic;

namespace Seminarski
{
    public partial class ControlPanel : Form
    {
        const String DB_CLIENTS = "clients";
        const String DB_PARTS = "parts";

        public bool searchActive = false;
        public int maxSer = Int32.MaxValue;
        public int maxPrc = Int32.MaxValue;

        public void FetchIDs(ComboBox box, String table)
        {
           OleDbConnection conn = new OleDbConnection(Program.db);
           OleDbCommand cmd = new OleDbCommand();
           OleDbDataReader er = null;
           try
           {
               conn.Open();
               cmd.Connection = conn;
               cmd.CommandType = CommandType.Text;
               cmd.CommandText = string.Format("SELECT id FROM "+table);
               er = cmd.ExecuteReader();
               box.Items.Clear();
               while (er.Read())
                   box.Items.Add(er[0].ToString());
               if(box.Items.Count > 0)
                   box.SelectedIndex = 0;
           }
           catch (Exception exp)
           {
               MessageBox.Show("Došlo je do greške.\n"+exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
           finally
           {
               if (conn != null)
                   conn.Close();
               er.Close();
               er.Dispose();
           }
        }

        public void FetchPartsData(TextBox a, TextBox b, TextBox c, ComboBox d)
        {
            if (d.SelectedIndex > -1)
            {
                OleDbConnection conn = new OleDbConnection(Program.db);
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataReader er = null;
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format("SELECT * FROM parts WHERE id={0}", d.Text);
                    er = cmd.ExecuteReader();
                    er.Read();
                    a.Text = er["serial"].ToString();
                    b.Text = er["price"].ToString();
                    c.Text = er["name"].ToString();
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                    er.Close();
                    er.Dispose();
                }
            }
        }

        public void FetchClients(TextBox a, TextBox b, TextBox c, ComboBox d, TextBox e)
        {
            if (d.SelectedIndex > -1)
            {
                OleDbConnection conn = new OleDbConnection(Program.db);
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataReader er = null;
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format("SELECT * FROM clients WHERE id={0}", d.Text);
                    er = cmd.ExecuteReader();
                    er.Read();
                    a.Text = er["name"].ToString();
                    b.Text = er["surname"].ToString();
                    c.Text = er["cardID"].ToString();
                    e.Text= er["mobileNum"].ToString();
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                    er.Close();
                    er.Dispose();
                }
            }
        }

        public void FetchNames(ComboBox c)
        {
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader er = null;
            try
            {
              conn.Open();
              cmd.Connection = conn;
              cmd.CommandType = CommandType.Text;
              cmd.CommandText = string.Format("SELECT name, surname FROM clients");
              er = cmd.ExecuteReader();
              c.Items.Clear();
              while (er.Read())
               c.Items.Add(er["name"] + " " + er["surname"]);
              if(c.Items.Count>0)
               c.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
              MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                 conn.Close();
                er.Close();
                er.Dispose();
            }
        }

        public void FetchParts(ComboBox c)
        {
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader er = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("SELECT name, price FROM parts");
                er = cmd.ExecuteReader();
                c.Items.Clear();
                while (er.Read())
                    c.Items.Add(er["name"] + " (" + er["price"]+" din.)");
                if (c.Items.Count > 0)
                    c.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                er.Close();
                er.Dispose();
            }
        }

        public void ViewData(DataGridView dv, String sql, String table)
        {
            if (searchActive)
            {
                searchActive = false;
                return;
            }
            if (sql == "")
                sql = "SELECT * FROM "+table;
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            DataSet dataSet = new DataSet();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = sql;
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(dataSet);
                dv.DataSource = dataSet.Tables[0];
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
        }

        public void DeleteRow(ComboBox box, String table)
        {
                OleDbConnection conn = new OleDbConnection(Program.db);
                OleDbCommand cmd = new OleDbCommand();
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM "+table+" WHERE id = "+ box.Text;
                    cmd.ExecuteNonQuery();
                    box.Items.RemoveAt(box.SelectedIndex);
                    box.SelectedIndex = 0;
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Podaci su uspešno izbrisani.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }

        public void GetMax()
        {
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            maxSer = Int32.MaxValue;
            maxPrc = Int32.MaxValue;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT MAX(serial) FROM parts";
                maxSer = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = "SELECT MAX(price) FROM parts";
                maxPrc = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception exp)
            {
               MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public void GenerateBill(string name, string device)
        {
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                int cid, price, fprice, pid, wid;
                string fname, surname, partName, serial, worker, contact;
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                fname = name.Substring(0, name.IndexOf(" "));
                surname = name.Substring(name.IndexOf(" ") + 1);
                cmd.CommandText = "SELECT id FROM clients WHERE name = '" + fname + "' AND surname = '" + surname + "'";
                cid = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = "SELECT mobileNum FROM clients WHERE id = "+cid;
                contact = (string) cmd.ExecuteScalar();
                cmd.CommandText = "SELECT partId FROM bill WHERE device = '" + device + "'";
                pid = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = "SELECT price FROM parts WHERE id = " + pid;
                price = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = "SELECT fixedPrice FROM bill WHERE clientId = "+cid+" AND partId = "+pid;
                fprice = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText="SELECT name FROM parts WHERE id = "+pid;
                partName=Convert.ToString(cmd.ExecuteScalar());
                cmd.CommandText="SELECT serial FROM parts WHERE id = "+pid;
                serial=Convert.ToString(cmd.ExecuteScalar());
                cmd.CommandText = "SELECT workerId FROM bill WHERE clientId = "+cid+" AND partId = "+pid;
                wid = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = "SELECT user FROM accounts WHERE id = "+wid;
                worker = Convert.ToString(cmd.ExecuteScalar());                
                listBoxBill.Items.Clear();
                listBoxBill.Items.Add("Klijent: "+name+" ("+contact+")");
                listBoxBill.Items.Add("Radnik: " + worker);
                listBoxBill.Items.Add("Uredjaj: " + device);
                listBoxBill.Items.Add("Deo: " + partName+" ("+serial+")");
                listBoxBill.Items.Add("Cena dela: "+price+" din.");
                listBoxBill.Items.Add("Cena rada: "+fprice+" din.");
                listBoxBill.Items.Add("");
                listBoxBill.Items.Add("Ukupno za naplatu: "+(fprice+price)+" din.");
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public void GetDevices(string name, ComboBox c)
        {
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader er = null;
            try
            {
                int cid;
                string fname, surname;
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                fname = name.Substring(0, name.IndexOf(" "));
                surname = name.Substring(name.IndexOf(" ") + 1);
                cmd.CommandText = "SELECT id FROM clients WHERE name = '" + fname + "' AND surname = '" + surname + "'";
                cid = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = "SELECT device FROM bill WHERE clientId = " + cid;
                er = cmd.ExecuteReader();
                c.Items.Clear();
                while (er.Read())
                    c.Items.Add(er["device"]);
                if (c.Items.Count > 0)
                    c.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public void ValidatePartAdd() {
            buttonAddPart.Enabled = (textBoxSerial.Text.Length == 8 && textBoxPrice.Text.Length > 0 && textBoxPartAddName.Text.Length > 0);
        }

        public void ValidatePartEdit()
        {
            buttonPartEdit.Enabled = (textBoxPartEditName.Text.Length > 0 && textBoxPartEditSerial.Text.Length == 8 && textBoxPartEditPrice.Text.Length > 0);
        }

        public void ValidateClientAdd()
        {
            buttonAddClient.Enabled = (textBoxNameAdd.Text.Length > 0) && (textBoxSurnameAdd.Text.Length > 0) && (textBoxCardAdd.Text.Length == 8) && (textBoxContactNumAdd.Text.Length>8);
        }

        public void ValidateClientEdit()
        {
            buttonClientEdit.Enabled = (textBoxClientNameEdit.Text.Length > 0) && (textBoxClientSurnameEdit.Text.Length > 0) && (textBoxClientCardEdit.Text.Length == 8) && (textBoxContactNumEdit.Text.Length > 8);
        }

        public void JumpToButton(int isEnter, Button b)
        {
            if (isEnter == 13 && b.CanFocus)
                b.PerformClick();
        }

        public void ClearAll(TabPage t)
        {
            TextBox toFocus = null;
            for (int i = 0; i < t.Controls.Count - 1; i++)
                if (t.Controls[i] is TextBox)
                {
                    if(toFocus==null)
                        toFocus = ((TextBox) t.Controls[i]);
                    ((TextBox) t.Controls[i]).Clear();
                }
            if (toFocus != null)
                toFocus.Focus();
        }

        public void SetFocus(TabControl t)
        {
            Control.ControlCollection c=t.SelectedTab.Controls;
            for(int i=0; i<c.Count-1; i++)
                if (c[i].TabIndex == 0)
                {
                    c[i].Focus();
                    break;
                }
        }

        public ControlPanel()
        {
            InitializeComponent();
        }

        private void ControlPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemSounds.Beep.Play();
          if (MessageBox.Show("Da li želite da ponovo otvorite formu za prijavu?", "Sistem", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
           {
             Program.loggedIn = false;
             Program.mainForm.WindowState = FormWindowState.Normal;
             Program.mainForm.Show();
           }
          else
           Program.mainForm.Close();
        }

        private void ControlPanel_Shown(object sender, EventArgs e)
        {
          Program.mainForm.WindowState = FormWindowState.Minimized;
          textBoxPartAddName.Focus();
        }

        private void tabParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFocus(tabPartsTab);
            if (tabPartsTab.SelectedTab == tabPartsEdit)
                FetchIDs(comboBoxPartEdit, DB_PARTS);
            else
                if (tabPartsTab.SelectedTab == tabPartsDelete)
                    FetchIDs(comboBoxPartDelete, DB_PARTS);
                else
                    if (tabPartsTab.SelectedTab == tabPartsView)
                        ViewData(dataViewParts, "", DB_PARTS);
                    else
                        if (tabPartsTab.SelectedTab == tabPartsFind)
                        {
                            GetMax();
                            if(comboBoxPartFind.SelectedIndex==-1 || comboBoxPartFind.SelectedIndex==2)
                             comboBoxPartFind.SelectedIndex = 0;
                        }
        }

        private void textBoxSerial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
                textBoxSerial.BackColor = Color.Red;
            }
            else
             textBoxSerial.BackColor = Color.White;
        }

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
                textBoxPrice.BackColor = Color.Red;
            }
            else
             textBoxPrice.BackColor = Color.White;
        }

        private void textBoxSerial_TextChanged(object sender, EventArgs e)
        {
            ValidatePartAdd();
        }

        private void buttonAddPart_Click(object sender, EventArgs e)
        {
            buttonAddPart.Enabled = false;
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("INSERT INTO parts (serial, price, name) VALUES ('{0}','{1}', '{2}')", textBoxSerial.Text, textBoxPrice.Text, textBoxPartAddName.Text);
                cmd.ExecuteNonQuery();
                ClearAll(tabPartsAdd);
                MessageBox.Show("Deo je uspešno dodat.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        private void textBoxPrice_TextChanged(object sender, EventArgs e)
        {
            ValidatePartAdd();
        }

        private void comboBoxIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxPartEdit.Enabled = comboBoxPartEdit.Items.Count > 0;
            buttonPartEdit.Enabled = comboBoxPartEdit.Items.Count > 0;
            FetchPartsData(textBoxPartEditSerial, textBoxPartEditPrice, textBoxPartEditName,comboBoxPartEdit);
        }

        private void textBoxEditSerial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
                textBoxPartEditSerial.BackColor = Color.Red;
            }
            else
             textBoxPartEditSerial.BackColor = Color.White;
        }

        private void textBoxEditPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
                textBoxPartEditPrice.BackColor = Color.Red;
            }
            else
                textBoxPartEditPrice.BackColor = Color.White;   
        }

        private void buttonEditPart_Click(object sender, EventArgs e)
        {

            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE parts SET name = '"+textBoxPartEditName.Text+"', serial = '" + textBoxPartEditSerial.Text + "', price = '" + textBoxPartEditPrice.Text + "' WHERE id = " + comboBoxPartEdit.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Uspešno ste izmenili podatke o delu.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                 conn.Close();
            }
        }

        private void textBoxEditSerial_TextChanged(object sender, EventArgs e)
        {
            ValidatePartEdit();
        }

        private void textBoxEditPrice_TextChanged(object sender, EventArgs e)
        {
            ValidatePartEdit();
        }

        private void buttonPartFind_Click(object sender, EventArgs e)
        {
          String criteria = "";
          if (comboBoxPartFind.SelectedIndex == 0)
           criteria="serial";
          else
           criteria="price";
          ViewData(dataViewParts, "SELECT id, serial, price FROM parts WHERE "+criteria+" BETWEEN " + numPartMin.Value + " AND " + numPartMax.Value, "");
          searchActive = true;
          tabPartsTab.SelectedTab = tabPartsView;
        }

        private void comboBoxPartDelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxPartDelete.Enabled = comboBoxPartDelete.Items.Count > 0;
            buttonPartDel.Enabled = comboBoxPartDelete.Items.Count > 0;
            FetchPartsData(textBoxPartDelSerial, textBoxPartDelPrice, textBoxPartNameDelete,comboBoxPartDelete);
            buttonPartDel.Focus();
        }

        private void buttonPartDel_Click(object sender, EventArgs e)
        {
            SystemSounds.Beep.Play();
          if (MessageBox.Show("Da li želite da izbrišete izabrani deo?", "Sistem - potvrda je potrebna", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            DeleteRow(comboBoxPartDelete, DB_PARTS);
        }

        private void tabClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFocus(tabClientsTab);
            if (tabClientsTab.SelectedTab == tabClientEdit)
                FetchIDs(comboBoxClientEdit, DB_CLIENTS);
            else
                if (tabClientsTab.SelectedTab == tabClientDelete)
                    FetchIDs(comboBoxClientDelete, DB_CLIENTS);
                else
                    if (tabClientsTab.SelectedTab == tabClientsView)
                        ViewData(dataViewClients, "", DB_CLIENTS);
                    else
                        if (tabClientsTab.SelectedTab == tabClientFind)
                        {
                            if (comboBoxClientFind.SelectedIndex == -1)
                                comboBoxClientFind.SelectedIndex = 0;
                        }
        }

        private void comboBoxPartsFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            numPartMin.Minimum = 0;
            numPartMax.Minimum = 1;
            if (comboBoxPartFind.SelectedIndex == 0)
            {
                groupBoxPartFind.Text = "Pretraga po serijskom broju";
                numPartMin.Maximum = maxSer - 1;
                numPartMax.Maximum = maxSer;
                labelPartFindMin.Text = "Početna vrednost (0 - "+(maxSer-1).ToString()+")";
                labelPartFindMax.Text = "Krajnja vrednost (1 - " + maxSer.ToString() + ")";
            }
            else
                if (comboBoxPartFind.SelectedIndex == 1)
                {
                    groupBoxPartFind.Text = "Pretraga po ceni";
                    numPartMin.Maximum = maxPrc - 1;
                    numPartMax.Maximum = maxPrc;
                    labelPartFindMin.Text = "Početna vrednost (0 - " + (maxPrc - 1).ToString() + ")";
                    labelPartFindMax.Text = "Krajnja vrednost (1 - " + maxPrc.ToString() + ")";
                }
                else
                {
                    TopMost = false;
                    string name = Interaction.InputBox("Unesite naziv dela:", "Sistem - pretraga po nazivu dela");
                    if(name!="") {
                    ViewData(dataViewParts, "SELECT * FROM parts WHERE name LIKE '%"+name+"%'", "");
                    searchActive = true;
                    tabPartsTab.SelectedTab = tabPartsView;
                    }
                    else
                     MessageBox.Show("Niste uneli naziv dela!", "Sistem - greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TopMost = true;
                }
        }

        private void numMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (numPartMin.Value.ToString().Length==numPartMin.Maximum.ToString().Length && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
            }
        }

        private void numMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (numPartMax.Value.ToString().Length == numPartMax.Maximum.ToString().Length && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
            }
        }

        private void buttonAddClient_Click(object sender, EventArgs e)
        {
            buttonAddClient.Enabled = false;
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("INSERT INTO clients(name, surname, cardID, mobileNum) VALUES ('{0}','{1}', {2}, {3})", textBoxNameAdd.Text, textBoxSurnameAdd.Text, Convert.ToInt32(textBoxCardAdd.Text), textBoxContactNumAdd.Text);
                cmd.ExecuteNonQuery();
                ClearAll(tabClientAdd);
                MessageBox.Show("Klijent je uspešno dodat.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            ValidateClientAdd();
        }

        private void textBoxSurname_TextChanged(object sender, EventArgs e)
        {
            ValidateClientAdd();
        }

        private void comboBoxClientEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxClientEdit.Enabled = comboBoxClientEdit.Items.Count > 0;
            buttonClientEdit.Enabled = comboBoxClientEdit.Items.Count > 0;
            FetchClients(textBoxClientNameEdit, textBoxClientSurnameEdit, textBoxClientCardEdit, comboBoxClientEdit, textBoxContactNumEdit);
        }

        private void comboBoxClientDelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxClientDelete.Enabled = comboBoxClientDelete.Items.Count > 0;
            buttonClientDelete.Enabled = comboBoxClientDelete.Items.Count > 0;
            FetchClients(textBoxClientNameDelete, textBoxClientSurnameDelete, textBoxClientCardDelete, comboBoxClientDelete, textBoxClientNumDel);
            buttonClientDelete.Focus();
        }

        private void buttonClientDelete_Click(object sender, EventArgs e)
        {
            SystemSounds.Beep.Play();
            if (MessageBox.Show("Da li želite da izbrišete podatke o izabranom klijentu?", "Sistem - potvrda je potrebna", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
             DeleteRow(comboBoxClientDelete, DB_CLIENTS);
        }

        private void buttonClientFind_Click(object sender, EventArgs e)
        {
            String sql = "";
            String name, surname;
            if (comboBoxClientFind.SelectedIndex == 0)
            {
                name = textBoxClientFind.Text;
                surname = "";
                if (name.IndexOf(" ") > -1)
                {
                    name = name.Substring(0, name.IndexOf(" "));
                    surname = name.Substring(name.IndexOf(" ") + 1);
                    sql = "SELECT * FROM clients WHERE (name LIKE '%" + name + "%') OR (surname LIKE '%" + surname + "%')";
                }
                else
                    sql = "SELECT * FROM clients WHERE (name LIKE '%" + name + "%') OR (surname LIKE '%" + name + "%')";
            }
            else
                if (comboBoxClientFind.SelectedIndex == 1)
                    sql="SELECT * FROM clients WHERE cardId = "+Convert.ToInt32(textBoxClientFind.Text);
            else
             sql = "SELECT * FROM clients WHERE mobileNum LIKE '%"+textBoxClientFind.Text+"%'";
            ViewData(dataViewClients, sql, "");
            searchActive = true;
            tabClientsTab.SelectedTab = tabClientsView;
        }

        private void comboBoxClientFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxClientFind.SelectedIndex == 0)
            {
                textBoxClientFind.MaxLength = 15;
                groupBoxClientFind.Text = "Pretraga po imenu/prezimenu";
                labelClientFind.Text = "Ime/prezime";
                textBoxClientFind.Text = "";
                textBoxClientFind.Focus();
            }
            else
                if (comboBoxClientFind.SelectedIndex == 1)
                {
                    textBoxClientFind.MaxLength = 8;
                    groupBoxClientFind.Text = "Pretraga po broju lične karte";
                    labelClientFind.Text = "Broj lične karte";
                    textBoxClientFind.Text = "";
                    textBoxClientFind.Focus();
                }
                else
                {
                    textBoxClientFind.MaxLength = 10;
                    groupBoxClientFind.Text = "Pretraga po kontakt telefonu";
                    labelClientFind.Text = "Kontakt telefon";
                    textBoxClientFind.Text = "";
                    textBoxClientFind.Focus();
                }
        }

        private void textBoxClientFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBoxClientFind.Text.Length > 0 && e.KeyData == Keys.Enter) 
                
              buttonClientFind.PerformClick();}

        private void tabFixTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFocus(tabFixTab);
            if (tabFixTab.SelectedTab == tabFixAdd)
            {
                FetchNames(comboBoxClientName);
                FetchParts(comboBoxFixPart);
            }
            else
                if (tabFixTab.SelectedTab == tabFixBill)
                    FetchNames(comboBoxBillClient);
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabs.SelectedTab.Controls[0] is TabControl)
             SetFocus((TabControl) tabs.SelectedTab.Controls[0]);
            if (tabs.SelectedTab == tabFix)
            {
                FetchNames(comboBoxClientName);
                FetchParts(comboBoxFixPart);
            }
        }

        private void checkBoxFixSortClients_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxClientName.Sorted = checkBoxFixSortClients.Checked;
            comboBoxClientName.SelectedIndex = 0;
            if (!checkBoxFixSortClients.Checked)
               FetchNames(comboBoxClientName);
        }

        private void checkBoxFixSortParts_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxFixPart.Sorted = checkBoxFixSortParts.Checked;
            comboBoxFixPart.SelectedIndex = 0;
            if (!checkBoxFixSortParts.Checked)
                FetchParts(comboBoxFixPart);
        }

        private void textBoxFixedPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
                textBoxFixedPrice.BackColor = Color.Red;
            }
            else
                textBoxFixedPrice.BackColor = Color.White;
        }

        private void buttonFixAdd_Click(object sender, EventArgs e)
        {
            buttonFixAdd.Enabled = false;
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                int cid, pid, wid, fp;
                string name, surname, part;
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                name = comboBoxClientName.Text.Substring(0, comboBoxClientName.Text.IndexOf(" "));
                surname = comboBoxClientName.Text.Substring(comboBoxClientName.Text.IndexOf(" ")+1);
                cmd.CommandText = "SELECT id FROM clients WHERE name = '" + name + "' AND surname = '" + surname + "'";
                cid=Convert.ToInt32(cmd.ExecuteScalar());
                part=comboBoxFixPart.Text.Substring(0, comboBoxFixPart.Text.IndexOf(" ("));
                cmd.CommandText = "SELECT id FROM parts WHERE name = '"+part+"'";
                pid = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = "SELECT id FROM accounts WHERE user = '"+Program.user+"'";
                wid = Convert.ToInt32(cmd.ExecuteScalar());
                fp = Convert.ToInt32(textBoxFixedPrice.Text);
                cmd.CommandText = string.Format("INSERT INTO bill (clientId, partId, workerId, fixedPrice, description, device, dateReceived) VALUES ({0}, {1}, {2}, {3}, '{4}', '{5}', '{6}')", cid, pid, wid, fp, textBoxFixDesc.Text, textBoxFixDevice.Text, DateTime.Now.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Podaci o popravci su uspešno dodati.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        private void comboBoxBillClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDevices(comboBoxBillClient.Text, comboBoxBillDev);
        }

        private void comboBoxBillDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateBill(comboBoxBillClient.Text, comboBoxBillDev.Text);
        }

        private void textBoxFixDevice_TextChanged(object sender, EventArgs e)
        {
            buttonFixAdd.Enabled = (textBoxFixDevice.Text.Length>0 && textBoxFixedPrice.Text.Length>0 && textBoxFixDesc.Text.Length>0);
        }

        private void textBoxFixDesc_TextChanged(object sender, EventArgs e)
        {
            buttonFixAdd.Enabled = (textBoxFixDevice.Text.Length > 0 && textBoxFixedPrice.Text.Length > 0 && textBoxFixDesc.Text.Length > 0);
        }

        private void textBoxFixedPrice_TextChanged(object sender, EventArgs e)
        {
            buttonFixAdd.Enabled = (textBoxFixDevice.Text.Length > 0 && textBoxFixedPrice.Text.Length > 0 && textBoxFixDesc.Text.Length > 0);
        }

        private void textBoxCardAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
                textBoxCardAdd.BackColor = Color.Red;
            }
            else
                textBoxCardAdd.BackColor = Color.White;
        }

        private void textBoxClientFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && ((comboBoxClientFind.SelectedIndex == 1 && !char.IsNumber(e.KeyChar)) || (comboBoxClientFind.SelectedIndex == 0 && !char.IsLetter(e.KeyChar))) && e.KeyChar!=' ')
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
                textBoxClientFind.BackColor = Color.Red;
            }
            else
                textBoxClientFind.BackColor = Color.White;
        }

        private void textBoxClientCardEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
                textBoxClientCardEdit.BackColor = Color.Red;
            }
            else
                textBoxClientCardEdit.BackColor = Color.White;
        }

        private void textBoxPartAddName_TextChanged(object sender, EventArgs e)
        {
            ValidatePartAdd();
        }

        private void textBoxPartEditName_TextChanged(object sender, EventArgs e)
        {
            ValidatePartEdit();
        }

        private void textBoxClientFind_TextChanged(object sender, EventArgs e)
        {
            buttonClientFind.Enabled = textBoxClientFind.Text.Length > 0;
        }

        private void textBoxClientNameEdit_TextChanged(object sender, EventArgs e)
        {
            ValidateClientEdit();
        }

        private void textBoxClientSurnameEdit_TextChanged(object sender, EventArgs e)
        {
            ValidateClientEdit();
        }

        private void textBoxClientCardEdit_TextChanged(object sender, EventArgs e)
        {
            ValidateClientEdit();
        }

        private void textBoxPrice_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToButton(e.KeyValue, buttonAddPart);
        }

        private void numPartMax_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToButton(e.KeyValue, buttonPartFind);
        }

        private void textBoxPartEditPrice_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToButton(e.KeyValue, buttonPartEdit);
        }

        private void textBoxCardAdd_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToButton(e.KeyValue, buttonAddClient);
        }

        private void textBoxClientCardEdit_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToButton(e.KeyValue, buttonClientEdit);
        }

        private void textBoxCardAdd_TextChanged(object sender, EventArgs e)
        {
            ValidateClientAdd();
        }

        private void textBoxContactNumAdd_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToButton(e.KeyValue, buttonAddClient);
        }

        private void textBoxContactNumAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
                textBoxContactNumAdd.BackColor = Color.Red;
            }
            else
                textBoxContactNumAdd.BackColor = Color.White;
        }

        private void textBoxContactNumAdd_TextChanged(object sender, EventArgs e)
        {
            ValidateClientAdd();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            printDoc.ShowDialog();
        }

        private void buttonClientEdit_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(Program.db);
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE clients SET name = '" + textBoxClientNameEdit.Text + "', surname = '" + textBoxClientSurnameEdit.Text + "', cardId = "+Convert.ToInt32(textBoxClientCardEdit.Text)+", mobileNum = '"+textBoxContactNumEdit.Text+"' WHERE id = " + comboBoxClientEdit.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Uspešno ste izmenili podatke o klijentu.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Došlo je do greške.\n" + exp.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        private void textBoxContactNumEdit_TextChanged(object sender, EventArgs e)
        {
            ValidateClientEdit();
        }

    }
}
