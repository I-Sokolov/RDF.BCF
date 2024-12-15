using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RDF.BCF
{
    public class Native
    {
        private const string DDFBCFDLL = @"..\..\bcfEngine.dll";

        /// <summary>
        /// BCF file version
        /// </summary>
        public enum Version
        {
            _2_1 = 21,
            _3_0 = 30
        }

        [DllImport(DDFBCFDLL, EntryPoint = "bcfOpenProject")]
        public static extern IntPtr OpenProject();

        [DllImport(DDFBCFDLL, EntryPoint = "bcfCloseProject")]
        public static extern void CloseProject(IntPtr project);

        [DllImport(DDFBCFDLL, EntryPoint = "bcfInitNew")]
        public static extern bool InitNew();

        [DllImport(DDFBCFDLL, EntryPoint = "bcfReadFile")]
        public static extern bool ReadFile(string filePath);

        [DllImport(DDFBCFDLL, EntryPoint = "bcfWriteFile")]
        public static extern bool WriteFile(string filePath, Version version);
    }
}
