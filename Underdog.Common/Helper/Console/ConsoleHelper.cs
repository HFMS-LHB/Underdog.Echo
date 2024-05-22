﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Underdog.Common.Helper.Console
{
    public static partial class ConsoleHelper
    {
        [LibraryImport("kernel32.dll")]
        private static partial IntPtr GetConsoleWindow();


        private static readonly object _objLock = new();

        /// <summary>
        /// 在控制台输出
        /// </summary>
        /// <param name="str">文本</param>
        /// <param name="color">前颜色</param>
        public static void WriteColorLine(string str, ConsoleColor color)
        {
            lock (_objLock)
            {
                ConsoleColor currentForeColor = System.Console.ForegroundColor;
                System.Console.ForegroundColor = color;
                System.Console.WriteLine(str);
                System.Console.ForegroundColor = currentForeColor;
            }
        }

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteErrorLine(this string str, ConsoleColor color = ConsoleColor.Red) => WriteColorLine(str, color);

        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteWarningLine(this string str, ConsoleColor color = ConsoleColor.Yellow) => WriteColorLine(str, color);

        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteInfoLine(this string str, ConsoleColor color = ConsoleColor.White) => WriteColorLine(str, color);

        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteSuccessLine(this string str, ConsoleColor color = ConsoleColor.Green) => WriteColorLine(str, color);

        public static bool IsConsoleApp()
        {
            return GetConsoleWindow() != IntPtr.Zero;
        }
    }
}
