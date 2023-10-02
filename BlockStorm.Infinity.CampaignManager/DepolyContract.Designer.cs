namespace BlockStorm.Infinity.CampaignManager
{
    partial class DepolyContractForm
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
            lblDeployer = new Label();
            txtDeployer = new TextBox();
            btnGeneratorDeployer = new Button();
            lblPK = new Label();
            txtPK = new TextBox();
            btnCopyDeployer = new Button();
            gbGenerateDeployer = new GroupBox();
            btnCopyOK = new Button();
            gbDeployContract = new GroupBox();
            btnCopyTotalSupply = new Button();
            txtTotalSupply = new TextBox();
            btnGenerateTotalSupply = new Button();
            btnCopyAuthCode = new Button();
            txtAuthCode = new TextBox();
            btnCalAuthCode = new Button();
            txtSymbol = new TextBox();
            lblSymbol = new Label();
            txtTokenName = new TextBox();
            lblTokenName = new Label();
            BtnSaveContract = new Button();
            txtFuncSig = new TextBox();
            lblFuncSig = new Label();
            txtContractAddress = new TextBox();
            lblContractAddress = new Label();
            txtChain = new TextBox();
            lblChain = new Label();
            txtAmountTosend = new TextBox();
            lblETH = new Label();
            btnSendFunds = new Button();
            gbSendFund = new GroupBox();
            lblETH2 = new Label();
            txtDeployerBalance = new TextBox();
            lblDeployerBalance = new Label();
            lblWETH = new Label();
            lblETH1 = new Label();
            txtWETHBalance = new TextBox();
            txtETHBalance = new TextBox();
            lblControllerBalance = new Label();
            txtControllerAddress = new TextBox();
            lblController = new Label();
            gbGenerateDeployer.SuspendLayout();
            gbDeployContract.SuspendLayout();
            gbSendFund.SuspendLayout();
            SuspendLayout();
            // 
            // lblDeployer
            // 
            lblDeployer.AutoSize = true;
            lblDeployer.Location = new Point(6, 57);
            lblDeployer.Name = "lblDeployer";
            lblDeployer.Size = new Size(44, 17);
            lblDeployer.TabIndex = 0;
            lblDeployer.Text = "部署者";
            // 
            // txtDeployer
            // 
            txtDeployer.Location = new Point(56, 54);
            txtDeployer.Name = "txtDeployer";
            txtDeployer.ReadOnly = true;
            txtDeployer.Size = new Size(270, 23);
            txtDeployer.TabIndex = 1;
            // 
            // btnGeneratorDeployer
            // 
            btnGeneratorDeployer.Location = new Point(8, 22);
            btnGeneratorDeployer.Name = "btnGeneratorDeployer";
            btnGeneratorDeployer.Size = new Size(93, 23);
            btnGeneratorDeployer.TabIndex = 2;
            btnGeneratorDeployer.Text = "生成部署者";
            btnGeneratorDeployer.UseVisualStyleBackColor = true;
            btnGeneratorDeployer.Click += BtnGeneratorDeployer_Click;
            // 
            // lblPK
            // 
            lblPK.AutoSize = true;
            lblPK.Location = new Point(8, 94);
            lblPK.Name = "lblPK";
            lblPK.Size = new Size(32, 17);
            lblPK.TabIndex = 3;
            lblPK.Text = "私钥";
            // 
            // txtPK
            // 
            txtPK.Location = new Point(56, 91);
            txtPK.Name = "txtPK";
            txtPK.ReadOnly = true;
            txtPK.Size = new Size(270, 23);
            txtPK.TabIndex = 4;
            // 
            // btnCopyDeployer
            // 
            btnCopyDeployer.Location = new Point(334, 52);
            btnCopyDeployer.Name = "btnCopyDeployer";
            btnCopyDeployer.Size = new Size(61, 28);
            btnCopyDeployer.TabIndex = 5;
            btnCopyDeployer.Text = "复制";
            btnCopyDeployer.UseVisualStyleBackColor = true;
            btnCopyDeployer.Click += BtnCopyDeployer_Click;
            // 
            // gbGenerateDeployer
            // 
            gbGenerateDeployer.Controls.Add(btnCopyOK);
            gbGenerateDeployer.Controls.Add(btnGeneratorDeployer);
            gbGenerateDeployer.Controls.Add(btnCopyDeployer);
            gbGenerateDeployer.Controls.Add(lblDeployer);
            gbGenerateDeployer.Controls.Add(txtPK);
            gbGenerateDeployer.Controls.Add(txtDeployer);
            gbGenerateDeployer.Controls.Add(lblPK);
            gbGenerateDeployer.Location = new Point(4, 12);
            gbGenerateDeployer.Name = "gbGenerateDeployer";
            gbGenerateDeployer.Size = new Size(414, 143);
            gbGenerateDeployer.TabIndex = 6;
            gbGenerateDeployer.TabStop = false;
            gbGenerateDeployer.Text = "Generate Deployer";
            // 
            // btnCopyOK
            // 
            btnCopyOK.Location = new Point(334, 87);
            btnCopyOK.Name = "btnCopyOK";
            btnCopyOK.Size = new Size(61, 28);
            btnCopyOK.TabIndex = 6;
            btnCopyOK.Text = "复制";
            btnCopyOK.UseVisualStyleBackColor = true;
            btnCopyOK.Click += BtnCopyPK_Click;
            // 
            // gbDeployContract
            // 
            gbDeployContract.Controls.Add(btnCopyTotalSupply);
            gbDeployContract.Controls.Add(txtTotalSupply);
            gbDeployContract.Controls.Add(btnGenerateTotalSupply);
            gbDeployContract.Controls.Add(btnCopyAuthCode);
            gbDeployContract.Controls.Add(txtAuthCode);
            gbDeployContract.Controls.Add(btnCalAuthCode);
            gbDeployContract.Controls.Add(txtSymbol);
            gbDeployContract.Controls.Add(lblSymbol);
            gbDeployContract.Controls.Add(txtTokenName);
            gbDeployContract.Controls.Add(lblTokenName);
            gbDeployContract.Controls.Add(BtnSaveContract);
            gbDeployContract.Controls.Add(txtFuncSig);
            gbDeployContract.Controls.Add(lblFuncSig);
            gbDeployContract.Controls.Add(txtContractAddress);
            gbDeployContract.Controls.Add(lblContractAddress);
            gbDeployContract.Controls.Add(txtChain);
            gbDeployContract.Controls.Add(lblChain);
            gbDeployContract.Location = new Point(4, 302);
            gbDeployContract.Name = "gbDeployContract";
            gbDeployContract.Size = new Size(414, 335);
            gbDeployContract.TabIndex = 7;
            gbDeployContract.TabStop = false;
            gbDeployContract.Text = "部署Token合约";
            // 
            // btnCopyTotalSupply
            // 
            btnCopyTotalSupply.Location = new Point(348, 166);
            btnCopyTotalSupply.Name = "btnCopyTotalSupply";
            btnCopyTotalSupply.Size = new Size(56, 23);
            btnCopyTotalSupply.TabIndex = 16;
            btnCopyTotalSupply.Text = "复制";
            btnCopyTotalSupply.UseVisualStyleBackColor = true;
            btnCopyTotalSupply.Click += BtnCopyTotalSupply_Click;
            // 
            // txtTotalSupply
            // 
            txtTotalSupply.Location = new Point(127, 166);
            txtTotalSupply.Name = "txtTotalSupply";
            txtTotalSupply.ReadOnly = true;
            txtTotalSupply.Size = new Size(214, 23);
            txtTotalSupply.TabIndex = 15;
            // 
            // btnGenerateTotalSupply
            // 
            btnGenerateTotalSupply.Location = new Point(6, 164);
            btnGenerateTotalSupply.Name = "btnGenerateTotalSupply";
            btnGenerateTotalSupply.Size = new Size(113, 25);
            btnGenerateTotalSupply.TabIndex = 14;
            btnGenerateTotalSupply.Text = "生成TotalSupply";
            btnGenerateTotalSupply.UseVisualStyleBackColor = true;
            btnGenerateTotalSupply.Click += BtnGenerateTotalSupply_Click;
            // 
            // btnCopyAuthCode
            // 
            btnCopyAuthCode.Location = new Point(347, 119);
            btnCopyAuthCode.Name = "btnCopyAuthCode";
            btnCopyAuthCode.Size = new Size(58, 23);
            btnCopyAuthCode.TabIndex = 13;
            btnCopyAuthCode.Text = "复制";
            btnCopyAuthCode.UseVisualStyleBackColor = true;
            btnCopyAuthCode.Click += BtnCopyAuthCode_Click;
            // 
            // txtAuthCode
            // 
            txtAuthCode.Location = new Point(96, 119);
            txtAuthCode.Name = "txtAuthCode";
            txtAuthCode.ReadOnly = true;
            txtAuthCode.Size = new Size(245, 23);
            txtAuthCode.TabIndex = 12;
            // 
            // btnCalAuthCode
            // 
            btnCalAuthCode.Location = new Point(6, 118);
            btnCalAuthCode.Name = "btnCalAuthCode";
            btnCalAuthCode.Size = new Size(84, 23);
            btnCalAuthCode.TabIndex = 11;
            btnCalAuthCode.Text = "计算授权码";
            btnCalAuthCode.UseVisualStyleBackColor = true;
            btnCalAuthCode.Click += BtnCalAuthCode_Click;
            // 
            // txtSymbol
            // 
            txtSymbol.Location = new Point(241, 75);
            txtSymbol.Name = "txtSymbol";
            txtSymbol.Size = new Size(100, 23);
            txtSymbol.TabIndex = 10;
            // 
            // lblSymbol
            // 
            lblSymbol.AutoSize = true;
            lblSymbol.Location = new Point(179, 78);
            lblSymbol.Name = "lblSymbol";
            lblSymbol.Size = new Size(54, 17);
            lblSymbol.TabIndex = 9;
            lblSymbol.Text = "Symbol:";
            // 
            // txtTokenName
            // 
            txtTokenName.Location = new Point(70, 75);
            txtTokenName.Name = "txtTokenName";
            txtTokenName.Size = new Size(103, 23);
            txtTokenName.TabIndex = 8;
            // 
            // lblTokenName
            // 
            lblTokenName.AutoSize = true;
            lblTokenName.Location = new Point(13, 78);
            lblTokenName.Name = "lblTokenName";
            lblTokenName.Size = new Size(50, 17);
            lblTokenName.TabIndex = 7;
            lblTokenName.Text = "Name: ";
            // 
            // BtnSaveContract
            // 
            BtnSaveContract.Enabled = false;
            BtnSaveContract.Location = new Point(156, 293);
            BtnSaveContract.Name = "BtnSaveContract";
            BtnSaveContract.Size = new Size(116, 28);
            BtnSaveContract.TabIndex = 6;
            BtnSaveContract.Text = "保存合约";
            BtnSaveContract.UseVisualStyleBackColor = true;
            BtnSaveContract.Click += BtnSaveContract_ClickAsync;
            // 
            // txtFuncSig
            // 
            txtFuncSig.Location = new Point(70, 250);
            txtFuncSig.Name = "txtFuncSig";
            txtFuncSig.Size = new Size(325, 23);
            txtFuncSig.TabIndex = 5;
            // 
            // lblFuncSig
            // 
            lblFuncSig.AutoSize = true;
            lblFuncSig.Location = new Point(8, 253);
            lblFuncSig.Name = "lblFuncSig";
            lblFuncSig.Size = new Size(56, 17);
            lblFuncSig.TabIndex = 4;
            lblFuncSig.Text = "函数签名";
            // 
            // txtContractAddress
            // 
            txtContractAddress.Location = new Point(70, 208);
            txtContractAddress.Name = "txtContractAddress";
            txtContractAddress.Size = new Size(325, 23);
            txtContractAddress.TabIndex = 3;
            // 
            // lblContractAddress
            // 
            lblContractAddress.AutoSize = true;
            lblContractAddress.Location = new Point(8, 211);
            lblContractAddress.Name = "lblContractAddress";
            lblContractAddress.Size = new Size(56, 17);
            lblContractAddress.TabIndex = 2;
            lblContractAddress.Text = "合约地址";
            // 
            // txtChain
            // 
            txtChain.Location = new Point(70, 37);
            txtChain.Name = "txtChain";
            txtChain.ReadOnly = true;
            txtChain.Size = new Size(325, 23);
            txtChain.TabIndex = 1;
            // 
            // lblChain
            // 
            lblChain.AutoSize = true;
            lblChain.Location = new Point(8, 40);
            lblChain.Name = "lblChain";
            lblChain.Size = new Size(56, 17);
            lblChain.TabIndex = 0;
            lblChain.Text = "目标网络";
            // 
            // txtAmountTosend
            // 
            txtAmountTosend.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            txtAmountTosend.Location = new Point(8, 31);
            txtAmountTosend.Name = "txtAmountTosend";
            txtAmountTosend.Size = new Size(93, 23);
            txtAmountTosend.TabIndex = 7;
            // 
            // lblETH
            // 
            lblETH.AutoSize = true;
            lblETH.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblETH.Location = new Point(65, 34);
            lblETH.Name = "lblETH";
            lblETH.Size = new Size(33, 17);
            lblETH.TabIndex = 8;
            lblETH.Text = "ETH";
            // 
            // btnSendFunds
            // 
            btnSendFunds.Enabled = false;
            btnSendFunds.Location = new Point(102, 31);
            btnSendFunds.Name = "btnSendFunds";
            btnSendFunds.Size = new Size(121, 23);
            btnSendFunds.TabIndex = 9;
            btnSendFunds.Text = "向部署者发送资金";
            btnSendFunds.UseVisualStyleBackColor = true;
            btnSendFunds.Click += BtnSendFunds_ClickAsync;
            // 
            // gbSendFund
            // 
            gbSendFund.Controls.Add(lblETH2);
            gbSendFund.Controls.Add(txtDeployerBalance);
            gbSendFund.Controls.Add(lblDeployerBalance);
            gbSendFund.Controls.Add(lblWETH);
            gbSendFund.Controls.Add(lblETH1);
            gbSendFund.Controls.Add(txtWETHBalance);
            gbSendFund.Controls.Add(txtETHBalance);
            gbSendFund.Controls.Add(lblControllerBalance);
            gbSendFund.Controls.Add(txtControllerAddress);
            gbSendFund.Controls.Add(lblController);
            gbSendFund.Controls.Add(btnSendFunds);
            gbSendFund.Controls.Add(lblETH);
            gbSendFund.Controls.Add(txtAmountTosend);
            gbSendFund.Location = new Point(4, 161);
            gbSendFund.Name = "gbSendFund";
            gbSendFund.Size = new Size(414, 135);
            gbSendFund.TabIndex = 8;
            gbSendFund.TabStop = false;
            gbSendFund.Text = "分配资金";
            // 
            // lblETH2
            // 
            lblETH2.AutoSize = true;
            lblETH2.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblETH2.Location = new Point(375, 35);
            lblETH2.Name = "lblETH2";
            lblETH2.Size = new Size(33, 17);
            lblETH2.TabIndex = 19;
            lblETH2.Text = "ETH";
            // 
            // txtDeployerBalance
            // 
            txtDeployerBalance.Location = new Point(292, 33);
            txtDeployerBalance.Name = "txtDeployerBalance";
            txtDeployerBalance.ReadOnly = true;
            txtDeployerBalance.Size = new Size(116, 23);
            txtDeployerBalance.TabIndex = 18;
            // 
            // lblDeployerBalance
            // 
            lblDeployerBalance.AutoSize = true;
            lblDeployerBalance.Location = new Point(222, 35);
            lblDeployerBalance.Name = "lblDeployerBalance";
            lblDeployerBalance.Size = new Size(68, 17);
            lblDeployerBalance.TabIndex = 17;
            lblDeployerBalance.Text = "部署者余额";
            // 
            // lblWETH
            // 
            lblWETH.AutoSize = true;
            lblWETH.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblWETH.Location = new Point(359, 106);
            lblWETH.Name = "lblWETH";
            lblWETH.Size = new Size(46, 17);
            lblWETH.TabIndex = 16;
            lblWETH.Text = "WETH";
            // 
            // lblETH1
            // 
            lblETH1.AutoSize = true;
            lblETH1.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblETH1.Location = new Point(229, 106);
            lblETH1.Name = "lblETH1";
            lblETH1.Size = new Size(33, 17);
            lblETH1.TabIndex = 15;
            lblETH1.Text = "ETH";
            // 
            // txtWETHBalance
            // 
            txtWETHBalance.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            txtWETHBalance.Location = new Point(275, 103);
            txtWETHBalance.Name = "txtWETHBalance";
            txtWETHBalance.ReadOnly = true;
            txtWETHBalance.Size = new Size(132, 23);
            txtWETHBalance.TabIndex = 14;
            // 
            // txtETHBalance
            // 
            txtETHBalance.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            txtETHBalance.Location = new Point(133, 103);
            txtETHBalance.Name = "txtETHBalance";
            txtETHBalance.ReadOnly = true;
            txtETHBalance.Size = new Size(131, 23);
            txtETHBalance.TabIndex = 13;
            // 
            // lblControllerBalance
            // 
            lblControllerBalance.AutoSize = true;
            lblControllerBalance.Location = new Point(8, 109);
            lblControllerBalance.Name = "lblControllerBalance";
            lblControllerBalance.Size = new Size(122, 17);
            lblControllerBalance.TabIndex = 12;
            lblControllerBalance.Text = "Controller Balance: ";
            // 
            // txtControllerAddress
            // 
            txtControllerAddress.Font = new Font("Microsoft YaHei UI", 7.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtControllerAddress.Location = new Point(132, 70);
            txtControllerAddress.Name = "txtControllerAddress";
            txtControllerAddress.ReadOnly = true;
            txtControllerAddress.Size = new Size(276, 20);
            txtControllerAddress.TabIndex = 11;
            // 
            // lblController
            // 
            lblController.AutoSize = true;
            lblController.Location = new Point(7, 73);
            lblController.Name = "lblController";
            lblController.Size = new Size(125, 17);
            lblController.TabIndex = 10;
            lblController.Text = "Controller Address: ";
            // 
            // DepolyContractForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(424, 649);
            Controls.Add(gbSendFund);
            Controls.Add(gbDeployContract);
            Controls.Add(gbGenerateDeployer);
            Name = "DepolyContractForm";
            Text = "部署合约";
            Load += DepolyContractForm_Load;
            gbGenerateDeployer.ResumeLayout(false);
            gbGenerateDeployer.PerformLayout();
            gbDeployContract.ResumeLayout(false);
            gbDeployContract.PerformLayout();
            gbSendFund.ResumeLayout(false);
            gbSendFund.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblDeployer;
        private TextBox txtDeployer;
        private Button btnGeneratorDeployer;
        private Label lblPK;
        private TextBox txtPK;
        private Button btnCopyDeployer;
        private GroupBox gbGenerateDeployer;
        private Button btnCopyOK;
        private GroupBox gbDeployContract;
        private Label lblFuncSig;
        private TextBox txtContractAddress;
        private Label lblContractAddress;
        private TextBox txtChain;
        private Label lblChain;
        private TextBox txtFuncSig;
        private Button BtnSaveContract;
        private Button btnSendFunds;
        private Label lblETH;
        private TextBox txtAmountTosend;
        private GroupBox gbSendFund;
        private TextBox txtControllerAddress;
        private Label lblController;
        private Label lblWETH;
        private Label lblETH1;
        private TextBox txtWETHBalance;
        private TextBox txtETHBalance;
        private Label lblControllerBalance;
        private Label lblDeployerBalance;
        private Label lblETH2;
        private TextBox txtDeployerBalance;
        private TextBox txtSymbol;
        private Label lblSymbol;
        private TextBox txtTokenName;
        private Label lblTokenName;
        private Button btnCopyAuthCode;
        private TextBox txtAuthCode;
        private Button btnCalAuthCode;
        private Button btnCopyTotalSupply;
        private TextBox txtTotalSupply;
        private Button btnGenerateTotalSupply;
    }
}