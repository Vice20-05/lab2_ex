using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


/*
 * Exercitii lab1 , lab 2 , sok de rovid
 * lab1 kulon project es kulon git a lab2 tol 
 * 
 */

namespace lab2
{
    public partial class Form1 : Form
    {
        private MyThread worker;
        private int elapsedTime;
        int timer_elapsed;
        private int textX = 10;

        private TextWriterTraceListener traceListener;

        private BooleanSwitch movementLogSwitch;
        public Form1()
        {
            InitializeComponent();
            up.Click += button1_Click;
            down.Click += down_Click;
            left.Click += button3_Click;
            right.Click += button4_Click;
            start.Click += Start_Click;
            Stop.Click += Stop_Click;
            this.Paint += Foral_Paint;

            movementLogSwitch = new BooleanSwitch("MovementLogSwitch", "Enable or Disable movement logging");
            // Initialize trace listener for movement logging
            traceListener = new TextWriterTraceListener("movementLog.txt");
            Trace.Listeners.Add(traceListener);
            Trace.AutoFlush = true;

            // Create worker thread helper (pass Form1 reference so it can call MoveTextOneStep)
            worker = new MyThread(this);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //up
            textBox1.Text="up";
            this.Top -= 20;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //left
            textBox1.Text = "left";
            this.Left -= 20; 
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //right
            textBox1.Text = "right";
            this.Left += 20;
        }
        
        private void down_Click(object sender, EventArgs e)
        {
            //down
            textBox1.Text = "down";
            this.Top += 20;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            // Start the background worker that moves the text
            worker?.Start();
        }

     
        // The Run loop has been moved to MyThread. Use MoveTextOneStep to update UI from that worker.

        public void MoveTextOneStep()
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke((Action)MoveTextOneStep);
                }
                catch (InvalidOperationException)
                {
                    // The form is closing.
                }

                return;
            }

            // Update UI state
            timer_show.Text = (timer_elapsed / 1000).ToString("0.0");
            BackColor = BackColor == Color.Red ? Color.Blue : Color.Red;

            textX += 5;
            if (textX > ClientSize.Width)
            {
                textX = -100;
                Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - Text moved off screen");
            }

            if (movementLogSwitch.Enabled)
            {
                Trace.WriteLine(DateTime.Now.ToString("yy yy-MM-dd HH:mm:ss") + " - Text position: " + textX);
            }

            Invalidate();
        }

        //Stop the worker thread when the form is closing
        private void StopThread()
        {
            try
            {
                worker?.Stop();
            }
            catch
            {
                // ignore
            }
            worker = null;
        }

        private void Foral_Paint(object sender, PaintEventArgs e)
        {
            using (Font font = new Font("Arial", 16))
            {
                e.Graphics.DrawString("Hello, World!", font, Brushes.Black, textX, 50);
            }
        }

        // Timer-based movement removed; MyThread.MoveTextOneStep handles movement now.
        private void Stop_Click(object sender, EventArgs e)
        {
            // Stop background worker
            worker?.Stop();
        }

        private void timer_show_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
