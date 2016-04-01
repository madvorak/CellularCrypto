using System;

namespace UserForms
{
    /// <summary>
    /// Static class that allows to display an interactive window from a console application.
    /// </summary>
    static class Adapter
    {
        public static void DisplayForm()
        {
            Console.WriteLine("Do you want to show an interactive window? Type Y to show it.");
            if (Console.ReadLine() == "Y")
            {
                var form = new DemoForm();
                form.ShowDialog();
            }
            else
            {
                Console.WriteLine("OK, continue normally...\n");
            }
        }
    }
}