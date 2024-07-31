using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static NexusShadow.Core.Subcore.Logging;

namespace NexusShadow.Core.Subcore
{
    public static class FileSystem
    {
        public static bool CreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error creating directory: {e.Message}");
                return false;
            }
        }

        public static bool DeleteDirectory(string path, bool recursive = false)
        {
            try
            {
                Directory.Delete(path, recursive);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error deleting directory: {e.Message}");
                return false;
            }
        }

        public static bool CopyFile(string sourcePath, string destinationPath)
        {
            try
            {
                File.Copy(sourcePath, destinationPath, true);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error copying file: {e.Message}");
                return false;
            }
        }

        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error deleting file: {e.Message}");
                return false;
            }
        }

        public static string ReadFileContent(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                LogError($"Error reading file: {e.Message}");
                return null;
            }
        }

        public static bool WriteFileContent(string path, string content)
        {
            try
            {
                File.WriteAllText(path, content);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error writing file: {e.Message}");
                return false;
            }
        }

        public static bool MoveFile(string sourcePath, string destinationPath)
        {
            try
            {
                File.Move(sourcePath, destinationPath);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error moving file: {e.Message}");
                return false;
            }
        }

        public static bool MoveDirectory(string sourcePath, string destinationPath)
        {
            try
            {
                Directory.Move(sourcePath, destinationPath);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error moving directory: {e.Message}");
                return false;
            }
        }

        public static bool FileExists(string path)
        {
            try
            {
                return File.Exists(path);
            }
            catch (Exception e)
            {
                LogError($"Error checking file existence: {e.Message}");
                return false;
            }
        }

        public static bool DirectoryExists(string path)
        {
            try
            {
                return Directory.Exists(path);
            }
            catch (Exception e)
            {
                LogError($"Error checking directory existence: {e.Message}");
                return false;
            }
        }

        public static string[] GetFiles(string path)
        {
            try
            {
                return Directory.GetFiles(path);
            }
            catch (Exception e)
            {
                LogError($"Error getting files in directory: {e.Message}");
                return null;
            }
        }

        public static string[] GetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch (Exception e)
            {
                LogError($"Error getting directories in directory: {e.Message}");
                return null;
            }
        }

        public static IEnumerable<string> ReadFileLines(string path)
        {
            try
            {
                return File.ReadLines(path);
            }
            catch (Exception e)
            {
                LogError($"Error reading file lines: {e.Message}");
                return null;
            }
        }

        public static bool WriteFileLines(string path, IEnumerable<string> lines)
        {
            try
            {
                File.WriteAllLines(path, lines);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error writing file lines: {e.Message}");
                return false;
            }
        }

        public static long GetFileSize(string path)
        {
            try
            {
                return new FileInfo(path).Length;
            }
            catch (Exception e)
            {
                LogError($"Error getting file size: {e.Message}");
                return -1;
            }
        }

        public static long GetDirectorySize(string path)
        {
            try
            {
                return Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                                .Sum(file => new FileInfo(file).Length);
            }
            catch (Exception e)
            {
                LogError($"Error getting directory size: {e.Message}");
                return -1;
            }
        }

        public static bool ClearDirectory(string path)
        {
            try
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }
                foreach (var dir in Directory.GetDirectories(path))
                {
                    Directory.Delete(dir, true);
                }
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error clearing directory: {e.Message}");
                return false;
            }
        }

        public static bool CopyDirectory(string sourcePath, string destinationPath)
        {
            try
            {
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                foreach (string file in Directory.GetFiles(sourcePath))
                {
                    string destFile = Path.Combine(destinationPath, Path.GetFileName(file));
                    File.Copy(file, destFile, true);
                }

                foreach (string subDir in Directory.GetDirectories(sourcePath))
                {
                    string destSubDir = Path.Combine(destinationPath, Path.GetFileName(subDir));
                    CopyDirectory(subDir, destSubDir);
                }

                return true;
            }
            catch (Exception e)
            {
                LogError($"Error copying directory: {e.Message}");
                return false;
            }
        }

        public static bool RenameFile(string path, string newName)
        {
            try
            {
                string newPath = Path.Combine(Path.GetDirectoryName(path), newName);
                File.Move(path, newPath);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error renaming file: {e.Message}");
                return false;
            }
        }

        public static bool RenameDirectory(string path, string newName)
        {
            try
            {
                string newPath = Path.Combine(Path.GetDirectoryName(path), newName);
                Directory.Move(path, newPath);
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error renaming directory: {e.Message}");
                return false;
            }
        }
    }
}