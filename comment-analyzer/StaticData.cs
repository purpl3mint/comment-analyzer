﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comment_analyzer
{
    public static class StaticData
    {
        public static DefaultDialogService dialogService = new DefaultDialogService();
        public static DefaultFileService fileService = new DefaultFileService();
        public static CommentAnalyzerForm mainForm;
        public static string currentData = "";
        public static bool unsaved = false;
        public static Stack<string> undoStack = new Stack<string>();
        public static Stack<string> redoStack = new Stack<string>();
        public static Commands commands = new Commands();
        public static string prePath = "";
    }
}
