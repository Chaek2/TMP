using System;
using System.Data;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace Porlam
{
    public class WordSert
    {
        private static Word.Application app = new Word.Application();
        private static string generalfile = @"E:\2\Porlam\tmp\sert.doc";
        private static string newfile = @"E:\2\tmp\";
        private static Object fileName = generalfile;
        private static Object missing = Type.Missing;

        private bool OpenFile()
        {
            try
            {
                app.Documents.OpenNoRepairDialog(generalfile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SaveCloseFile(string name)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(newfile);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            app.ActiveDocument.SaveAs(newfile+name+".doc");
            app.ActiveDocument.Close();
            app.Quit();
        }

        private void FindReplace(string str_old, string str_new)
        {
            Word.Find find = app.Selection.Find;

            find.Text = str_old;
            find.Replacement.Text = str_new;

            find.Execute(FindText: Type.Missing, MatchCase: false, MatchWholeWord: false, MatchWildcards: false,
                        MatchSoundsLike: missing, MatchAllWordForms: false, Forward: true, Wrap: Word.WdFindWrap.wdFindContinue,
                        Format: false, ReplaceWith: missing, Replace: Word.WdReplace.wdReplaceAll);
        }

        public void Sert(DataRow dr)
        {
            bool open = OpenFile();
            if (open)
            {
                FindReplace("Имя Фамилия", dr["Surname"].ToString() + " " + dr["Name"].ToString());
                FindReplace("LFNF", DateTime.Now.ToShortDateString().Replace(" 12:00:00", ""));
                SaveCloseFile(dr["ID_People"].ToString());
            }
        }
    }
}
