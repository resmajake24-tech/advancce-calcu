using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdvanceCalculator
{
    public class Form1 : Form
    {
        TextBox display;
        ListBox history;

        double firstValue = 0;
        string op = "";
        bool newEntry = true;

        public Form1()
        {
            this.Text = "Scientific Calculator";
            this.Size = new Size(500, 760); // FIXED SIZE
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;

            BuildUI();
        }

        void BuildUI()
        {
            // ================= DISPLAY =================
            display = new TextBox();
            display.Location = new Point(10, 10);
            display.Size = new Size(460, 55);
            display.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            display.TextAlign = HorizontalAlignment.Right;
            display.ReadOnly = true;
            display.BorderStyle = BorderStyle.None;
            display.BackColor = Color.FromArgb(20, 20, 20);
            display.ForeColor = Color.White;

            this.Controls.Add(display);

            // ================= BUTTON GRID =================
            string[,] buttons =
            {
                { "sin","cos","tan","sqrt" },
                { "log","pi","C","⌫" },
                { "7","8","9","/" },
                { "4","5","6","*" },
                { "1","2","3","-" },
                { "0",".","=","+" }
            };

            int startX = 10;
            int startY = 80;

            int w = 110;
            int h = 70;
            int gap = 10;

            for (int r = 0; r < buttons.GetLength(0); r++)
            {
                for (int c = 0; c < buttons.GetLength(1); c++)
                {
                    string t = buttons[r, c];

                    Button btn = new Button();
                    btn.Text = t;
                    btn.Size = new Size(w, h);

                    btn.Location = new Point(
                        startX + c * (w + gap),
                        startY + r * (h + gap)
                    );

                    btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.ForeColor = Color.White;

                    // ================= COLORS =================
                    if ("+-*/=".Contains(t))
                        btn.BackColor = Color.Orange;

                    else if (t == "C")
                        btn.BackColor = Color.Red;

                    else if (t == "⌫")
                        btn.BackColor = Color.Gray;

                    else
                        btn.BackColor = Color.FromArgb(35, 35, 35);

                    btn.Click += ButtonClick;

                    this.Controls.Add(btn);
                }
            }

            // ================= HISTORY =================
            history = new ListBox();
            history.Location = new Point(10, 560); // MOVED DOWN
            history.Size = new Size(460, 140);
            history.Font = new Font("Consolas", 9);
            history.BackColor = Color.FromArgb(15, 15, 15);
            history.ForeColor = Color.LightGray;
            history.BorderStyle = BorderStyle.None;

            this.Controls.Add(history);
        }

        void ButtonClick(object sender, EventArgs e)
        {
            string t = ((Button)sender).Text;

            // ================= NUMBERS =================
            if (char.IsDigit(t, 0) || t == ".")
            {
                if (newEntry)
                {
                    display.Text = "";
                    newEntry = false;
                }

                display.Text += t;
                return;
            }

            // ================= CLEAR =================
            if (t == "C")
            {
                display.Clear();
                firstValue = 0;
                op = "";
                return;
            }

            // ================= BACKSPACE =================
            if (t == "⌫")
            {
                if (display.Text.Length > 0)
                    display.Text = display.Text.Substring(0, display.Text.Length - 1);

                return;
            }

            // ================= SCIENTIFIC =================
            if (t == "sin")
            {
                ApplyFunc(Math.Sin, "sin");
                return;
            }

            if (t == "cos")
            {
                ApplyFunc(Math.Cos, "cos");
                return;
            }

            if (t == "tan")
            {
                ApplyFunc(Math.Tan, "tan");
                return;
            }

            if (t == "sqrt")
            {
                ApplyFunc(Math.Sqrt, "sqrt");
                return;
            }

            if (t == "log")
            {
                ApplyFunc(Math.Log10, "log");
                return;
            }

            if (t == "pi")
            {
                display.Text = Math.PI.ToString();
                newEntry = true;
                return;
            }

            // ================= OPERATORS =================
            if (t == "+" || t == "-" || t == "*" || t == "/")
            {
                firstValue = GetValue();
                op = t;
                newEntry = true;
                return;
            }

            // ================= EQUALS =================
            if (t == "=")
            {
                double second = GetValue();
                double result = 0;

                switch (op)
                {
                    case "+":
                        result = firstValue + second;
                        break;

                    case "-":
                        result = firstValue - second;
                        break;

                    case "*":
                        result = firstValue * second;
                        break;

                    case "/":
                        result = second != 0 ? firstValue / second : 0;
                        break;
                }

                display.Text = result.ToString();

                history.Items.Add(
                    $"{firstValue} {op} {second} = {result}"
                );

                newEntry = true;
            }
        }

        void ApplyFunc(Func<double, double> func, string name)
        {
            double v = GetValue();
            double r = func(v);

            display.Text = r.ToString();

            history.Items.Add(
                $"{name}({v}) = {r}"
            );

            newEntry = true;
        }

        double GetValue()
        {
            double.TryParse(display.Text, out double v);
            return v;
        }
    }
}