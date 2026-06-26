using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Chatbot
{
    public class ChatbotForm : Form
    {
        private readonly UserProfile _user;
        private readonly ChatBotResponses _responses;

        private readonly TaskService _taskService = new TaskService();
        private readonly ActivityLogService _activityLog = new ActivityLogService();
        private readonly QuizService _quizService = new QuizService();

        private int _currentQuestionIndex = 0;
        private int _score = 0;
        private bool _quizStarted = false;

        private RichTextBox rtbChat = null!;
        private TextBox txtInput = null!;
        private Button btnSend = null!;
        private Label lblTitle = null!;
        private Label lblSubtitle = null!;
        private Panel headerPanel = null!;
        private Panel bottomPanel = null!;
        private FlowLayoutPanel suggestionPanel = null!;
        private TabControl tabControl = null!;

        private TextBox txtTaskTitle = null!;
        private TextBox txtTaskDescription = null!;
        private DateTimePicker dtpReminder = null!;
        private CheckBox chkReminder = null!;
        private Button btnAddTask = null!;
        private Button btnCompleteTask = null!;
        private Button btnDeleteTask = null!;
        private Button btnRefreshTasks = null!;
        private DataGridView dgvTasks = null!;

        private Label lblQuestion = null!;
        private RadioButton rbOptionA = null!;
        private RadioButton rbOptionB = null!;
        private RadioButton rbOptionC = null!;
        private RadioButton rbOptionD = null!;
        private Button btnStartQuiz = null!;
        private Button btnSubmitAnswer = null!;
        private Label lblScore = null!;
        private Label lblFeedback = null!;

        private ListBox lstActivityLog = null!;
        private Button btnShowLog = null!;

        public ChatbotForm()
        {
            _user = new UserProfile();
            _responses = new ChatBotResponses(_user);

            BuildInterface();
            ShowWelcomeMessage();
            PlayVoiceGreeting();
            LoadTasksIntoGrid();
            RefreshActivityLog();
        }

        private void BuildInterface()
        {
            Text = "Cybersecurity Awareness Chatbot";
            Width = 1100;
            Height = 780;
            MinimumSize = new Size(950, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(13, 18, 28);

            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 105,
                BackColor = Color.FromArgb(7, 12, 22),
                Padding = new Padding(20, 10, 20, 10)
            };

            lblTitle = new Label
            {
                Text = "🛡️ Cybersecurity Awareness Chatbot",
                Dock = DockStyle.Top,
                Height = 45,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White
            };

            lblSubtitle = new Label
            {
                Text = "Passwords • Phishing • Privacy • Tasks • Reminders • Quiz • Activity Log",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(170, 190, 210)
            };

            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);

            suggestionPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 55,
                Padding = new Padding(15, 8, 15, 8),
                BackColor = Color.FromArgb(13, 18, 28),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true
            };

            AddSuggestionButton("Password safety");
            AddSuggestionButton("Phishing tips");
            AddSuggestionButton("Online scams");
            AddSuggestionButton("Privacy");
            AddSuggestionButton("Malware");
            AddSuggestionButton("Start quiz");
            AddSuggestionButton("Show activity log");

            bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 90,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(7, 12, 22)
            };

            txtInput = new TextBox
            {
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(245, 247, 250),
                ForeColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle,
                Left = 20,
                Top = 25,
                Width = 820,
                Height = 38,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                Text = "Type your cybersecurity question here..."
            };

            txtInput.Enter += TxtInput_Enter;
            txtInput.Leave += TxtInput_Leave;
            txtInput.KeyDown += TxtInput_KeyDown;

            btnSend = new Button
            {
                Text = "Send ➜",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Width = 150,
                Height = 40,
                Left = 870,
                Top = 24,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                BackColor = Color.FromArgb(0, 150, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += BtnSend_Click;

            bottomPanel.Controls.Add(txtInput);
            bottomPanel.Controls.Add(btnSend);

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            BuildChatTab();
            BuildTasksTab();
            BuildQuizTab();
            BuildActivityLogTab();

            Controls.Add(tabControl);
            Controls.Add(suggestionPanel);
            Controls.Add(bottomPanel);
            Controls.Add(headerPanel);
        }

        private void BuildChatTab()
        {
            TabPage chatTab = new TabPage("Chatbot")
            {
                BackColor = Color.FromArgb(20, 27, 40)
            };

            rtbChat = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 10),
                BackColor = Color.FromArgb(20, 27, 40),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            chatTab.Controls.Add(rtbChat);
            tabControl.Controls.Add(chatTab);
        }

        private void BuildTasksTab()
        {
            TabPage tasksTab = new TabPage("Task Assistant")
            {
                BackColor = Color.FromArgb(20, 27, 40)
            };

            Label lblTaskHeader = new Label
            {
                Text = "Cybersecurity Task Assistant with Reminders",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Left = 20,
                Top = 20,
                Width = 700,
                Height = 35
            };

            Label lblTaskTitle = new Label
            {
                Text = "Task Title:",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 75,
                Width = 120
            };

            txtTaskTitle = new TextBox
            {
                Left = 150,
                Top = 72,
                Width = 350,
                Font = new Font("Segoe UI", 10)
            };

            Label lblTaskDescription = new Label
            {
                Text = "Description:",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 20,
                Top = 115,
                Width = 120
            };

            txtTaskDescription = new TextBox
            {
                Left = 150,
                Top = 112,
                Width = 350,
                Height = 70,
                Multiline = true,
                Font = new Font("Segoe UI", 10)
            };

            chkReminder = new CheckBox
            {
                Text = "Set Reminder",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 530,
                Top = 75,
                Width = 150
            };

            dtpReminder = new DateTimePicker
            {
                Left = 530,
                Top = 110,
                Width = 250,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm",
                Font = new Font("Segoe UI", 10)
            };

            btnAddTask = CreateStyledButton("Add Task", 820, 72, Color.FromArgb(0, 150, 255));
            btnCompleteTask = CreateStyledButton("Mark Complete", 820, 122, Color.FromArgb(60, 180, 120));
            btnDeleteTask = CreateStyledButton("Delete Task", 820, 172, Color.FromArgb(220, 70, 70));
            btnRefreshTasks = CreateStyledButton("Refresh", 820, 222, Color.FromArgb(120, 120, 220));

            btnAddTask.Click += BtnAddTask_Click;
            btnCompleteTask.Click += BtnCompleteTask_Click;
            btnDeleteTask.Click += BtnDeleteTask_Click;
            btnRefreshTasks.Click += (s, e) => LoadTasksIntoGrid();

            dgvTasks = new DataGridView
            {
                Left = 20,
                Top = 270,
                Width = 1000,
                Height = 330,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                BackgroundColor = Color.FromArgb(30, 40, 58),
                ForeColor = Color.Black,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };

            tasksTab.Controls.Add(lblTaskHeader);
            tasksTab.Controls.Add(lblTaskTitle);
            tasksTab.Controls.Add(txtTaskTitle);
            tasksTab.Controls.Add(lblTaskDescription);
            tasksTab.Controls.Add(txtTaskDescription);
            tasksTab.Controls.Add(chkReminder);
            tasksTab.Controls.Add(dtpReminder);
            tasksTab.Controls.Add(btnAddTask);
            tasksTab.Controls.Add(btnCompleteTask);
            tasksTab.Controls.Add(btnDeleteTask);
            tasksTab.Controls.Add(btnRefreshTasks);
            tasksTab.Controls.Add(dgvTasks);

            tabControl.Controls.Add(tasksTab);
        }

        private void BuildQuizTab()
        {
            TabPage quizTab = new TabPage("Mini Quiz")
            {
                BackColor = Color.FromArgb(20, 27, 40)
            };

            Label lblQuizHeader = new Label
            {
                Text = "Cybersecurity Mini-Game Quiz",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Left = 20,
                Top = 20,
                Width = 700,
                Height = 35
            };

            lblScore = new Label
            {
                Text = "Score: 0",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 230, 160),
                Left = 20,
                Top = 65,
                Width = 250,
                Height = 30
            };

            lblQuestion = new Label
            {
                Text = "Click Start Quiz to begin.",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                Left = 20,
                Top = 115,
                Width = 950,
                Height = 80
            };

            rbOptionA = CreateQuizRadioButton("A", 40, 220);
            rbOptionB = CreateQuizRadioButton("B", 40, 270);
            rbOptionC = CreateQuizRadioButton("C", 40, 320);
            rbOptionD = CreateQuizRadioButton("D", 40, 370);

            btnStartQuiz = CreateStyledButton("Start Quiz", 20, 440, Color.FromArgb(0, 150, 255));
            btnSubmitAnswer = CreateStyledButton("Submit Answer", 170, 440, Color.FromArgb(60, 180, 120));

            btnStartQuiz.Click += (s, e) => StartQuiz();
            btnSubmitAnswer.Click += BtnSubmitAnswer_Click;

            lblFeedback = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 210, 100),
                Left = 20,
                Top = 510,
                Width = 950,
                Height = 80
            };

            quizTab.Controls.Add(lblQuizHeader);
            quizTab.Controls.Add(lblScore);
            quizTab.Controls.Add(lblQuestion);
            quizTab.Controls.Add(rbOptionA);
            quizTab.Controls.Add(rbOptionB);
            quizTab.Controls.Add(rbOptionC);
            quizTab.Controls.Add(rbOptionD);
            quizTab.Controls.Add(btnStartQuiz);
            quizTab.Controls.Add(btnSubmitAnswer);
            quizTab.Controls.Add(lblFeedback);

            tabControl.Controls.Add(quizTab);
        }

        private void BuildActivityLogTab()
        {
            TabPage logTab = new TabPage("Activity Log")
            {
                BackColor = Color.FromArgb(20, 27, 40)
            };

            Label lblLogHeader = new Label
            {
                Text = "Activity Log - Recent Chatbot Actions",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Left = 20,
                Top = 20,
                Width = 700,
                Height = 35
            };

            btnShowLog = CreateStyledButton("Refresh Log", 20, 70, Color.FromArgb(0, 150, 255));
            btnShowLog.Click += (s, e) => RefreshActivityLog();

            lstActivityLog = new ListBox
            {
                Left = 20,
                Top = 130,
                Width = 1000,
                Height = 450,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                BackColor = Color.FromArgb(30, 40, 58),
                ForeColor = Color.White,
                Font = new Font("Consolas", 10)
            };

            logTab.Controls.Add(lblLogHeader);
            logTab.Controls.Add(btnShowLog);
            logTab.Controls.Add(lstActivityLog);

            tabControl.Controls.Add(logTab);
        }

        private Button CreateStyledButton(string text, int left, int top, Color backColor)
        {
            Button button = new Button
            {
                Text = text,
                Left = left,
                Top = top,
                Width = 130,
                Height = 38,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private RadioButton CreateQuizRadioButton(string text, int left, int top)
        {
            return new RadioButton
            {
                Text = text,
                Left = left,
                Top = top,
                Width = 900,
                Height = 35,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(20, 27, 40)
            };
        }

        private void AddSuggestionButton(string text)
        {
            Button button = new Button
            {
                Text = text,
                Width = 145,
                Height = 35,
                Margin = new Padding(5),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(35, 48, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;

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
                else if (text == "Start quiz")
                    txtInput.Text = "Start quiz";
                else if (text == "Show activity log")
                    txtInput.Text = "Show activity log";
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

Part 3 features added:
• Task assistant with reminders
• MySQL task storage
• Cybersecurity quiz mini-game
• NLP keyword command detection
• Activity log

Try typing:
• Add task - Review privacy settings
• Remind me to update my password tomorrow
• Start quiz
• Show activity log
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

            string lowerInput = input.ToLower();

            if (_quizStarted)
            {
                HandleQuizAnswer(lowerInput);
                ClearInputBox();
                return;
            }

            if (lowerInput.Contains("start quiz") ||
                lowerInput.Contains("quiz") ||
                lowerInput.Contains("mini game") ||
                lowerInput.Contains("game"))
            {
                StartQuiz();
                tabControl.SelectedIndex = 2;
            }
            else if (lowerInput.Contains("show activity") ||
                     lowerInput.Contains("activity log") ||
                     lowerInput.Contains("what have you done"))
            {
                ShowActivityLogInChat();
                tabControl.SelectedIndex = 3;
            }
            else if (lowerInput.Contains("add task") ||
                     lowerInput.Contains("remind me") ||
                     lowerInput.Contains("set reminder") ||
                     lowerInput.Contains("task"))
            {
                AddTaskFromChat(input);
                LoadTasksIntoGrid();
                tabControl.SelectedIndex = 1;
            }
            else
            {
                string response = _responses.GetResponse(input);
                _user.Interactions++;

                _activityLog.AddLog("NLP response given for: " + input);
                RefreshActivityLog();

                AppendBotMessage(response);
            }

            ClearInputBox();
        }

        private void AddTaskFromChat(string input)
        {
            string title = input
                .Replace("Add task", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Add a task", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Remind me to", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Set reminder to", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Set a reminder to", "", StringComparison.OrdinalIgnoreCase)
                .Replace("-", "", StringComparison.OrdinalIgnoreCase)
                .Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                title = "Cybersecurity task";
            }

            string description = $"Cybersecurity task: {title}";
            DateTime? reminder = DetectReminderDate(input);

            try
            {
                _taskService.AddTask(title, description, reminder);
                _activityLog.AddLog("Task added: " + title);

                if (reminder.HasValue)
                {
                    _activityLog.AddLog("Reminder set for task: " + title);
                    AppendBotMessage($"Task added: '{title}'. Reminder set for {reminder.Value:yyyy-MM-dd HH:mm}.");
                }
                else
                {
                    AppendBotMessage($"Task added: '{title}'. No reminder was set.");
                }

                RefreshActivityLog();
            }
            catch (Exception ex)
            {
                AppendSystemMessage("Task could not be saved to the database: " + ex.Message);
            }
        }

        private DateTime? DetectReminderDate(string input)
        {
            string lowerInput = input.ToLower();

            if (lowerInput.Contains("tomorrow"))
                return DateTime.Now.AddDays(1);

            if (lowerInput.Contains("in 3 days"))
                return DateTime.Now.AddDays(3);

            if (lowerInput.Contains("in 5 days"))
                return DateTime.Now.AddDays(5);

            if (lowerInput.Contains("in 7 days") || lowerInput.Contains("next week"))
                return DateTime.Now.AddDays(7);

            if (lowerInput.Contains("today"))
                return DateTime.Now;

            return null;
        }

        private void BtnAddTask_Click(object? sender, EventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();
            string description = txtTaskDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a task title.", "Missing Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                description = "Cybersecurity task: " + title;
            }

            DateTime? reminder = chkReminder.Checked ? dtpReminder.Value : null;

            try
            {
                _taskService.AddTask(title, description, reminder);
                _activityLog.AddLog("Task added from GUI: " + title);

                if (reminder.HasValue)
                    _activityLog.AddLog("Reminder set from GUI for: " + title);

                txtTaskTitle.Clear();
                txtTaskDescription.Clear();
                chkReminder.Checked = false;

                LoadTasksIntoGrid();
                RefreshActivityLog();

                AppendBotMessage($"Task added from GUI: '{title}'.");

                MessageBox.Show("Task added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Task could not be saved: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCompleteTask_Click(object? sender, EventArgs e)
        {
            int? taskId = GetSelectedTaskId();

            if (taskId == null)
            {
                MessageBox.Show("Please select a task to complete.", "No Task Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _taskService.CompleteTask(taskId.Value);
                _activityLog.AddLog("Task marked as completed. Task ID: " + taskId.Value);

                LoadTasksIntoGrid();
                RefreshActivityLog();

                AppendBotMessage("Selected task marked as completed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Task could not be completed: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeleteTask_Click(object? sender, EventArgs e)
        {
            int? taskId = GetSelectedTaskId();

            if (taskId == null)
            {
                MessageBox.Show("Please select a task to delete.", "No Task Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this task?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                _taskService.DeleteTask(taskId.Value);
                _activityLog.AddLog("Task deleted. Task ID: " + taskId.Value);

                LoadTasksIntoGrid();
                RefreshActivityLog();

                AppendBotMessage("Selected task deleted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Task could not be deleted: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int? GetSelectedTaskId()
        {
            if (dgvTasks.SelectedRows.Count == 0)
                return null;

            object value = dgvTasks.SelectedRows[0].Cells["TaskID"].Value;

            if (value == null)
                return null;

            return Convert.ToInt32(value);
        }

        private void LoadTasksIntoGrid()
        {
            try
            {
                dgvTasks.DataSource = null;
                dgvTasks.DataSource = _taskService.GetTasks();

                if (dgvTasks.Columns["TaskID"] != null)
                    dgvTasks.Columns["TaskID"].HeaderText = "ID";

                if (dgvTasks.Columns["ReminderDate"] != null)
                    dgvTasks.Columns["ReminderDate"].HeaderText = "Reminder";

                if (dgvTasks.Columns["IsCompleted"] != null)
                    dgvTasks.Columns["IsCompleted"].HeaderText = "Completed";
            }
            catch (Exception ex)
            {
                AppendSystemMessage("Tasks could not be loaded: " + ex.Message);
            }
        }

        private void StartQuiz()
        {
            _quizStarted = true;
            _currentQuestionIndex = 0;
            _score = 0;

            _activityLog.AddLog("Quiz started");
            RefreshActivityLog();

            lblFeedback.Text = "";
            lblScore.Text = "Score: 0";

            AppendBotMessage("Quiz started. Answer using A, B, C, or D.");
            ShowCurrentQuestion();
        }

        private void ShowCurrentQuestion()
        {
            ClearQuizOptions();

            if (_currentQuestionIndex >= _quizService.Questions.Count)
            {
                EndQuiz();
                return;
            }

            QuizQuestion question = _quizService.Questions[_currentQuestionIndex];

            lblQuestion.Text = $"Question {_currentQuestionIndex + 1}/{_quizService.Questions.Count}: {question.Question}";

            rbOptionA.Text = question.Options.Length > 0 ? "A) " + question.Options[0] : "";
            rbOptionB.Text = question.Options.Length > 1 ? "B) " + question.Options[1] : "";
            rbOptionC.Text = question.Options.Length > 2 ? "C) " + question.Options[2] : "";
            rbOptionD.Text = question.Options.Length > 3 ? "D) " + question.Options[3] : "";

            rbOptionA.Visible = question.Options.Length > 0;
            rbOptionB.Visible = question.Options.Length > 1;
            rbOptionC.Visible = question.Options.Length > 2;
            rbOptionD.Visible = question.Options.Length > 3;

            string message = $"Question {_currentQuestionIndex + 1}/{_quizService.Questions.Count}\n";
            message += question.Question + "\n\n";

            for (int i = 0; i < question.Options.Length; i++)
            {
                char optionLetter = (char)('A' + i);
                message += $"{optionLetter}) {question.Options[i]}\n";
            }

            message += "\nType A, B, C, or D, or select an answer in the Quiz tab.";

            AppendBotMessage(message);
        }

        private void BtnSubmitAnswer_Click(object? sender, EventArgs e)
        {
            if (!_quizStarted)
            {
                MessageBox.Show("Please start the quiz first.", "Quiz Not Started", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int selectedIndex = GetSelectedQuizOption();

            if (selectedIndex == -1)
            {
                MessageBox.Show("Please select an answer.", "No Answer Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProcessQuizAnswer(selectedIndex);
        }

        private int GetSelectedQuizOption()
        {
            if (rbOptionA.Checked) return 0;
            if (rbOptionB.Checked) return 1;
            if (rbOptionC.Checked) return 2;
            if (rbOptionD.Checked) return 3;

            return -1;
        }

        private void HandleQuizAnswer(string input)
        {
            int selectedIndex = -1;

            if (input == "a") selectedIndex = 0;
            else if (input == "b") selectedIndex = 1;
            else if (input == "c") selectedIndex = 2;
            else if (input == "d") selectedIndex = 3;
            else
            {
                AppendBotMessage("Please answer using A, B, C, or D.");
                return;
            }

            ProcessQuizAnswer(selectedIndex);
        }

        private void ProcessQuizAnswer(int selectedIndex)
        {
            QuizQuestion question = _quizService.Questions[_currentQuestionIndex];

            if (selectedIndex == question.CorrectIndex)
            {
                _score++;
                lblFeedback.Text = "Correct! " + question.Explanation;
                AppendBotMessage("Correct! " + question.Explanation);
            }
            else
            {
                char correctLetter = (char)('A' + question.CorrectIndex);
                lblFeedback.Text = $"Incorrect. Correct answer: {correctLetter}. {question.Explanation}";
                AppendBotMessage($"Incorrect. Correct answer: {correctLetter}. {question.Explanation}");
            }

            lblScore.Text = $"Score: {_score}/{_quizService.Questions.Count}";

            _currentQuestionIndex++;
            ShowCurrentQuestion();
        }

        private void EndQuiz()
        {
            _quizStarted = false;

            string finalMessage = $"Quiz completed!\nYour final score is {_score}/{_quizService.Questions.Count}.\n\n";

            if (_score >= 8)
                finalMessage += "Great job! You are a cybersecurity pro!";
            else
                finalMessage += "Keep learning to stay safe online!";

            lblQuestion.Text = "Quiz completed.";
            lblFeedback.Text = finalMessage;
            lblScore.Text = $"Final Score: {_score}/{_quizService.Questions.Count}";

            _activityLog.AddLog($"Quiz completed with score {_score}/{_quizService.Questions.Count}");
            RefreshActivityLog();

            AppendBotMessage(finalMessage);
        }

        private void ClearQuizOptions()
        {
            rbOptionA.Checked = false;
            rbOptionB.Checked = false;
            rbOptionC.Checked = false;
            rbOptionD.Checked = false;
        }

        private void ShowActivityLogInChat()
        {
            var logs = _activityLog.GetRecentLogs();

            if (logs.Count == 0)
            {
                AppendBotMessage("No recent activity yet.");
                return;
            }

            string message = "Here is a summary of recent actions:\n\n";

            foreach (string log in logs)
            {
                message += "• " + log + "\n";
            }

            AppendBotMessage(message);
            RefreshActivityLog();
        }

        private void RefreshActivityLog()
        {
            if (lstActivityLog == null)
                return;

            lstActivityLog.Items.Clear();

            var logs = _activityLog.GetRecentLogs();

            if (logs.Count == 0)
            {
                lstActivityLog.Items.Add("No recent activity yet.");
                return;
            }

            foreach (string log in logs)
            {
                lstActivityLog.Items.Add(log);
            }
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