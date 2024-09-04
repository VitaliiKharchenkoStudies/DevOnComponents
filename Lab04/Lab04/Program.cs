using System;
using System.IO;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;

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

        var keywords = new[] { "public", "override", "bool", "int", "return" };
        var words = inputText.Split(' ');

        var range = para.Range;

        foreach (var word in words)
        {
            if (word.StartsWith("+"))
            {
                // Bold words starting with +
                var keyword = word.Substring(1); // Remove '+'
                range.InsertAfter(keyword + " ");
                range.Font.Bold = 1;
            }
            else if (word.StartsWith("[") && word.EndsWith("]"))
            {
                // Bold text between [ and ]
                range.InsertAfter(word + " ");
                range.Font.Bold = 1;
            }
            else if (keywords.Contains(word))
            {
                // Words with green background
                range.InsertAfter(word);
                range.Shading.BackgroundPatternColor = Word.WdColor.wdColorGreen;

                range.InsertAfter(" ");
            }
            else
            {
                // Regular text
                range.InsertAfter(word + " ");
                range.Font.Bold = 0;
                range.Shading.BackgroundPatternColor = Word.WdColor.wdColorAutomatic; // Regular background
            }

            // Move the range to the end of the current content after inserting text
            range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        }

        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FormattedDocument.docx");
        doc.SaveAs2(filePath);

        doc.Close();
        wordApp.Quit();

        System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
    }
}