using System;


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
            Console.WriteLine("OK, continuing normally");
        }
    }
}
