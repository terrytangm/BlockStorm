namespace BlockStorm.Infinity.CampaignManager
{
    partial class ClosingDoor
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
            lblCampaignID = new Label();
            lblChainID = new Label();
            lblNetwork = new Label();
            gbInfo = new GroupBox();
            lblWETH3 = new Label();
            txtTotal = new TextBox();
            lblTotal = new Label();
            lblWETH2 = new Label();
            txtToClose = new TextBox();
            lblToClose = new Label();
            lblWETH1 = new Label();
            txtClosedAmt = new TextBox();
            lblClosed = new Label();
            txtWETH = new TextBox();
            lblWETH = new Label();
            txtCloser = new TextBox();
            lblCloser = new Label();
            txtPair = new TextBox();
            lblPair = new Label();
            txtTokenName = new TextBox();
            lblTokenName = new Label();
            txtToken = new TextBox();
            lblToken = new Label();
            gbAddressToClose = new GroupBox();
            txtCloseDoorInfo = new TextBox();
            btnAutoClose = new Button();
            btnCloseSelected = new Button();
            btnUnselectAll = new Button();
            btnSelectAll = new Button();
            cblAddressesToClose = new CheckedListBox();
            txtInfo = new TextBox();
            lbCloseDoorRecords = new ListBox();
            gbInfo.SuspendLayout();
            gbAddressToClose.SuspendLayout();
            SuspendLayout();
            // 
            // lblCampaignID
            // 
            lblCampaignID.AutoSize = true;
            lblCampaignID.Location = new Point(6, 30);
            lblCampaignID.Name = "lblCampaignID";
            lblCampaignID.Size = new Size(84, 17);
            lblCampaignID.TabIndex = 0;
            lblCampaignID.Text = "Campaign ID";
            // 
            // lblChainID
            // 
            lblChainID.AutoSize = true;
            lblChainID.Location = new Point(204, 30);
            lblChainID.Name = "lblChainID";
            lblChainID.Size = new Size(57, 17);
            lblChainID.TabIndex = 1;
            lblChainID.Text = "Chain ID";
            // 
            // lblNetwork
            // 
            lblNetwork.AutoSize = true;
            lblNetwork.Location = new Point(392, 30);
            lblNetwork.Name = "lblNetwork";
            lblNetwork.Size = new Size(32, 17);
            lblNetwork.TabIndex = 2;
            lblNetwork.Text = "网络";
            // 
            // gbInfo
            // 
            gbInfo.Controls.Add(lblWETH3);
            gbInfo.Controls.Add(txtTotal);
            gbInfo.Controls.Add(lblTotal);
            gbInfo.Controls.Add(lblWETH2);
            gbInfo.Controls.Add(txtToClose);
            gbInfo.Controls.Add(lblToClose);
            gbInfo.Controls.Add(lblWETH1);
            gbInfo.Controls.Add(txtClosedAmt);
            gbInfo.Controls.Add(lblClosed);
            gbInfo.Controls.Add(txtWETH);
            gbInfo.Controls.Add(lblWETH);
            gbInfo.Controls.Add(txtCloser);
            gbInfo.Controls.Add(lblCloser);
            gbInfo.Controls.Add(txtPair);
            gbInfo.Controls.Add(lblPair);
            gbInfo.Controls.Add(txtTokenName);
            gbInfo.Controls.Add(lblTokenName);
            gbInfo.Controls.Add(txtToken);
            gbInfo.Controls.Add(lblToken);
            gbInfo.Controls.Add(lblCampaignID);
            gbInfo.Controls.Add(lblNetwork);
            gbInfo.Controls.Add(lblChainID);
            gbInfo.Location = new Point(3, 12);
            gbInfo.Name = "gbInfo";
            gbInfo.Size = new Size(772, 209);
            gbInfo.TabIndex = 3;
            gbInfo.TabStop = false;
            gbInfo.Text = "基本信息";
            // 
            // lblWETH3
            // 
            lblWETH3.AutoSize = true;
            lblWETH3.Location = new Point(715, 169);
            lblWETH3.Name = "lblWETH3";
            lblWETH3.Size = new Size(43, 17);
            lblWETH3.TabIndex = 21;
            lblWETH3.Text = "WETH";
            // 
            // txtTotal
            // 
            txtTotal.Location = new Point(559, 166);
            txtTotal.Name = "txtTotal";
            txtTotal.ReadOnly = true;
            txtTotal.Size = new Size(203, 23);
            txtTotal.TabIndex = 20;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(520, 170);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(35, 17);
            lblTotal.TabIndex = 19;
            lblTotal.Text = "总和:";
            // 
            // lblWETH2
            // 
            lblWETH2.AutoSize = true;
            lblWETH2.Location = new Point(715, 138);
            lblWETH2.Name = "lblWETH2";
            lblWETH2.Size = new Size(43, 17);
            lblWETH2.TabIndex = 18;
            lblWETH2.Text = "WETH";
            // 
            // txtToClose
            // 
            txtToClose.Location = new Point(559, 134);
            txtToClose.Name = "txtToClose";
            txtToClose.ReadOnly = true;
            txtToClose.Size = new Size(203, 23);
            txtToClose.TabIndex = 17;
            // 
            // lblToClose
            // 
            lblToClose.AutoSize = true;
            lblToClose.Location = new Point(508, 137);
            lblToClose.Name = "lblToClose";
            lblToClose.Size = new Size(47, 17);
            lblToClose.TabIndex = 16;
            lblToClose.Text = "待关门:";
            // 
            // lblWETH1
            // 
            lblWETH1.AutoSize = true;
            lblWETH1.Location = new Point(715, 101);
            lblWETH1.Name = "lblWETH1";
            lblWETH1.Size = new Size(43, 17);
            lblWETH1.TabIndex = 15;
            lblWETH1.Text = "WETH";
            // 
            // txtClosedAmt
            // 
            txtClosedAmt.Location = new Point(559, 98);
            txtClosedAmt.Name = "txtClosedAmt";
            txtClosedAmt.ReadOnly = true;
            txtClosedAmt.Size = new Size(203, 23);
            txtClosedAmt.TabIndex = 14;
            // 
            // lblClosed
            // 
            lblClosed.AutoSize = true;
            lblClosed.Location = new Point(508, 101);
            lblClosed.Name = "lblClosed";
            lblClosed.Size = new Size(47, 17);
            lblClosed.TabIndex = 13;
            lblClosed.Text = "已关门:";
            // 
            // txtWETH
            // 
            txtWETH.Location = new Point(56, 98);
            txtWETH.Name = "txtWETH";
            txtWETH.ReadOnly = true;
            txtWETH.Size = new Size(399, 23);
            txtWETH.TabIndex = 12;
            // 
            // lblWETH
            // 
            lblWETH.AutoSize = true;
            lblWETH.Location = new Point(4, 101);
            lblWETH.Name = "lblWETH";
            lblWETH.Size = new Size(46, 17);
            lblWETH.TabIndex = 11;
            lblWETH.Text = "WETH:";
            // 
            // txtCloser
            // 
            txtCloser.Location = new Point(56, 170);
            txtCloser.Name = "txtCloser";
            txtCloser.ReadOnly = true;
            txtCloser.Size = new Size(399, 23);
            txtCloser.TabIndex = 10;
            // 
            // lblCloser
            // 
            lblCloser.AutoSize = true;
            lblCloser.Location = new Point(2, 173);
            lblCloser.Name = "lblCloser";
            lblCloser.Size = new Size(48, 17);
            lblCloser.TabIndex = 9;
            lblCloser.Text = "Closer:";
            // 
            // txtPair
            // 
            txtPair.Location = new Point(56, 134);
            txtPair.Name = "txtPair";
            txtPair.ReadOnly = true;
            txtPair.Size = new Size(399, 23);
            txtPair.TabIndex = 8;
            // 
            // lblPair
            // 
            lblPair.AutoSize = true;
            lblPair.Location = new Point(17, 137);
            lblPair.Name = "lblPair";
            lblPair.Size = new Size(33, 17);
            lblPair.TabIndex = 7;
            lblPair.Text = "Pair:";
            // 
            // txtTokenName
            // 
            txtTokenName.Location = new Point(559, 61);
            txtTokenName.Name = "txtTokenName";
            txtTokenName.ReadOnly = true;
            txtTokenName.Size = new Size(203, 23);
            txtTokenName.TabIndex = 6;
            // 
            // lblTokenName
            // 
            lblTokenName.AutoSize = true;
            lblTokenName.Location = new Point(470, 64);
            lblTokenName.Name = "lblTokenName";
            lblTokenName.Size = new Size(90, 17);
            lblTokenName.TabIndex = 5;
            lblTokenName.Text = "Token Name: ";
            // 
            // txtToken
            // 
            txtToken.Location = new Point(56, 62);
            txtToken.Name = "txtToken";
            txtToken.ReadOnly = true;
            txtToken.Size = new Size(399, 23);
            txtToken.TabIndex = 4;
            // 
            // lblToken
            // 
            lblToken.AutoSize = true;
            lblToken.Location = new Point(3, 65);
            lblToken.Name = "lblToken";
            lblToken.Size = new Size(47, 17);
            lblToken.TabIndex = 3;
            lblToken.Text = "Token:";
            // 
            // gbAddressToClose
            // 
            gbAddressToClose.Controls.Add(txtCloseDoorInfo);
            gbAddressToClose.Controls.Add(btnAutoClose);
            gbAddressToClose.Controls.Add(btnCloseSelected);
            gbAddressToClose.Controls.Add(btnUnselectAll);
            gbAddressToClose.Controls.Add(btnSelectAll);
            gbAddressToClose.Controls.Add(cblAddressesToClose);
            gbAddressToClose.Location = new Point(3, 227);
            gbAddressToClose.Name = "gbAddressToClose";
            gbAddressToClose.Size = new Size(772, 543);
            gbAddressToClose.TabIndex = 5;
            gbAddressToClose.TabStop = false;
            gbAddressToClose.Text = "待关门地址";
            // 
            // txtCloseDoorInfo
            // 
            txtCloseDoorInfo.Location = new Point(353, 22);
            txtCloseDoorInfo.Multiline = true;
            txtCloseDoorInfo.Name = "txtCloseDoorInfo";
            txtCloseDoorInfo.ReadOnly = true;
            txtCloseDoorInfo.ScrollBars = ScrollBars.Vertical;
            txtCloseDoorInfo.Size = new Size(409, 503);
            txtCloseDoorInfo.TabIndex = 5;
            // 
            // btnAutoClose
            // 
            btnAutoClose.Location = new Point(264, 24);
            btnAutoClose.Name = "btnAutoClose";
            btnAutoClose.Size = new Size(75, 23);
            btnAutoClose.TabIndex = 4;
            btnAutoClose.Text = "自动关门";
            btnAutoClose.UseVisualStyleBackColor = true;
            btnAutoClose.Click += btnAutoClose_Click;
            // 
            // btnCloseSelected
            // 
            btnCloseSelected.Location = new Point(132, 24);
            btnCloseSelected.Name = "btnCloseSelected";
            btnCloseSelected.Size = new Size(75, 23);
            btnCloseSelected.TabIndex = 3;
            btnCloseSelected.Text = "关门选中";
            btnCloseSelected.UseVisualStyleBackColor = true;
            btnCloseSelected.Click += btnCloseSelected_Click;
            // 
            // btnUnselectAll
            // 
            btnUnselectAll.Location = new Point(67, 24);
            btnUnselectAll.Name = "btnUnselectAll";
            btnUnselectAll.Size = new Size(59, 23);
            btnUnselectAll.TabIndex = 2;
            btnUnselectAll.Text = "全不选";
            btnUnselectAll.UseVisualStyleBackColor = true;
            btnUnselectAll.Click += btnUnselectAll_Click;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Location = new Point(4, 24);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(57, 23);
            btnSelectAll.TabIndex = 1;
            btnSelectAll.Text = "全选";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // cblAddressesToClose
            // 
            cblAddressesToClose.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            cblAddressesToClose.FormattingEnabled = true;
            cblAddressesToClose.Location = new Point(0, 53);
            cblAddressesToClose.Name = "cblAddressesToClose";
            cblAddressesToClose.Size = new Size(339, 472);
            cblAddressesToClose.TabIndex = 0;
            // 
            // txtInfo
            // 
            txtInfo.Location = new Point(781, 12);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.Size = new Size(671, 1167);
            txtInfo.TabIndex = 6;
            // 
            // lbCloseDoorRecords
            // 
            lbCloseDoorRecords.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            lbCloseDoorRecords.FormattingEnabled = true;
            lbCloseDoorRecords.ItemHeight = 20;
            lbCloseDoorRecords.Location = new Point(3, 785);
            lbCloseDoorRecords.Name = "lbCloseDoorRecords";
            lbCloseDoorRecords.Size = new Size(772, 404);
            lbCloseDoorRecords.TabIndex = 7;
            // 
            // ClosingDoor
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1464, 1199);
            Controls.Add(lbCloseDoorRecords);
            Controls.Add(txtInfo);
            Controls.Add(gbAddressToClose);
            Controls.Add(gbInfo);
            Name = "ClosingDoor";
            Text = "ClosingDoor";
            FormClosed += ClosingDoor_FormClosed;
            Load += ClosingDoor_Load;
            gbInfo.ResumeLayout(false);
            gbInfo.PerformLayout();
            gbAddressToClose.ResumeLayout(false);
            gbAddressToClose.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCampaignID;
        private Label lblChainID;
        private Label lblNetwork;
        private GroupBox gbInfo;
        private TextBox txtToken;
        private Label lblToken;
        private TextBox txtTokenName;
        private Label lblTokenName;
        private TextBox txtCloser;
        private Label lblCloser;
        private TextBox txtPair;
        private Label lblPair;
        private TextBox txtClosedAmt;
        private Label lblClosed;
        private TextBox txtWETH;
        private Label lblWETH;
        private Label lblWETH1;
        private TextBox txtToClose;
        private Label lblToClose;
        private Label lblWETH3;
        private TextBox txtTotal;
        private Label lblTotal;
        private Label lblWETH2;
        private GroupBox gbAddressToClose;
        private CheckedListBox cblAddressesToClose;
        private TextBox txtInfo;
        private Button btnUnselectAll;
        private Button btnSelectAll;
        private Button btnAutoClose;
        private Button btnCloseSelected;
        private TextBox txtCloseDoorInfo;
        private ListBox lbCloseDoorRecords;
    }
}