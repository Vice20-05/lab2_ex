using System;
using System.Threading;

namespace lab2
{
    public class MyThread
    {
        private readonly Form1 form;
        private readonly Action formMethod;

        private Thread thread;
        private volatile bool running;

        // Option 1: store a reference to Form1.
        public MyThread(Form1 form)
        {
            this.form = form;
        }

        // Option 2: store a delegate to a Form1 method.
        public MyThread(Action formMethod)
        {
            this.formMethod = formMethod;
        }

        public void Start()
        {
            if (thread != null && thread.IsAlive)
                return;

            running = true;
            thread = new Thread(Run);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Run()
        {
            while (running)
            {
                if (form != null)
                    form.MoveTextOneStep(); // Form1 reference variant
                else
                    formMethod();           // Delegate variant

                Thread.Sleep(100);
            }
        }

        public void Stop()
        {
            running = false;

            if (thread != null && thread.IsAlive)
                thread.Join();

            thread = null;
        }
    }
}
