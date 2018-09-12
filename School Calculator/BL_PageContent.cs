using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Calculator
{
    class BL_PageContent
    {
        public static string CreatedBy { get; set; }
        public static string VarOutput { get; set; }
        public static void CourseCredits(int requester)
        {
            string[] names;
            switch (requester) // case statement for the name, do things depending on the button clicked
            {
                case 1:
                    names = new string[3] { "COP3488C", "UWP1", "This course is mobile app development." };
                    break;
                case 2:
                    names = new string[3] { "COP3488C", " UWP1", "This course is Universal Windows Applications Programming I." };
                    break;
                case 3:
                    names = new string[3] { "MAT3172", " MTM1", "This course is The Mathematics of Games." };
                    break;
                default:
                    names = new string[3] { "Error", "01", "No a valid argument" };
                    break;
            }
            //string VarOutput = null;
            for (int i = 0; i < names.Length; i++)
            {
                VarOutput = VarOutput + names[i] + "  ";
            }
        }
    }
}
