using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace t7
{
    public partial class Form1 : Form
    {
        private const int MaxDepth = 3;
        private readonly string rootDirectory;
       
        public Form1()
        {
            InitializeComponent();
            rootDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string treeText = GenerateTreeText(rootDirectory, 0);
                textBox1.Text = treeText;
            }
            catch (UnauthorizedAccessException ex)
            {
                textBox1.Text = $"Error: {ex.Message}";
            }
        }

        private string GenerateTreeText(string directory, int currentDepth)
        {
            if (currentDepth > MaxDepth)
                return "";

            StringBuilder sb = new StringBuilder();

            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            string prefix = new string('\t', currentDepth);

            sb.AppendLine(prefix + dirInfo.Name);

            foreach (var file in dirInfo.GetFiles())
            {
                sb.AppendLine(prefix + "\t" + file.Name);
            }

            try
            {
                foreach (var subDirectory in dirInfo.GetDirectories())
                {
                    string subTreeText = GenerateTreeText(subDirectory.FullName, currentDepth + 1);
                    sb.AppendLine(subTreeText);
                }
            }
            catch (UnauthorizedAccessException)
            {
                sb.AppendLine(prefix + "\tAcces nepermis");
            }

            return sb.ToString();
        }
    }
}
