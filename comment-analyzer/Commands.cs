using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comment_analyzer
{
    public class Commands
    {

        public void CommandCreate()
        {
            if (StaticData.unsaved)
            {
                StaticData.currentData = StaticData.mainForm.TextBox.Text;
                var saveBeforeCloseWindow = new SaveBeforeCloseForm();
                saveBeforeCloseWindow.ShowDialog();
            }

            StaticData.dialogService.FilePath = "";
            StaticData.currentData = "";
            StaticData.mainForm.TextBox.Text = StaticData.currentData;
            StaticData.mainForm.Heading = "Language Processor - unnamed";
        }

        public void CommandOpen()
        {
            if (StaticData.unsaved)
            {
                StaticData.currentData = StaticData.mainForm.TextBox.Text;
                var saveBeforeCloseWindow = new SaveBeforeCloseForm();
                saveBeforeCloseWindow.ShowDialog();
            }

            StaticData.dialogService.OpenFileDialog();
            StaticData.currentData = StaticData.fileService.ReadFile(StaticData.dialogService.FilePath);

            StaticData.mainForm.TextBox.Text = StaticData.currentData;

            StaticData.mainForm.Heading = "Language Processor";
            if (StaticData.dialogService.FilePath != null || StaticData.dialogService.FilePath != "")
                StaticData.mainForm.Heading += " - " + StaticData.dialogService.FilePath;
            else
                StaticData.mainForm.Heading += " - unnamed";

            StaticData.unsaved = false;
        }

        public void CommandSave()
        {
            StaticData.currentData = StaticData.mainForm.TextBox.Text;

            if (StaticData.dialogService.FilePath == null)
            {
                StaticData.dialogService.SaveFileDialog();
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }
            else
            {
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }

            StaticData.unsaved = false;
            StaticData.mainForm.Heading = "Language Processor - " + StaticData.dialogService.FilePath;
        }

        public void CommandSaveAs()
        {
            StaticData.currentData = StaticData.mainForm.TextBox.Text;
            StaticData.dialogService.SaveFileDialog();
            StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            StaticData.mainForm.Heading = "Language Processor - " + StaticData.dialogService.FilePath;
            StaticData.unsaved = false;
        }

        public void CommandUndo()
        {
            if (StaticData.undoStack.Count > 0)
            {
                StaticData.redoStack.Push(StaticData.mainForm.TextBox.Text);
                string newValue = StaticData.undoStack.Pop();
                StaticData.mainForm.TextBox.Text = newValue;
            }
        }

        public void CommandRedo()
        {
            if (StaticData.redoStack.Count > 0)
            {
                StaticData.undoStack.Push(StaticData.mainForm.TextBox.Text);
                string newValue = StaticData.redoStack.Pop();
                StaticData.mainForm.TextBox.Text = newValue;
            }
        }

        public void CommandCopy()
        {
            if (StaticData.mainForm.TextBox.SelectionLength > 0)
                StaticData.mainForm.TextBox.Copy();
        }
        public void CommandPaste()
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                if (StaticData.mainForm.TextBox.SelectionLength > 0)
                {
                    StaticData.mainForm.TextBox.SelectionStart = StaticData.mainForm.TextBox.SelectionStart + StaticData.mainForm.TextBox.SelectionLength;
                }
                StaticData.mainForm.TextBox.Paste();
            }
        }

        public void CommandCut()
        {
            if (StaticData.mainForm.TextBox.SelectedText != "")
                StaticData.mainForm.TextBox.Cut();
        }

        public void CommandDelete()
        {
            int StartPosDel = StaticData.mainForm.TextBox.SelectionStart;
            int LenSelection = StaticData.mainForm.TextBox.SelectionLength;
            StaticData.mainForm.TextBox.Text = StaticData.mainForm.TextBox.Text.Remove(StartPosDel, LenSelection);
        }

        public void CommandSelectAll()
        {
            StaticData.mainForm.TextBox.SelectAll();
        }

        public void CommandHelp()
        {
            Help.ShowHelp(null, "../../help/help1.html");
        }

        public void CommandCheck()
        {
            string[] errors = { "Ошибок не обнаружено",
                                "Пустая строка",
                                "Строка не является комментарием(начинается не с символа *)"};
            string warning = "Обнаружена последовательность символов не из алфавита '";

            int line = 0;
            int status = 0;
            bool hasErrors = false;

            int errorsCount = 0;
            int warnCount = 0;

            string[] strings = StaticData.mainForm.TextBox.Text.Split('\n');
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].TrimEnd('\r');
            }

            List<string> wrongSymbols = new List<string>();
            List<int> wrongPositions = new List<int>();
            Automata automata = new Automata();

            StaticData.mainForm.ResultsTextBox.Text = "";

            for (line = 0; line < strings.Length; line++)
            {
                status = automata.analyzeLine(strings[line], ref wrongSymbols, ref wrongPositions);

                if(status >= 3)
                {
                    for(int j = 0; wrongSymbols.Count > j; j++)
                    {
                        warnCount++;
                        StaticData.mainForm.ResultsTextBox.Text += "Строка " + (line + 1).ToString() + ": [Предуп] " +
                            warning + wrongSymbols[j] + "'(начало с " + (wrongPositions[j] + 1) + " символа)" + Environment.NewLine;
                    }
                    status -= 3;
                    wrongSymbols.Clear();
                    wrongPositions.Clear();
                }
                if (status != 0)
                {
                    StaticData.mainForm.ResultsTextBox.Text += "Строка " + (line + 1).ToString() + ": [Ошибка] " +
                            errors[status] + Environment.NewLine;
                    hasErrors = true;
                    errorsCount++;
                }

            }

            StaticData.mainForm.ResultsTextBox.Text += Environment.NewLine;

            if (warnCount > 0)
            {
                StaticData.mainForm.ResultsTextBox.Text += "Предупреждений: " + warnCount + Environment.NewLine;
            }
            if (!hasErrors)
            {
                StaticData.mainForm.ResultsTextBox.Text += "Ошибок не обнаружено" +  Environment.NewLine;
            }
            else
            {
                StaticData.mainForm.ResultsTextBox.Text += "Ошибок: " + errorsCount + Environment.NewLine;
            }
        }
    }
}
