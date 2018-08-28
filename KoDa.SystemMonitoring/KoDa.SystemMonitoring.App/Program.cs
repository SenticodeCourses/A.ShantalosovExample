using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KoDa.SystemMonitoring.Library;

namespace KoDa.SystemMonitoring.App
{
    class Program
    {
        static void Main(string[] args)
        {

            FolderMonitor dev1 = new FolderMonitor(@"D:\\Lesha\\c#");
             dev1._isMonitoring = true;
            dev1.Monitoring();
        
            Console.ReadLine();

        }
    }
}
