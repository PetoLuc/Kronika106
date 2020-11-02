using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Words.NET;

namespace ExportToWord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string filename = "test.docx";
            var doc = DocX.Create(filename);
            //doc.InsertParagraph("Hello Word");

            string title = "ROK - AKCIA";
            string akciaPopis = "AKCIA - popis";
            string akciaKomentare = "" + "Dear Friends, " + Environment.NewLine + "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " + "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " + Environment.NewLine + "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " + "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " + "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";

            Formatting formatTitle = new Formatting();
            formatTitle.FontFamily = new Font("Algerian");
            formatTitle.Size = 18;
            
            //ako daleko za textom s tymto formatovanim moze byt dalsi text
            formatTitle.Position = 40;
            //zistit center            
            
            formatTitle.FontColor = System.Drawing.Color.Orange;
            formatTitle.UnderlineColor = System.Drawing.Color.Green;
            formatTitle.Italic = true;


            Xceed.Words.NET.Paragraph pgTitle = doc.InsertParagraph(title, false, formatTitle);
            pgTitle.Alignment = Alignment.center;        

            Formatting formatText = new Formatting();
            formatText.FontFamily = new Font("Calibri (Základný text)");
            formatText.Size = 10;
            formatText.Spacing = 2;
            var textParagraph = doc.InsertParagraph(akciaKomentare, false, formatText);


            var image = doc.AddImage(@"C:\Projects\Projekty\Kronika106_prihlasovanie\Kronika106\Kronika106\AllPhotos\2007\Zimná chata\AkciaFotka.jpg");
            var picture = image.CreatePicture();
            textParagraph.AppendPicture(picture);
            


            doc.Save();
            Process.Start("WINWORD.EXE", filename);
        }
    }
}
