namespace Laboratpr2DB
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1PatentTable = new System.Windows.Forms.DataGridView();
            this.dataGridView2ChildTable = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.updateChildButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1PatentTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2ChildTable)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1PatentTable
            // 
            this.dataGridView1PatentTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1PatentTable.Location = new System.Drawing.Point(664, 74);
            this.dataGridView1PatentTable.Name = "dataGridView1PatentTable";
            this.dataGridView1PatentTable.RowTemplate.Height = 24;
            this.dataGridView1PatentTable.Size = new System.Drawing.Size(422, 280);
            this.dataGridView1PatentTable.TabIndex = 0;
            this.dataGridView1PatentTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1PatentTable_CellClick);
            // 
            // dataGridView2ChildTable
            // 
            this.dataGridView2ChildTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2ChildTable.Location = new System.Drawing.Point(73, 74);
            this.dataGridView2ChildTable.Name = "dataGridView2ChildTable";
            this.dataGridView2ChildTable.RowTemplate.Height = 24;
            this.dataGridView2ChildTable.Size = new System.Drawing.Size(410, 280);
            this.dataGridView2ChildTable.TabIndex = 1;
            this.dataGridView2ChildTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2ChildTable_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(940, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "Parent Table";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(362, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "Child Table";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(73, 421);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 45);
            this.button1.TabIndex = 4;
            this.button1.Text = "Insert Child";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonInsert_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(367, 421);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(141, 45);
            this.button2.TabIndex = 5;
            this.button2.Text = "Delete Child";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonDeleteChild_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(650, 421);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(141, 45);
            this.button3.TabIndex = 6;
            this.button3.Text = "Display Child Table";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.buttonDisplayChildTable_Click);
            // 
            // updateChildButton
            // 
            this.updateChildButton.Location = new System.Drawing.Point(904, 421);
            this.updateChildButton.Name = "updateChildButton";
            this.updateChildButton.Size = new System.Drawing.Size(141, 45);
            this.updateChildButton.TabIndex = 7;
            this.updateChildButton.Text = "Update Child";
            this.updateChildButton.UseVisualStyleBackColor = true;
            this.updateChildButton.Click += new System.EventHandler(this.updateChildButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 594);
            this.Controls.Add(this.updateChildButton);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView2ChildTable);
            this.Controls.Add(this.dataGridView1PatentTable);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1PatentTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2ChildTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1PatentTable;
        private System.Windows.Forms.DataGridView dataGridView2ChildTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button updateChildButton;
    }
}

