namespace MediaSync
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Global exception handlers
            Application.ThreadException += (sender, e) =>
            {
                MessageBox.Show("Unhandled thread exception: " + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.Show("Unhandled exception: " + ((Exception)e.ExceptionObject).Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            Application.Run(new Form1());
        }
    }
}