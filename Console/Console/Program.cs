using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Console;
using Microsoft.VisualStudio.DebuggerVisualizers;

//[assembly: DebuggerTypeProxy(typeof(ScientistTypeProxy), Target = typeof(Scientist))]
[assembly: DebuggerDisplay("Name: {Name}, Birthday: {Birthday}", Target = typeof(Scientist))]
[assembly: DebuggerVisualizer(typeof(ScientistVisualizer), Target = typeof(Scientist))]


namespace Console
{


    class Program
    {
        static void Main(string[] args)
        {
            var scientists = new[]
            {
                new Scientist("Marie Curie", new DateTime(1867, 11, 7), new []{ "Physics", "Chemistry"}, new []{ "Nobel Prize in Physics", "Nobel Prize in Chemistry", "Davy Medal", "Matteucci Medal", "Elliott Cresson Medal", "Albert Medal", "Willard Gibbs Award" } ), 
                new Scientist("Maria Goeppert-Mayer", new DateTime(1906, 06, 28), new []{ "Physics"}, new []{ "Nobel Prize in Physics" }), 
                new Scientist("Rosalind Franklin", new DateTime(1920, 07, 25), new []{ "Physical Chemistry", "X-ray Crystallography" }, Array.Empty<string>()),
                new Scientist("Barbara McClintock", new DateTime(1902, 06, 16), new []{ "Cytogenetics" }, new []{ "Nobel Prize in Physiology or Medicine", "National Medal of Science", "Thomas Hunt Morgan Medal", "Louisa Gross Horwitz Prize"}), 
            };


            System.Console.ReadKey();
        }
    }
    //[DebuggerDisplay("Name: {Name}, Birthday: {Birthday}", Target = typeof(Scientist))]
    //[DebuggerTypeProxy(typeof(ScientistTypeProxy))]
    //[DebuggerVisualizer(typeof(ScientistVisualizer))]
    [Serializable]
    class Scientist
    {
        public string Name { get; }
        public DateTime Birthday { get; }

        public string[] Fields { get; }
        public string[] Awards { get; }

        public Scientist(string name, DateTime birthday, string[] fields, string[] awards)
        {
            Name = name;
            Birthday = birthday;
            Fields = fields;
            Awards = awards;
        }
    }

    class ScientistTypeProxy
    {
        private readonly Scientist _scientist;

        public ScientistTypeProxy(Scientist scientist)
        {
            _scientist = scientist;
        }

        public string Name => _scientist.Name;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public string[] Fields => _scientist.Fields;
        public int YearsPassed => _scientist.Birthday.YearsPassedSince();
    }

    public class ScientistVisualizer : DialogDebuggerVisualizer
    {
        public ScientistVisualizer()
        {
        }

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var scientist = (Scientist)objectProvider.GetObject();

            Window window = new Window()
            {
                Title = scientist.Name,
                Width = 400,
                Height = 300
            };

            var images = new Dictionary<string, string>
            {
                ["Marie Curie"] = "Marie_Curie.jpg",
                ["Maria Goeppert-Mayer"] = "Maria_Goeppert-Mayer.jpg",
                ["Rosalind Franklin"] = "Rosalind_Franklin.jpg",
                ["Barbara McClintock"] = "Barbara_McClintock.jpg",

            };

        
            if (images.ContainsKey(scientist.Name))
            {
                string imageName = images[scientist.Name];

                window.Background = new ImageBrush(new BitmapImage(new Uri(string.Format("pack://application:,,,/{0};component/Images/{1}", typeof(ScientistVisualizer).Assembly.GetName().Name, imageName))));

                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
        }
    }

    public static class Extensions
    {
        public static int YearsPassedSince(this DateTime dateTime)
            => (new DateTime(1, 1, 1) + (DateTime.Now - dateTime)).Year - 1;
    }
}
