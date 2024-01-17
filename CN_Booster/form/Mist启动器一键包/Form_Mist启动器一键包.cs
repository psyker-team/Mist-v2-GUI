namespace Mist启动器一键包
{
    public partial class Form_Mist启动器一键包 : Form
    {
        public Form_Mist启动器一键包()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Mist启动器一键包));
            imageButtonRunMist = new ImageButton();
            imageButtonKnowMore = new ImageButton();
            SuspendLayout();
            // 
            // imageButtonRunMist
            // 
            imageButtonRunMist.BackColor = Color.Transparent;
            imageButtonRunMist.BackgroundImage = Properties.Resources.button_run_mist;
            resources.ApplyResources(imageButtonRunMist, "imageButtonRunMist");
            imageButtonRunMist.ClickedBackgroundImage = Properties.Resources.button_run_mist_clicked;
            imageButtonRunMist.DefaultBackgroundImage = Properties.Resources.button_run_mist;
            imageButtonRunMist.HoveredBackgroundImage = Properties.Resources.button_run_mist_hovered;
            imageButtonRunMist.Name = "imageButtonRunMist";
            imageButtonRunMist.Click += ImageButtonRunMist_Click;
            // 
            // imageButtonKnowMore
            // 
            imageButtonKnowMore.BackColor = Color.Transparent;
            imageButtonKnowMore.BackgroundImage = Properties.Resources.know_more;
            resources.ApplyResources(imageButtonKnowMore, "imageButtonKnowMore");
            imageButtonKnowMore.ClickedBackgroundImage = Properties.Resources.know_more;
            imageButtonKnowMore.DefaultBackgroundImage = Properties.Resources.know_more;
            imageButtonKnowMore.HoveredBackgroundImage = Properties.Resources.know_more;
            imageButtonKnowMore.Name = "imageButtonKnowMore";
            imageButtonKnowMore.Click += ImageButtonKnowMore_Click;
            // 
            // Form_Mist启动器一键包
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            BackgroundImage = Properties.Resources.background;
            resources.ApplyResources(this, "$this");
            Controls.Add(imageButtonKnowMore);
            Controls.Add(imageButtonRunMist);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form_Mist启动器一键包";
            ResumeLayout(false);
        }

        private ImageButton imageButtonRunMist;
        private ImageButton imageButtonKnowMore;

        private void ImageButtonRunMist_Click(object sender, EventArgs e)
        {
            envManager.RunMist(this);
        }

        private void ImageButtonKnowMore_Click(object sender, EventArgs e)
        {
            ShowLink();
        }
    }
}