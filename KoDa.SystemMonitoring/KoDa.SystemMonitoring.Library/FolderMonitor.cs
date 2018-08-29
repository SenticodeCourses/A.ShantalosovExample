using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Security.Cryptography;

namespace KoDa.SystemMonitoring.Library
{
    public class FolderMonitor
    {

        private string _path;
        public bool _isMonitoring;
        private Dictionary<string, bool> _previousFolderState = new Dictionary<string, bool>();

        public FolderMonitor(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException();
            }
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }
            _path = path;
            _previousFolderState = GetFolderState();
        }

        public void Monitoring()
        {
            while (_isMonitoring)
            {
                TrackChanges();
                GetHash();
                Thread.Sleep(15000);
                Console.WriteLine("\n10 second\n");
                
            }
        }

        public void TrackChanges()
        {
            var _latestFolderState = GetFolderState();
            var _deleteFolder = CheckChanges(_previousFolderState, _latestFolderState);
            var _addedFoulder = CheckChanges(_latestFolderState, _previousFolderState);

            foreach (var s in _deleteFolder)
            {
                Console.WriteLine($"the foulder {s} was deleted! ");
            }

            foreach (var s in _addedFoulder)
            {
                Console.WriteLine($"the foulder {s} was added! ");
            }
            _previousFolderState = _latestFolderState;

        }

        public Dictionary<string, bool> GetFolderState()
        {
            return Directory.EnumerateDirectories(_path)
                .ToDictionary(i => i, i => false);
          
        }

        public List<string> CheckChanges(Dictionary<string, bool> dict1, Dictionary<string, bool> dict2)
        {
            List<string> lst = new List<string>();
            foreach (var s in dict1)
            {
                if (!dict2.TryGetValue(s.Key, out var folderName))
                {
                    lst.Add(s.Key.ToString());
                }
            }
            return lst;
        }
        
            public void GetHash()
            {

            var md5 = System.Security.Cryptography.MD5.Create();

            foreach (var d1 in new System.IO.DirectoryInfo(_path).GetDirectories())
            {
               
                    foreach (var d2 in d1.GetFiles())
                    {
                        Console.WriteLine("MD5: {0} - FileName: {1}",
                            BitConverter.ToString(md5.ComputeHash(d2.OpenRead()))
                                .Replace("-", string.Empty),
                            d2.Name);
                    }

                    foreach (var d3 in d1.GetDirectories())
                {
                    foreach (var d4 in d3.GetFiles())
                    {
                        Console.WriteLine("MD5: {0} - FileName: {1}",
                            BitConverter.ToString(md5.ComputeHash(d4.OpenRead()))
                                .Replace("-", string.Empty),
                            d4.Name);
                    }
                }
                }
           
        }


    }
        }

    

