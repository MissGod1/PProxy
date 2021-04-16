using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PProxy.Controller
{
    class FileController
    {
        /// <summary>
        /// 获取指定路径下的文件列表
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="ext">文件后缀名</param>
        /// <returns>文件列表</returns>
        public string[] GetFileList(string path, string ext)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception($"{path} is not exists.");
            }

            List<string> files = new List<string>();
            foreach (var file in Directory.GetFiles(path, $"*.{ext}"))
            {
                files.Add(Path.GetFileNameWithoutExtension(file));
            }
            return files.ToArray();
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>文件内容</returns>
        public string GetFileContent(string file)
        {
            if (!File.Exists(file))
            {
                throw new Exception($"{file} is not exists.");
            }
            return File.ReadAllText(file);
        }

        /// <summary>
        /// 获取json文件内容
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>字典对象</returns>
        public Dictionary<string, object> GetJsonFile(string file)
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(GetFileContent(file));
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 存储json对象
        /// </summary>
        /// <param name="dic">字典对象</param>
        /// <param name="file">文件路径</param>
        /// <param name="recover">是否覆盖</param>
        public void SaveJsonFile(Dictionary<string, object> dic, string file, bool recover = false)
        {
            try
            {
                SaveFile(file, JsonConvert.SerializeObject(dic), recover);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取文件内容行列表
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns></returns>
        public string[] GetFileLines(string file)
        {
            if (!File.Exists(file))
            {
                throw new Exception($"{file} is not exists.");
            }
            return File.ReadAllLines(file)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s) && !s.StartsWith("#")).ToArray();
        }

        /// <summary>
        /// 保存指定文件内容
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="recover">是否覆盖</param>
        public void SaveFile(string file, string content, bool recover = false)
        {
            if (File.Exists(file) && !recover)
            {
                throw new Exception($"{file} is exists.");
            }
            File.WriteAllText(file, content);
        }

        /// <summary>
        /// 保存指定文件内容
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="recover">是否覆盖</param>
        public void SaveFile(string file, string[] content, bool recover = false)
        {
            if (File.Exists(file) && !recover)
            {
                throw new Exception($"{file} is exists.");
            }
            File.WriteAllLines(file, content);
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>bool</returns>
        public bool DeleteFile(string file)
        {
            try
            {
                File.Delete(file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
