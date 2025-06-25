namespace VCheckViewer.Services.MessageBox
{
    public class CustomMessageBox : Form
    {
        private TextBox textBox;
        private Button button1;
        private Button button2;
        private CheckBox checkBox;

        public bool IsCheckBoxChecked { get; private set; }
        public string SelectedButton { get; private set; }

        public CustomMessageBox()
        {
            // Initialize components
            textBox = new TextBox() { Multiline = true, WordWrap = true, ReadOnly = true, Dock = DockStyle.Fill, Text = Properties.Resources.General_Message_NewVersionAvailable, Location = new Point(20, 20) };
            button1 = new Button { Text = Properties.Resources.General_Label_Yes, Location = new Point(100, 150), Width = 170, Height = 30, DialogResult = DialogResult.OK};
            button2 = new Button { Text = Properties.Resources.General_Label_No, Location = new Point(300, 150), Width = 170, Height = 30, DialogResult = DialogResult.Cancel };
            checkBox = new CheckBox { Text = Properties.Resources.General_Label_ThreeDaysReminder, Location = new Point(20, 70), AutoSize = true };

            // Add event handlers
            button1.Click += (sender, e) => { SelectedButton = "Yes"; Close(); };
            button2.Click += (sender, e) => { SelectedButton = "No"; Close(); };
            checkBox.CheckedChanged += (sender, e) => { IsCheckBoxChecked = checkBox.Checked; };
            
            // Add controls to the form
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(checkBox);
            Controls.Add(textBox);

            // Configure form
            Text = Properties.Resources.General_Message_NewVersionNotification;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            AcceptButton = button1;
            CancelButton = button2;
            Size = new Size(600, 250);
        }

        public static (string SelectedButton, bool IsCheckBoxChecked) Show()
        {
            using (var customMessageBox = new CustomMessageBox())
            {
                customMessageBox.ShowDialog();
                return (customMessageBox.SelectedButton, customMessageBox.IsCheckBoxChecked);
            }
        }
    }

}
