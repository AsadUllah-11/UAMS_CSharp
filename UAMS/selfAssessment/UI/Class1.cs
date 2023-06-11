using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using selfAssessment.BL;
using selfAssessment.DL;
using selfAssessment.UI;

namespace selfAssessment.UI
{
    class SubjectUI
    {
        public static Subject takeInputForSubject()
        {
            string code;
            string type;
            int creditHours;
            int subjectFees;
            Console.Write("Enter Subject Code: ");
            code = Console.ReadLine();
            Console.Write("Enter subject Type: ");
            type = Console.ReadLine();
            Console.Write("Enter Subject Credit Hours: ");
            creditHours = int.Parse(Console.ReadLine());
            Console.Write("Etner Subject Fees: ");
            subjectFees = int.Parse(Console.ReadLine());
            Subject sub = new Subject(code, type, creditHours, subjectFees);
            return sub;
        }

        public static void viewSubjects(Student s)
        {
            if (s.regDegree != null)
            {
                Console.WriteLine("Sub Code\tSub Type");
                foreach (Subject sub in s.regDegree.subjects)
                {
                    Console.WriteLine(sub.code + "\t\t" + sub.type);
                }
            }
        }

        public static void registerSubjects(Student s)
        {
            Console.WriteLine("Enter how many subjects you want to register");
            int count = int.Parse(Console.ReadLine());
            for(int x=0; x<count; x++)
            {
                Console.WriteLine("Enter the subject Code");
                string code = Console.ReadLine();
                bool Flag = false;
                foreach (Subject sub in s.regDegree.subjects)
                {
                    if (code == sub.code && !(s.regSubject.Contains(sub)))
                    {
                        if (s.regStudentSubject(sub))
                        {
                            Flag = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("A student cannot have more than 9 CH");
                            Flag = true;
                            break;
                        }
                    }
                }
                if (Flag == false)
                {
                    Console.WriteLine("Enter Valid Course");
                    x--;
                }
            }
        }

    }
    class DegreeProgramUI
    {
        public static DegreeProgram takeInputForDegree()
        {
            string degreeName;
            float degreeDuration;
            int seats;
            Console.Write("Enter Degree Name: ");
            degreeName = Console.ReadLine();
            Console.Write("Enter Degree Duration: ");
            degreeDuration = float.Parse(Console.ReadLine());
            Console.Write("Enter Seats for Degree: ");
            seats = int.Parse(Console.ReadLine());

            DegreeProgram degProg = new DegreeProgram(degreeName, degreeDuration, seats);
            Console.Write("Enter How many Subjects to Enter: ");
            int count = int.Parse(Console.ReadLine());

            for (int x = 0; x < count; x++)
            {
                Subject s = SubjectUI.takeInputForSubject();
                if (degProg.AddSubject(s))
                {
                    // These are done here because we did not add a separate option to add only the Subjects.
                    if (!(SubjectDL.subjectList.Contains(s)))
                    {
                        SubjectDL.addSubjectIntoList(s);
                        SubjectDL.storeintoFile("subject.txt", s);
                    }
                    Console.WriteLine("Subject Added");
                }
                else
                {
                    Console.WriteLine("Subject Not Added");
                    Console.WriteLine("20 credit hour limit exceeded");
                    x--;
                }
            }
            return degProg;
        }
        public static void viewDegreePrograms()
        {
            foreach(DegreeProgram dp in DegreeProgramDL.programList)
            {
                Console.WriteLine(dp.degreeName);
            }
        }
    }
    class StudentUI
    {
        public static void printStudents()
        {
            foreach(Student s in StudentDL.studentList)
            {
                if (s.regDegree != null)
                {
                    Console.WriteLine(s.name + " got Admission in " + s.regDegree.degreeName);
                }
                else
                {
                    Console.WriteLine(s.name + " did not get Admission");
                }
            }
        }

        public static void viewStudentInDegree(string degName)
        {
            Console.WriteLine("Name\tFSC\tEcat\tAge");
            foreach(Student s in StudentDL.studentList)
            {
                if (s.regDegree != null)
                {
                    if (degName == s.regDegree.degreeName)
                    {
                        Console.WriteLine(s.name + "\t" + s.fscMarks + "\t" + s.ecatMarks + "\t" + s.age);
                    }
                }
            }
        }

        public static void viewRegisteredStudents()
        {
            Console.WriteLine("Name\tFSC\tEcat\tAge");
            foreach(Student s in StudentDL.studentList)
            {
                if (s.regDegree != null)
                {
                    Console.WriteLine(s.name + "\t" + s.fscMarks + "\t" + s.ecatMarks + "\t" + s.age);
                }

            }
        }

        public static Student takeInputForStudent()
        {
            string name;
            int age;
            double fscMarks;
            double ecatMarks;
            List<DegreeProgram> preferences = new List<DegreeProgram>();
            Console.Write("Enter Student Name: ");
            name = Console.ReadLine();
            Console.Write("Enter Student Age: ");
            age = int.Parse(Console.ReadLine());
            Console.Write("Enter Student FSc Marks: ");
            fscMarks = double.Parse(Console.ReadLine());
            Console.Write("Enter Student Ecat Marks: ");
            ecatMarks = double.Parse(Console.ReadLine());
            Console.WriteLine("Available Degree Programs");
            DegreeProgramUI.viewDegreePrograms();
            Console.Write("Enter how many preferences to Enter: ");
            int Count = int.Parse(Console.ReadLine());
            for (int x = 0; x<Count; x++)
            {
                string degName = Console.ReadLine();
                bool flag = false;
                foreach(DegreeProgram dp in DegreeProgramDL.programList)
                {
                    if (degName == dp.degreeName && !(preferences.Contains(dp)))
                    {
                        preferences.Add(dp);
                        flag = true;
                    }
                }
                if (flag == false)
                {
                    Console.WriteLine("Enter Valid Degree Program Name");
                    x--;
                }
            }
            Student s = new Student(name, age, fscMarks, ecatMarks, preferences);
            return s;

        }

        public static void calculateFeeForAll()
        {
            foreach(Student s in StudentDL.studentList)
            {
                if(s.regDegree != null)
                {
                    Console.WriteLine(s.name + " has " + s.calculateFee() + " fees");
                }
            }
        }


    }
    class MenuUI
    {
        public static void header()
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("                   UAMS                     ");
            Console.WriteLine("********************************************");
        }
        public static void clearScreen()
        {
            Console.WriteLine("Press any key to Continue...");
            Console.ReadKey();
            Console.Clear();
        }

        public static int Menu()
        {
            header();
            int option;
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. Add Degree Program");
            Console.WriteLine("3. Generate Merit");
            Console.WriteLine("4. View Registered Students");
            Console.WriteLine("5. View Students of a Specific Program");
            Console.WriteLine("6. Register Subjects for a Specific Student");
            Console.WriteLine("7. Calculate Fees for all Registered Students");
            Console.WriteLine("8. Exit");
            Console.Write("Enter Option: ");
            option = int.Parse(Console.ReadLine());
            return option;

        }


    }
        
      

}

