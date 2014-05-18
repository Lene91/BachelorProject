using ExperimentTemplate;
using Eyetracker.MouseTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Reflection;


namespace BachelorProject
{
    public delegate void Del();

    class EntryPoint
    {
        private static Dictionary<int, string> allConstraints = new Dictionary<int, string>();
        private static List<string> allNames = new List<string>();
        

        [STAThread]
        static void Main()
        {
            var experiment = new Experiment(new MouseTracker())
            {
                ShowDefaultStartScreen = false,
                ShowDebugEndScreen = false,
                HideMouseCursor = false
            };

            string textInput = "Console.WriteLine(\"Hallo\");";

            Assembly assembly = CompileCode(textInput);
            object o = assembly.CreateInstance("gfoidl.Code.Test");
            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("Ergebnis");
            
            
           
            

            // Dateien, die für alle Trials benötigt werden
            readConstraints("constraints.txt"); // entsprechend Trialnumber entsprechenden Indexinhalt übergeben
            readNames("names.txt"); // shuffle()-Aufruf gibt neu sortierte Liste zurück

            // Variablen, die für jeden Trial unterschiedlich sind
            int numberOfPersons = 5;

            Del handler = new Del(DelegateMethod);
            experiment.AddTrial(new TrialExampleExercise(numberOfPersons, allConstraints[1], shuffleNames(),handler,mi,o));
            experiment.AddTrial(new TrialEndScreen());
           
            //experiment.AddTrial(new TrialExampleExercise(4, allConstraints[2], shuffleNames()));
            

            experiment.ConfigureTracker();
            experiment.DoCalibration();

            experiment.Run();
        }

        private static void readConstraints(string filename)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@filename,Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                var trialNumber = Convert.ToInt32(line.Substring(0,line.IndexOf(";")));
                var constraints = line.Substring(line.IndexOf(";")+1);
                allConstraints.Add(trialNumber, constraints);
            }
            file.Close();
        }

        private static void readNames(string filename)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@filename, Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                allNames.Add(line);
            }
            file.Close();
        }

        public static List<string> shuffleNames()
        {
            // Source http://stackoverflow.com/questions/5383498/shuffle-rearrange-randomly-a-liststring
            int n = allNames.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                string value = allNames[k];
                allNames[k] = allNames[n];
                allNames[n] = value;
            }
            return allNames;
        }

        public static void DelegateMethod()
        {
            Debug.WriteLine("Delegate verwendet.");
        }

        public static Assembly CompileCode(string CodeInput)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("system.dll");
            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"namespace gfoidl.Code{");
            sb.AppendLine(@"public class Test{");
            sb.AppendLine(@"public string Ergebnis(string input){");
            sb.AppendLine(CodeInput);
            sb.AppendLine(@"return input;");
            sb.AppendLine(@"}}}");

            CompilerResults cr =
                provider.CompileAssemblyFromSource(cp, sb.ToString());

            if (cr.Errors.Count > 0)
            {
                Console.WriteLine(cr.Errors[0].ErrorText);
                return null;
            }

            return cr.CompiledAssembly;
        }
    }
}
