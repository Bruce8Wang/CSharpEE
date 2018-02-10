using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lucky
{
    public class frmSetForm : Form
    {
        public frmSetForm()
        {
            var btnclose = new Button();
            var lblName = new Label();
            var lblNum = new Label();
            var txtName = new TextBox();
            var txtNum = new TextBox();
            SuspendLayout();
 
            btnclose.Anchor = AnchorStyles.Top;
            btnclose.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            btnclose.ForeColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            btnclose.Location = new Point(193, 178);
            btnclose.Name = "btnclose";
            btnclose.Size = new Size(210, 41);
            btnclose.TabIndex = 1;
            btnclose.Text = "保存并返回";
            btnclose.UseVisualStyleBackColor = true;
            btnclose.Click += (sender, e) =>
            {
                try
                {
                    var num = Convert.ToInt32(txtNum.Text);
                    if (num < 0 || num > 200)
                    {
                        txtNum.Clear();
                        txtNum.Focus();
                        MessageBox.Show("你输入的数字大小不符合!请输入1~200之间的数字!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                catch (Exception)
                {
                    txtNum.Clear();
                    txtNum.Focus();
                    MessageBox.Show("输入不符合要求，请输入数字!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    var name = txtName.Text;
                    var num = Convert.ToInt32(txtNum.Text);
                    UserHelper.SaveAward(name, num);
                    DialogResult = DialogResult.OK;
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败，请重试！", "出错了");
                }
                Close();
            };
 
            lblName.AutoSize = true;
            lblName.Font = new Font("Microsoft YaHei", 25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            lblName.ForeColor = Color.Yellow;
            lblName.Location = new Point(60, 38);
            lblName.Name = "lblName";
            lblName.Size = new Size(190, 45);
            lblName.TabIndex = 4;
            lblName.Text = "奖项名称：";

            lblNum.AutoSize = true;
            lblNum.Font = new Font("Microsoft YaHei", 25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            lblNum.ForeColor = Color.Yellow;
            lblNum.Location = new Point(60, 106);
            lblNum.Name = "lblNum";
            lblNum.Size = new Size(190, 45);
            lblNum.TabIndex = 5;
            lblNum.Text = "奖项数量：";

            txtName.Font = new Font("Microsoft YaHei", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            txtName.ForeColor = Color.Red;
            txtName.Location = new Point(268, 41);
            txtName.Name = "txtName";
            txtName.Size = new Size(260, 43);
            txtName.TabIndex = 6;

            txtNum.Font = new Font("Microsoft YaHei", 20F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            txtNum.ForeColor = Color.Red;
            txtNum.Location = new Point(269, 106);
            txtNum.Name = "txtNum";
            txtNum.Size = new Size(260, 43);
            txtNum.TabIndex = 7;

            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Red;
            ClientSize = new Size(605, 268);
            Controls.Add(txtNum);
            Controls.Add(txtName);
            Controls.Add(lblNum);
            Controls.Add(lblName);
            Controls.Add(btnclose);
            Name = "frmSetForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "新增奖项";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
