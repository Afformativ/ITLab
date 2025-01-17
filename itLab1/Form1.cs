﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace itLab1
{
    public partial class Form1 : Form
    {
        dbManager dbm = new dbManager();
        string curFilePath = "";
        string cellOldValue = "";
        string cellNewValue = "";
        public Form1()
        {
            InitializeComponent();
            cbTypes.SelectedIndex = 0;
        }

        private void butCreate_Click(object sender, EventArgs e)
        {
            if (!curFilePath.Equals(""))
            {
                DialogResult dialogResult = MessageBox.Show("Зберегти зміни?", "Увага!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    dbm.SaveDB(curFilePath);
                }
            }

            if (dbm.CreateDB(tbCreateDBName.Text))
            {
                curFilePath = "";
                tabControl.TabPages.Clear();
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();
            }
        }

        private void butAddTable_Click(object sender, EventArgs e)
        {
            if (dbm.AddTable(tbAddTableName.Text))
            {
                tabControl.TabPages.Add(tbAddTableName.Text);
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = tabControl.SelectedIndex;
            if (ind != -1) VisualTable(dbm.GetTable(ind));
        }

        void VisualTable(Table t)
        {
            try
            {
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();

                foreach (Column c in t.tColumnsList)
                {
                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                    column.Name = c.cName;
                    column.HeaderText = c.cName;
                    dataGridView.Columns.Add(column);
                }

                foreach (Row r in t.tRowsList)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    foreach (string s in r.rValuesList)
                    {
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        cell.Value = s;
                        row.Cells.Add(cell);
                    }
                    try
                    {
                        dataGridView.Rows.Add(row);
                    }
                    catch { }
                }
            }
            catch { }
        }

        private void butAddColumn_Click(object sender, EventArgs e)
        {
            if (dbm.AddColumn(tabControl.SelectedIndex, tbAddColumnName.Text, cbTypes.Text))
            {

                int ind = tabControl.SelectedIndex;
                if (ind != -1) VisualTable(dbm.GetTable(ind));
            }
        }

        private void butAddRow_Click(object sender, EventArgs e)
        {
            if (dbm.AddRow(tabControl.SelectedIndex))
            {

                int ind = tabControl.SelectedIndex;
                if (ind != -1) VisualTable(dbm.GetTable(ind));
            }
        }

        private void butSearch_Click(object sender, EventArgs e)
        {
            string search = tbSearch.Text;
            int ind = tabControl.SelectedIndex;
            if (ind != -1) VisualTable(dbm.SearchInTable(ind, search));
            dataGridView.ReadOnly = true;
        }

        private void butCancelSearch_Click(object sender, EventArgs e)
        {
            dataGridView.ReadOnly = false;
            int ind = tabControl.SelectedIndex;
            if (ind != -1) VisualTable(dbm.GetTable(ind));
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            cellOldValue = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            cellNewValue = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (!dbm.ChangeValue(cellNewValue, tabControl.SelectedIndex, e.ColumnIndex, e.RowIndex))
            {
                dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cellOldValue;
            }

            int ind = tabControl.SelectedIndex;
            if (ind != -1) VisualTable(dbm.GetTable(ind));
        }

        private void butChooseFilePath_Click(object sender, EventArgs e)
        {
            if (ofdChooseFilePath.ShowDialog() == DialogResult.OK)
            {
                tbFilePath.Text = ofdChooseFilePath.FileName;
            }
        }

        private void butDeleteRow_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count == 0) return;
            try
            {
                dbm.DeleteRow(tabControl.SelectedIndex, dataGridView.CurrentCell.RowIndex);
            }
            catch { }

            int ind = tabControl.SelectedIndex;
            if (ind != -1) VisualTable(dbm.GetTable(ind));
        }

        private void butDeleteColumn_Click(object sender, EventArgs e)
        {
            if (dataGridView.Columns.Count == 0) return;
            try
            {
                dbm.DeleteColumn(tabControl.SelectedIndex, dataGridView.CurrentCell.ColumnIndex);
            }
            catch { }

            int ind = tabControl.SelectedIndex;
            if (ind != -1) VisualTable(dbm.GetTable(ind));
        }

        private void butDeleteTable_Click(object sender, EventArgs e)
        {
            if (tabControl.TabCount == 0) return;
            try
            {
                dbm.DeleteTable(tabControl.SelectedIndex);
                tabControl.TabPages.RemoveAt(tabControl.SelectedIndex);
            }
            catch { }
            if (tabControl.TabCount == 0) return;

            int ind = tabControl.SelectedIndex;
            if (ind != -1) VisualTable(dbm.GetTable(ind));
        }

        private void butSaveDB_Click(object sender, EventArgs e)
        {
            if (!curFilePath.Equals(""))
            {
                dbm.SaveDB(curFilePath);
            }
            else
            {
                Stream myStream;

                sfdSaveDB.Filter = "tdb files (*.tdb)|*.tdb";
                sfdSaveDB.FilterIndex = 1;
                sfdSaveDB.RestoreDirectory = true;

                if (sfdSaveDB.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = sfdSaveDB.OpenFile()) != null)
                    {
                        // Code to write the stream goes here.
                        myStream.Close();

                        dbm.SaveDB(sfdSaveDB.FileName);
                    }
                }
            }
        }

        private void butOpen_Click(object sender, EventArgs e)
        {
            if (!curFilePath.Equals(""))
            {
                DialogResult dialogResult = MessageBox.Show("Зберегти зміни?", "Увага!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    dbm.SaveDB(curFilePath);
                }
            }

            ofdOpenDB.Filter = "tdb files (*.tdb)|*.tdb";
            ofdOpenDB.FilterIndex = 1;
            ofdOpenDB.RestoreDirectory = true;

            if (ofdChooseFilePath.ShowDialog() == DialogResult.OK)
            {
                curFilePath = ofdChooseFilePath.FileName;
                dbm.OpenDB(ofdChooseFilePath.FileName);
            }

            tabControl.TabPages.Clear();
            List<string> buf = dbm.GetTableNameList();
            foreach (string s in buf)
                tabControl.TabPages.Add(s);

            int ind = tabControl.SelectedIndex;
            if (ind != -1) VisualTable(dbm.GetTable(ind));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!curFilePath.Equals(""))
            {
                DialogResult dialogResult = MessageBox.Show("Зберегти зміни?", "Увага!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    dbm.SaveDB(curFilePath);
                }
            }
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Right))
            {
                string s = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                bdTypePath isFile = new bdTypePath();
                if (isFile.Validation(s))
                {
                    FormOpenFile f = new FormOpenFile(s);
                    f.Show();
                }
            }
        }
    }
}