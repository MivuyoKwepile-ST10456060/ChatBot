using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Chatbot
{
    public class ChatbotForm : Form
    {
        private readonly UserProfile _user;
        private readonly ChatBotResponses _responses;

        private RichTextBox rtbChat = null!;
        private TextBox txtInput = null!;
        private Button btnSend = null!;
        private Label lblTitle = null!;
        private Panel bottomPanel = null!;

        public ChatbotForm()
        {
            _user = new UserProfile();
            _responses = new ChatBotResponses(_user);

            BuildInterface();
            ShowWelcomeMessage();
            PlayVoiceGreeting();
        }

        private void BuildInterface()
        {
            Text = "Cybersecurity Awareness Chatbot";
            Width = 900;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(18, 24, 35);

            lblTitle = new Label();
            lblTitle.Text = "Cybersecurity Awareness Chatbot";
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 65;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.FromArgb(10, 15, 25);

            rtbChat = new RichTextBox();
            rtbChat.Dock = DockStyle.Fill;
            rtbChat.ReadOnly = true;
            rtbChat.Font = new Font("Consolas", 10);
            rtbChat.BackColor = Color.FromArgb(25, 32, 45);
            rtbChat.ForeColor = Color.White;
            rtbChat.BorderStyle = BorderStyle.None;
            rtbChat.Padding = new Padding(10);

            bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 75;
            bottomPanel.Padding = new Padding(15);
            bottomPanel.BackColor = Color.FromArgb(10, 15, 25);

            txtInput = new TextBox();
            txtInput.Font = new Font("Segoe UI", 12);
            txtInput.Width = 680;
            txtInput.Height = 35;
            txtInput.Left = 15;
            txtInput.Top = 20;
            txtInput.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            txtInput.KeyDown += TxtInput_KeyDown;

            btnSend = new Button();
            btnSend.Text = "Send";
            btnSend.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnSend.Width = 130;
            btnSend.Height = 35;
            btnSend.Left = 720;
            btnSend.Top = 20;
            btnSend.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnSend.BackColor = Color.FromArgb(0, 120, 215);
            btnSend.ForeColor = Color.White;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Click += BtnSend_Click;

            bottomPanel.Controls.Add(txtInput);
            bottomPanel.Controls.Add(btnSend);

            Controls.Add(rtbChat);
            Controls.Add(bottomPanel);
            Controls.Add(lblTitle);
        }

        private void ShowWelcomeMessage()
        {
            string asciiArt =
@"   ____      _               ____        _   
  / ___|   _| |__   ___ _ __| __ )  ___ | |_ 
 | |  | | | | '_ \ / _ \ '__|  _ \ / _ \| __|
 | |__| |_| | |_) |  __/ |  | |_) | (_) | |_ 
  \____\__, |_.__/ \___|_|  |____/ \___/ \__|
       |___/                                  

Welcome to your Cybersecurity Awareness Chatbot.

You can ask me about:
- Password safety
- Phishing
- Scams
- Privacy
- Malware
- Public Wi-Fi
- VPNs
- Backups
- Software updates
- Social media safety

Try examples like:
- My name is Mivuyo
- I am worried about online scams
- Tell me about password safety
- Give me another tip
- I am interested in privacy
- What do you remember?
";

            AppendBotMessage(asciiArt);
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "welcome.wav");

                if (File.Exists(filePath))
                {
                    SoundPlayerHelper.PlayGreeting(filePath);
                }
                else
                {
                    AppendBotMessage("Voice greeting file was not found, so the chatbot will continue without audio.");
                }
            }
            catch
            {
                AppendBotMessage("The voice greeting could not be played, but the chatbot is still working.");
            }
        }

        private void BtnSend_Click(object? sender, EventArgs e)
        {
            SendMessage();
        }

        private void TxtInput_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.SuppressKeyPress = true;
            }
        }

        private void SendMessage()
        {
            string input = txtInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            AppendUserMessage(input);

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                AppendBotMessage($"Goodbye {_user.Name}! Stay safe online.");
                txtInput.Clear();
                return;
            }

            string response = _responses.GetResponse(input);
            _user.Interactions++;

            AppendBotMessage(response);

            txtInput.Clear();
            txtInput.Focus();
        }

        private void AppendUserMessage(string message)
        {
            rtbChat.SelectionColor = Color.LightSkyBlue;
            rtbChat.AppendText($"\nYou: {message}\n");
            rtbChat.SelectionColor = Color.White;
            rtbChat.ScrollToCaret();
        }

        private void AppendBotMessage(string message)
        {
            rtbChat.SelectionColor = Color.LightGreen;
            rtbChat.AppendText($"\nChatbot: {message}\n");
            rtbChat.SelectionColor = Color.White;
            rtbChat.ScrollToCaret();
        }
    }
}