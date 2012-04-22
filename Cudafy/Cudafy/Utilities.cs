﻿/*
CUDAfy.NET - LGPL 2.1 License
Please consider purchasing a commerical license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2011 Hybrid DSP Systems
http://www.hybriddsp.com

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Cudafy.Types;
namespace Cudafy
{
    /// <summary>
    /// Internal use.
    /// </summary>
    public static class CV
    {
        /// <summary>
        /// Version 1.9.*
        /// </summary>
        public const string csVERSION = "1.9.*";
    }

    /// <summary>
    /// Class used for performing checksum.
    /// </summary>
    public class Crc32
    {
        uint[] table;

        /// <summary>
        /// Computes the checksum.
        /// </summary>
        /// <param name="location">The file.</param>
        /// <returns>Checksum.</returns>
        public static long ComputeChecksum(string location)
        {
            long checksum = 0;
            using (FileStream fs = new FileStream(location, FileMode.Open, FileAccess.Read))
            {
                byte[] ba = new byte[fs.Length];
                fs.Read(ba, 0, ba.Length);
                Crc32 crc = new Crc32();
                checksum = crc.ComputeChecksum(ba);
            }
            return checksum;
        }

        /// <summary>
        /// Computes the checksum.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Checksum.</returns>
        public long ComputeChecksum(byte[] bytes)
        {
            uint crc = 0xffffffff;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = (uint)((crc >> 8) ^ table[index]);
            }
            return ~crc;
        }

        /// <summary>
        /// Computes the checksum.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <returns>Checksum.</returns>
        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            return BitConverter.GetBytes(ComputeChecksum(bytes));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crc32"/> class.
        /// </summary>
        public Crc32()
        {
            uint poly = 0xedb88320;
            table = new uint[256];
            uint temp = 0;
            for (uint i = 0; i < table.Length; ++i)
            {
                temp = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (uint)((temp >> 1) ^ poly);
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                table[i] = temp;
            }
        }
    }

    /// <summary>
    /// Utility methods.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Dumps supplied text to file.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="filename">The file.</param>
        /// <returns>The text.</returns>
        public static string DumpToFile(string text, string filename)
        {
            using (StreamWriter fs = new StreamWriter(filename))
            {
                fs.Write(text);
            }
            return text;
        }

        /// <summary>
        /// Gets the x86 program files directory.
        /// </summary>
        /// <returns>x86 program files directory.</returns>
        public static string ProgramFilesx86() 
        { 
            if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))) 
            { 
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)"); 
            } 
            return Environment.GetEnvironmentVariable("ProgramFiles"); 
        }

        /// <summary>
        /// Gets the x64 program files directory.
        /// </summary>
        /// <returns>x64 program files directory or empty string if does not exist.</returns>
        public static string ProgramFilesx64()
        {
            if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles");
            }
            return string.Empty;
        }

        public static string ProgramFiles()
        {
            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        /// <summary>
        /// Converts the specified values to an array of floats.
        /// </summary>
        /// <param name="cplx">The values.</param>
        /// <returns></returns>
        public static float[] Convert(ComplexF[] cplx)
        {
            float[] fa = new float[cplx.Length * 2];
            for (int i = 0; i < cplx.Length; i++)
            {
                fa[i * 2] = cplx[i].x;
                fa[i * 2 + 1] = cplx[i].y;
            }
            return fa;
        }

        /// <summary>
        /// Converts the specified values to an array of doubles.
        /// </summary>
        /// <param name="cplx">The values.</param>
        /// <returns></returns>
        public static double[] Convert(ComplexD[] cplx)
        {
            double[] fa = new double[cplx.Length * 2];
            for (int i = 0; i < cplx.Length; i++)
            {
                fa[i * 2] = cplx[i].x;
                fa[i * 2 + 1] = cplx[i].y;
            }
            return fa;
        }
    }


}
