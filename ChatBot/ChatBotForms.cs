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
        private Label lblSubtitle = null!;
        private Panel headerPanel = null!;
        private Panel bottomPanel = null!;
        private FlowLayoutPanel suggestionPanel = null!;

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
            Width = 950;
            Height = 700;
            MinimumSize = new Size(850, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(13, 18, 28);

            headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 105;
            headerPanel.BackColor = Color.FromArgb(7, 12, 22);
            headerPanel.Padding = new Padding(20, 10, 20, 10);

            lblTitle = new Label();
            lblTitle.Text = "🛡️ Cybersecurity Awareness Chatbot";
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 45;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            lblSubtitle = new Label();
            lblSubtitle.Text = "Learn about passwords, phishing, scams, privacy, malware, VPNs and online safety";
            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Height = 30;
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.FromArgb(170, 190, 210);

            suggestionPanel = new FlowLayoutPanel();
            suggestionPanel.Dock = DockStyle.Top;
            suggestionPanel.Height = 55;
            suggestionPanel.Padding = new Padding(15, 8, 15, 8);
            suggestionPanel.BackColor = Color.FromArgb(13, 18, 28);
            suggestionPanel.FlowDirection = FlowDirection.LeftToRight;
            suggestionPanel.WrapContents = false;
            suggestionPanel.AutoScroll = true;

            AddSuggestionButton("Password safety");
            AddSuggestionButton("Phishing tips");
            AddSuggestionButton("Online scams");
            AddSuggestionButton("Privacy");
            AddSuggestionButton("Malware");
            AddSuggestionButton("What do you remember?");

            rtbChat = new RichTextBox();
            rtbChat.Dock = DockStyle.Fill;
            rtbChat.ReadOnly = true;
            rtbChat.Font = new Font("Consolas", 10);
            rtbChat.BackColor = Color.FromArgb(20, 27, 40);
            rtbChat.ForeColor = Color.White;
            rtbChat.BorderStyle = BorderStyle.None;
            rtbChat.Margin = new Padding(15);
            rtbChat.Padding = new Padding(15);
            rtbChat.ScrollBars = RichTextBoxScrollBars.Vertical;

            bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 90;
            bottomPanel.Padding = new Padding(20);
            bottomPanel.BackColor = Color.FromArgb(7, 12, 22);

            txtInput = new TextBox();
            txtInput.Font = new Font("Segoe UI", 12);
            txtInput.BackColor = Color.FromArgb(245, 247, 250);
            txtInput.ForeColor = Color.FromArgb(20, 27, 40);
            txtInput.BorderStyle = BorderStyle.FixedSingle;
            txtInput.Left = 20;
            txtInput.Top = 25;
            txtInput.Width = 720;
            txtInput.Height = 38;
            txtInput.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            txtInput.Text = "Type your cybersecurity question here...";
            txtInput.ForeColor = Color.Gray;
            txtInput.Enter += TxtInput_Enter;
            txtInput.Leave += TxtInput_Leave;
            txtInput.KeyDown += TxtInput_KeyDown;

            btnSend = new Button();
            btnSend.Text = "Send ➜";
            btnSend.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnSend.Width = 140;
            btnSend.Height = 40;
            btnSend.Left = 760;
            btnSend.Top = 24;
            btnSend.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnSend.BackColor = Color.FromArgb(0, 150, 255);
            btnSend.ForeColor = Color.White;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Cursor = Cursors.Hand;
            btnSend.Click += BtnSend_Click;

            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);

            bottomPanel.Controls.Add(txtInput);
            bottomPanel.Controls.Add(btnSend);

            Controls.Add(rtbChat);
            Controls.Add(suggestionPanel);
            Controls.Add(bottomPanel);
            Controls.Add(headerPanel);
        }

        private void AddSuggestionButton(string text)
        {
            Button button = new Button();
            button.Text = text;
            button.Width = 135;
            button.Height = 35;
            button.Margin = new Padding(5);
            button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            button.BackColor = Color.FromArgb(35, 48, 68);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;

            button.Click += (sender, e) =>
            {
                txtInput.ForeColor = Color.FromArgb(20, 27, 40);

                if (text == "Password safety")
                    txtInput.Text = "Tell me about password safety";
                else if (text == "Phishing tips")
                    txtInput.Text = "Give me phishing tips";
                else if (text == "Online scams")
                    txtInput.Text = "I am worried about online scams";
                else if (text == "Privacy")
                    txtInput.Text = "I am interested in privacy";
                else if (text == "Malware")
                    txtInput.Text = "Tell me about malware";
                else
                    txtInput.Text = "What do you remember?";

                SendMessage();
            };

            suggestionPanel.Controls.Add(button);
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

I can help you learn about:
• Password safety
• Phishing
• Online scams
• Privacy
• Malware
• Public Wi-Fi
• VPNs
• Backups
• Software updates
• Social media safety
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
                    AppendSystemMessage("Voice greeting file was not found. Continuing without audio.");
                }
            }
            catch (Exception ex)
            {
                AppendSystemMessage("The voice greeting could not be played: " + ex.Message);
            }
        }

        private void TxtInput_Enter(object? sender, EventArgs e)
        {
            if (txtInput.Text == "Type your cybersecurity question here...")
            {
                txtInput.Text = "";
                txtInput.ForeColor = Color.FromArgb(20, 27, 40);
            }
        }

        private void TxtInput_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text))
            {
                txtInput.Text = "Type your cybersecurity question here...";
                txtInput.ForeColor = Color.Gray;
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

            if (string.IsNullOrWhiteSpace(input) ||
                input == "Type your cybersecurity question here...")
            {
                return;
            }

            AppendUserMessage(input);

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                AppendBotMessage($"Goodbye {_user.Name}! Stay safe online.");
                ClearInputBox();
                return;
            }

            string response = _responses.GetResponse(input);
            _user.Interactions++;

            AppendBotMessage(response);
            ClearInputBox();
        }

        private void ClearInputBox()
        {
            txtInput.Clear();
            txtInput.Focus();
        }

        private void AppendUserMessage(string message)
        {
            rtbChat.SelectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            rtbChat.SelectionColor = Color.FromArgb(80, 180, 255);
            rtbChat.AppendText("\nYou\n");

            rtbChat.SelectionFont = new Font("Segoe UI", 10, FontStyle.Regular);
            rtbChat.SelectionColor = Color.White;
            rtbChat.AppendText(message + "\n");

            rtbChat.ScrollToCaret();
        }

        private void AppendBotMessage(string message)
        {
            rtbChat.SelectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            rtbChat.SelectionColor = Color.FromArgb(90, 230, 160);
            rtbChat.AppendText("\nChatbot\n");

            rtbChat.SelectionFont = new Font("Segoe UI", 10, FontStyle.Regular);
            rtbChat.SelectionColor = Color.FromArgb(230, 235, 240);
            rtbChat.AppendText(message + "\n");

            rtbChat.ScrollToCaret();
        }

        private void AppendSystemMessage(string message)
        {
            rtbChat.SelectionFont = new Font("Segoe UI", 9, FontStyle.Italic);
            rtbChat.SelectionColor = Color.FromArgb(255, 190, 90);
            rtbChat.AppendText("\nSystem: " + message + "\n");
            rtbChat.ScrollToCaret();
        }
    }
}