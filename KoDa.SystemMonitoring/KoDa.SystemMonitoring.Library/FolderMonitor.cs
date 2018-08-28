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
                Thread.Sleep(10000);
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

            byte[] buffer = new byte[4096];

            using (var md5 = new MD5CryptoServiceProvider())
            {
                foreach (var file in Directory.EnumerateDirectories(_path, ".files", SearchOption.AllDirectories))
                {
                    int length;
                    using (var fs = File.OpenRead(file))
                    {
                        do
                        {
                            length = fs.Read(buffer, 0, buffer.Length);
                            md5.TransformBlock(buffer, 0, length, buffer, 0);
                        } while (length > 0);
                    }
                }
                md5.TransformFinalBlock(buffer, 0, 0);
                Console.WriteLine(BitConverter.ToString(md5.Hash));
            }

         
        }
        }

    }

