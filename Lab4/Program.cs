using System;
using System.IO;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputText = ReadInputText();

            GenerateWordDocument(inputText);

            Console.WriteLine("Word document generated successfully.");
        }

        static string ReadInputText()
        {
            Console.WriteLine("Enter text with markup in a single line:");
            return Console.ReadLine();
        }

        static void GenerateWordDocument(string inputText)
        {
            // Initialize Word application
            var wordApp = new Word.Application() { Visible = true };

            // Create a new document
            var doc = wordApp.Documents.Add();
            var para = doc.Content.Paragraphs.Add();

            var words = inputText.Split(' ');

            var range = para.Range;

            foreach (var word in words)
            {
                string trimmedWord = word.Trim();

                if (word.Contains('?'))
                {
                    range.InsertAfter(trimmedWord + " ");
                    range.Shading.BackgroundPatternColor = Word.WdColor.wdColorPink;
                }
                else if (word.Length != trimmedWord.Length)
                {
                    range.InsertAfter(trimmedWord + " ");
                    range.Font.Color = Word.WdColor.wdColorGrayText;
                }
                else if (word.Length == 1 && char.IsDigit(word[0]))
                {
                    range.InsertAfter(trimmedWord);
                    range.Font.Bold = 1;
                    range.Font.Subscript = 1;
                    range.InsertAfter(" ");
                }
                else
                {
                    range.InsertAfter(trimmedWord + " ");
                    range.Font.Bold = 0;
                    range.Shading.BackgroundPatternColor = Word.WdColor.wdColorAutomatic;
                    range.Font.Color = Word.WdColor.wdColorAutomatic;
                }

                range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            }

            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FormattedDocument.docx");
            doc.SaveAs2(filePath);

            doc.Close();
            wordApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
        }
    }
}
