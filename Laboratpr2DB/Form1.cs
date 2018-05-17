using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Laboratpr2DB
{
    public partial class Form1 : Form
    {
        DataSet dataSet = new DataSet();
        string connection = ConfigurationManager.AppSettings["SQLconnectionString"];
        //SqlConnection sqlConnection = new SqlConnection(connection);
        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        SqlDataAdapter dataAdapterChildTable = new SqlDataAdapter();
        BindingSource bindingSourceForParentTable= new BindingSource();
        BindingSource bindingSourceForChildTable = new BindingSource();
        int currentRowClickedFromParent;
        int currentRowClickedChild;
        string parentTableKey = ConfigurationManager.AppSettings["ParentTable"];
        string childTable = ConfigurationManager.AppSettings["ChildTable"];
        DataTable table = new DataTable();
        TextBox[] textboxesForInsert = new TextBox[100];
        TextBox[] textboxesForUpdate = new TextBox[100];
        int currentSelecedForeignKeyFromChild = 0;


        public Form1()
        {
            InitializeComponent();
            
        }


        public int GetIndexOfForeignKey()
        {
            string foreignKeyName = getForeignKeyName();
            int foreignKeyIndex = 0;
            var dictColumnNames = getListOfColumnNames(childTable);
            for (int i = 0; i < dictColumnNames.Count; i++)
            {
                if (dictColumnNames.ElementAt(i).Key.Equals(foreignKeyName))
                {
                    foreignKeyIndex = i;
                }
            }
            return foreignKeyIndex;

        }

        public Dictionary<string, string> getDictFOrInsert()
        {
            Dictionary<string, string> dictWithAllNames = getListOfColumnNames(childTable);
            int foreignKeyindex = GetIndexOfForeignKey();
            for (int i = 0; i < dictWithAllNames.Count; i++)
            {
                if (i == foreignKeyindex)
                    dictWithAllNames.Remove(dictWithAllNames.ElementAt(i).Key);             
            }
            return dictWithAllNames;
        }


        private void GetDataFromParent(string selectCommand)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                dataAdapter = new SqlDataAdapter(selectCommand, sqlConnection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSourceForParentTable.DataSource = table;
                dataGridView1PatentTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void GetDataFromChild(string selectCommand)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                dataAdapterChildTable = new SqlDataAdapter(selectCommand, sqlConnection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapterChildTable);
                DataTable tableChild = new DataTable();
                tableChild.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapterChildTable.Fill(tableChild);
                bindingSourceForChildTable.DataSource = tableChild;
                dataGridView2ChildTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {  
            dataGridView1PatentTable.DataSource = bindingSourceForParentTable;
            GetDataFromParent("select * from "+ parentTableKey + "");     
            dataGridView2ChildTable.DataSource = bindingSourceForChildTable;
            GetDataFromChild("Select * from " + childTable + "");

            int A = 1;

            Dictionary<string, string> columnsNamesChildTable = getDictFOrInsert();
            int columnsForInsert = getNumberOfColumnsFromTable(childTable)-1;
            for (int i = 0; i < columnsForInsert; i++)
            {
                TextBox txt = new TextBox();
                this.Controls.Add(txt);
                txt.Top = A + 402;
                txt.Left = 54;
                txt.Text = columnsNamesChildTable.ElementAt(i).Key;
                A = A + 25;
                textboxesForInsert[i] = txt;
            }
            A=1;
            Dictionary<string, string> columnsNamesChildTableUpdate = getDictOFColumnNamesAndTypesForUpdate(childTable);
            int columnsForUpdate = getNumberOfColumnsFromTable(childTable) - 2;
            for (int i = 0; i < columnsForUpdate; i++)
            {
                TextBox txt = new TextBox();
                this.Controls.Add(txt);
                txt.Top = A + 402;
                txt.Left = 680;
                txt.Text = columnsNamesChildTableUpdate.ElementAt(i).Key;
                A = A + 25;
                textboxesForUpdate[i] = txt;
            }
        }

       
        private void dataGridView1PatentTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {

                int index = e.RowIndex;
                DataGridViewRow selectedRow = dataGridView1PatentTable.Rows[index];
                var selectedID = selectedRow.Cells[0].Value.ToString();
                int selectedINTid = Int32.Parse(selectedID);
                currentRowClickedFromParent = selectedINTid;

                //string primaryKeyColumn = getPrimaryKeyName();
                string foreignKeyColumn = getForeignKeyName();

                dataAdapterChildTable.SelectCommand = new SqlCommand("Select * From " + childTable + " where " + foreignKeyColumn + " = @selected", sqlConnection);
                dataAdapterChildTable.SelectCommand.Parameters.Add("@selected", SqlDbType.Int).Value = selectedINTid;

                sqlConnection.Open();
                dataAdapterChildTable.SelectCommand.ExecuteNonQuery();
                dataGridView2ChildTable.AutoGenerateColumns = true;
                dataSet.Clear();
                dataAdapterChildTable.Fill(dataSet);
                dataGridView2ChildTable.DataSource = dataSet.Tables[0];
                sqlConnection.Close();
            }
       
        }

        public string getPrimaryKeyName(string tableName)
        {
            string sql = "SELECT ColumnName = col.column_name " +
              "FROM information_schema.table_constraints tc " +
              "INNER JOIN information_schema.key_column_usage col " +
              "ON col.Constraint_Name = tc.Constraint_Name " +
              "AND col.Constraint_schema = tc.Constraint_schema " +
              "WHERE tc.Constraint_Type = 'Primary Key' AND col.Table_name = '" + tableName + "'";

            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand selectPrimaryKey = new SqlCommand(sql);
                selectPrimaryKey.CommandType = CommandType.Text;
                selectPrimaryKey.Connection = sqlConnection;
                sqlConnection.Open();
                string columnName = (string)selectPrimaryKey.ExecuteScalar();
                sqlConnection.Close();
                return columnName;
            }    
        }

        public string getForeignKeyName()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                string sql = "SELECT COL_NAME(fc.parent_object_id, fc.parent_column_id) ColName, " +
                        " OBJECT_NAME(f.parent_object_id) TableName " +
                       "FROM sys.foreign_keys AS f " +
                        "INNER JOIN sys.foreign_key_columns AS fc " +
                      " ON f.OBJECT_ID = fc.constraint_object_id " +
                       "INNER JOIN sys.tables t " +
                       " ON t.OBJECT_ID = fc.referenced_object_id " +
                        " WHERE OBJECT_NAME(f.referenced_object_id) = '" + parentTableKey + "'";

                SqlCommand selectPrimaryKey = new SqlCommand(sql);
                selectPrimaryKey.CommandType = CommandType.Text;
                selectPrimaryKey.Connection = sqlConnection;
                sqlConnection.Open();
                string columnName = (string)selectPrimaryKey.ExecuteScalar();
                sqlConnection.Close();
                return columnName;
            }
        }

        private void dataGridView2ChildTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow selectedRow = dataGridView2ChildTable.Rows[index];
            var selectedIDfomChild = selectedRow.Cells[0].Value.ToString();           
            int selectedINTid = Int32.Parse(selectedIDfomChild);
            currentRowClickedChild = selectedINTid;


            string foreignKeyName = getForeignKeyName();
            int foreignKeiIndex = 0;
            var dictForInsert = getListOfColumnNames(childTable);
            for (int i = 0; i < dictForInsert.Count; i++)
            {
                if (dictForInsert.ElementAt(i).Key.Equals(foreignKeyName))
                    foreignKeiIndex = i;
            }
            var selectedForeignKeyFromChild = selectedRow.Cells[foreignKeiIndex].Value.ToString();
            int selectedForeignKeyINT = Int32.Parse(selectedForeignKeyFromChild);
            currentSelecedForeignKeyFromChild = selectedForeignKeyINT;
        }


        private void buttonDeleteChild_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                string primaryKeyColumn = getPrimaryKeyName(childTable);
                dataAdapterChildTable.DeleteCommand = new SqlCommand("Delete from " + childTable + " where [" + primaryKeyColumn + "] = @selected", sqlConnection);
                dataAdapterChildTable.DeleteCommand.Parameters.Add("@selected", SqlDbType.Int).Value = currentRowClickedChild;

                try
                {
                    sqlConnection.Open();
                }
                catch (Exception ex)
                { Debug.WriteLine(ex); }
                dataAdapterChildTable.DeleteCommand.ExecuteNonQuery();
                DisplayTableToDataGridView2ChildTable();
                sqlConnection.Close();
            }
        }

        public void DisplayTableToDataGridView2ChildTable()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                dataGridView2ChildTable.DataSource = bindingSourceForChildTable;
                dataAdapterChildTable = new SqlDataAdapter("Select * from " + childTable, sqlConnection);
                SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapterChildTable.Fill(table);
                bindingSourceForChildTable.DataSource = table;
            }
        }

        private void buttonDisplayChildTable_Click(object sender, EventArgs e)
        {
            DisplayTableToDataGridView2ChildTable();
        }


       
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            string foreignKey = getForeignKeyName();

            Dictionary<string, string> columnNamesAndTypesDict = new Dictionary<string, string>();
            columnNamesAndTypesDict = getListOfColumnNames(childTable);
            int parameterNumberForeignKey=0;
            for (int i = 0; i < columnNamesAndTypesDict.Count; i++)
            {
                if (foreignKey.Equals(columnNamesAndTypesDict.ElementAt(i).Key))
                {
                    parameterNumberForeignKey = i;
                }
            }

            string sqlCommand = "Insert into " + childTable + " values(";
            for (int index = 0; index < columnNamesAndTypesDict.Count; index++)
            {
                if (index == columnNamesAndTypesDict.Count - 1)
                {
                    sqlCommand = sqlCommand + "@value"+ index + ") ";
                }
                else
                {
                    sqlCommand = sqlCommand + "@value"+index + ", ";
                }
            }
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
               

                SqlCommand select = new SqlCommand(sqlCommand, sqlConnection);
                int textBoxIndex = 0;
                for (int i = 0; i < columnNamesAndTypesDict.Count; i++)
                {
                    string parameter = "@value" + i.ToString();
                    string type = columnNamesAndTypesDict.ElementAt(i).Value;
                    if (i == parameterNumberForeignKey)
                    {
                        SqlParameter sqlParam = new SqlParameter();
                        sqlParam.ParameterName = parameter;
                        sqlParam.Value = (Int32)currentRowClickedFromParent;
                        select.Parameters.Add(sqlParam);
                    }

                    else
                    {
                        SqlParameter sqlParam = new SqlParameter();
                        sqlParam.ParameterName = parameter;
                        sqlParam.Value = textboxesForInsert[textBoxIndex].Text;
                        select.Parameters.Add(sqlParam);
                        textBoxIndex++;
                    }
                }

                sqlConnection.Open();
                select.ExecuteNonQuery();
                DisplayTableToDataGridView2ChildTable();
                sqlConnection.Close();
            }
        }

        public int getNumberOfColumnsFromTable(string tableName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                string sql = "SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.Columns where TABLE_NAME = " + "'" + tableName + "'";
                SqlCommand selectNumberOfColumns = new SqlCommand(sql);
                selectNumberOfColumns.CommandType = CommandType.Text;
                selectNumberOfColumns.Connection = sqlConnection;
                sqlConnection.Open();
                int numberOfColumns = (Int32)selectNumberOfColumns.ExecuteScalar();
                sqlConnection.Close();
                return numberOfColumns;

            }
        }

        public int getNumberOfForeignKeysINTable(string tableName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                string sql = "SELECT COUNT(f.name) AS ForeignKey " +
                          "FROM sys.foreign_keys AS f " +
                          "INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id " +
                          "where OBJECT_NAME(f.parent_object_id) = '" + tableName + "'";
                SqlCommand selectNumberOfForeignKeys = new SqlCommand(sql);
                selectNumberOfForeignKeys.CommandType = CommandType.Text;
                selectNumberOfForeignKeys.Connection = sqlConnection;
                sqlConnection.Open();
                int numberOfColumns = (Int32)selectNumberOfForeignKeys.ExecuteScalar();
                sqlConnection.Close();
                return numberOfColumns;
            }

        }

        public Dictionary<string, string> getListOfColumnNames(string tablename)
        {
            Dictionary<string, string> columnNamesAndTypesDict = new Dictionary<string, string>();
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {

                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("select * from " + tablename, sqlConnection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columnNamesAndTypesDict.Add(reader.GetName(i), reader.GetDataTypeName(i));
                    }
                }
            
            sqlConnection.Close();
            return columnNamesAndTypesDict;
           }
        }

        public Dictionary<string, string> getDictOFColumnNamesAndTypesForUpdate(string tableName)
        {
            Dictionary<string, string> columnNamesAndTypesDictUpdate = getListOfColumnNames(childTable);
            var columnNamesAndTypesDict = getListOfColumnNames(childTable);
            string foreignKeyName = getForeignKeyName();
            string primaryKeyName = getPrimaryKeyName(childTable);
            for (int i = 0; i < columnNamesAndTypesDictUpdate.Count; i++)
            {
                if (columnNamesAndTypesDictUpdate.ElementAt(i).Key.Equals(primaryKeyName) || columnNamesAndTypesDictUpdate.ElementAt(i).Key.Equals(foreignKeyName))
                {
                    columnNamesAndTypesDictUpdate.Remove(columnNamesAndTypesDictUpdate.ElementAt(i).Key);
                }
            }
            return columnNamesAndTypesDictUpdate;
        }


        private void updateChildButton_Click(object sender, EventArgs e)
        {
            string foreignKey = getForeignKeyName();
            string primaryKeyName = getPrimaryKeyName(childTable);

            Dictionary<string, string> columnNamesAndTypesDict = new Dictionary<string, string>();
            columnNamesAndTypesDict = getListOfColumnNames(childTable);
            int parameterNumberPrimaryKey = 0;
            int parameterNumberForeignKey = 0;
            for (int i = 0; i < columnNamesAndTypesDict.Count; i++)
            {
                if (primaryKeyName.Equals(columnNamesAndTypesDict.ElementAt(i).Key))
                {
                    parameterNumberPrimaryKey = i;
                }
                if (foreignKey.Equals(columnNamesAndTypesDict.ElementAt(i).Key))
                {
                    parameterNumberForeignKey = i;
                }
                
            }

            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                var dictForUpdate = getDictOFColumnNamesAndTypesForUpdate(childTable);

                dataAdapterChildTable.DeleteCommand = new SqlCommand("Delete from " + childTable + " where [" + primaryKeyName + "] = @selected", sqlConnection);
                dataAdapterChildTable.DeleteCommand.Parameters.Add("@selected", SqlDbType.Int).Value = currentRowClickedChild;

                sqlConnection.Open();
                dataAdapterChildTable.DeleteCommand.ExecuteNonQuery();



                string sqlCommand = "Insert into " + childTable + " values(";
                for (int index = 0; index < columnNamesAndTypesDict.Count; index++)
                {
                    if (index == columnNamesAndTypesDict.Count - 1)
                    {
                        sqlCommand = sqlCommand + "@value" + index + ") ";
                    }
                    else
                    {
                        sqlCommand = sqlCommand + "@value" + index + ", ";
                    }
                }
                int textBoxIndex = 0;
                

                SqlCommand select = new SqlCommand(sqlCommand, sqlConnection);
                    for (int i = 0; i < columnNamesAndTypesDict.Count; i++)
                    {
                        string parameter = "@value" + i.ToString();
                        string type = columnNamesAndTypesDict.ElementAt(i).Value;

                    if (i == parameterNumberForeignKey)
                    {
                        
                        SqlParameter sqlParam = new SqlParameter();
                        sqlParam.ParameterName = parameter;
                        sqlParam.Value = currentSelecedForeignKeyFromChild;
                        select.Parameters.Add(sqlParam);
                    }
                    else if (i == parameterNumberPrimaryKey)
                    {
                        SqlParameter sqlParam = new SqlParameter();
                        sqlParam.ParameterName = parameter;
                        sqlParam.Value = (Int32)currentRowClickedChild;
                        select.Parameters.Add(sqlParam);
                    }

                    else
                    {
                        SqlParameter sqlParam = new SqlParameter();
                        sqlParam.ParameterName = parameter;
                        sqlParam.Value = textboxesForUpdate[textBoxIndex].Text;
                        select.Parameters.Add(sqlParam);
                        textBoxIndex++;
                    }
                    }

                    select.ExecuteNonQuery();
                    sqlConnection.Close();  
            }
            DisplayTableToDataGridView2ChildTable();
        }
    }
}
