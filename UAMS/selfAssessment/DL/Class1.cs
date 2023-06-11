using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using selfAssessment.BL;
using selfAssessment.DL;
using selfAssessment.UI;
using System.IO;

namespace selfAssessment.DL
{
    class SubjectDL
    {
        public static List<Subject> subjectList = new List<Subject>();
        public static void addSubjectIntoList(Subject s)
        {
            subjectList.Add(s);
        }
        public static bool readFromFile(string path)
        {
            StreamReader f = new StreamReader(path);
            string record;
            if (File.Exists(path))
            {
                while ((record = f.ReadLine()) != null)
                {
                    string[] splittedRecord = record.Split(',');
                    string code = splittedRecord[0];
                    string type = splittedRecord[1];
                    int creditHours = int.Parse(splittedRecord[2]);
                    int subjectFees = int.Parse(splittedRecord[3]);
                    Subject s = new Subject(code, type, creditHours, subjectFees);
                    addSubjectIntoList(s);
                }
                f.Close();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void storeintoFile(string path, Subject s)
        {
            StreamWriter f = new StreamWriter(path, true);
            f.WriteLine(s.code + "," + s.type + "," + s.creditHours + "," + s.subjectFees);
            f.Flush();
            f.Close();
        }
        public static Subject isSubjectExists(string type)
        {
            foreach (Subject s in subjectList)
            {
                if (s.type == type)
                {
                    return s;
                }
            }
            return null;
        }


    }
    class DegreeProgramDL
    {
        public static List<DegreeProgram> programList = new List<DegreeProgram>();
        public static void addIntoDegreeList(DegreeProgram d)
        {
            programList.Add(d);
        }
        public static DegreeProgram isDegreeExists(string degreeName)
        {
            foreach (DegreeProgram d in programList)
            {
                if (d.degreeName == degreeName)
                {
                    return d;
                }
            }
            return null;
        }
        public static void storeintoFile(string path, DegreeProgram d)
        {
            StreamWriter f = new StreamWriter(path, true);
            string SubjectNames = "";
            for (int x = 0; x < d.subjects.Count - 1; x++)
            {
                SubjectNames = SubjectNames + d.subjects[x].type + ";";
            }
            SubjectNames = SubjectNames + d.subjects[d.subjects.Count - 1].type;
            f.WriteLine(d.degreeName + "," + d.degreeDuration + "," + d.seats + "," + SubjectNames);
            f.Flush();
            f.Close();
        }

        public static bool readFromFile(string path)
        {
            StreamReader f = new StreamReader(path);
            string record;
            if (File.Exists(path))
            {
                while ((record = f.ReadLine()) != null)
                {
                    string[] splittedRecord = record.Split(',');
                    string degreeName = splittedRecord[0];
                    float degreeDuration = float.Parse(splittedRecord[1]);
                    int seats = int.Parse(splittedRecord[2]);
                    string[] splittedRecordForSubject = splittedRecord[3].Split(';');
                    DegreeProgram d = new DegreeProgram(degreeName, degreeDuration, seats);
                    for (int x = 0; x < splittedRecordForSubject.Length; x++)
                    {
                        Subject s = SubjectDL.isSubjectExists(splittedRecordForSubject[x]);
                        if (s != null)
                        {
                            d.AddSubject(s);
                        }
                    }
                    addIntoDegreeList(d);
                }
                f.Close();
                return true;
            }
            else
            {
                return false;
            }
        }


    }
    class StudentDL
    {
        public static List<Student> studentList = new List<Student>();
        public static void addIntoStudentList(Student s)
        {
            studentList.Add(s);
        }

        public static Student StudentPresent(string name)
        {
            foreach(Student s in studentList)
            {
                if (name == s.name && s.regDegree != null)
                {
                    return s;
                }
            }
            return null;
        }

        public static List<Student> sortStudentByMerit()
        {
            List<Student> sortedStudentList = new List<Student>();
            foreach (Student s in studentList)
            {
                s.calculateMerit();
            }
            sortedStudentList = studentList.OrderByDescending(o => o.merit).ToList();
            return sortedStudentList;
        }

        public static void giveAdmission (List<Student> sortedStudentList)
        {
            foreach( Student s in sortedStudentList)
            {
                foreach(DegreeProgram d in s.preferences)
                {
                    if (d.seats > 0 && s.regDegree == null)
                    {
                        s.regDegree = d;
                        d.seats--;
                        break;
                    }
                }
            }
        }
        
        public static void storeintoFile(string path, Student s)
        {
            StreamWriter f = new StreamWriter(path, true);
            string degreeNames = "";
            for (int x =0; x<s.preferences.Count -1; x++)
            {
                degreeNames = degreeNames + s.preferences[x].degreeName + ";";
            }
            degreeNames = degreeNames + s.preferences[s.preferences.Count - 1].degreeName;
            f.WriteLine(s.name + "," + s.age + "," + s.fscMarks + "," + s.ecatMarks + "," + degreeNames);
            f.Flush();
            f.Close();
        }

        public static bool readFromFile(string path)
        {
            StreamReader f = new StreamReader(path);
            string record;
            if(File.Exists(path))
            {
                while((record = f.ReadLine()) != null)
                {
                    string[] splittedRecord = record.Split(',');
                    string name = splittedRecord[0];
                    int age = int.Parse(splittedRecord[1]);
                    double fscMarks = double.Parse(splittedRecord[2]);
                    double ecatMarks = double.Parse(splittedRecord[3]);
                    string[] splittedRecordForPreference = splittedRecord[4].Split(';');
                    List<DegreeProgram> preferences = new List<DegreeProgram>();
                    for (int x = 0; x<splittedRecordForPreference.Length; x++)
                    {
                        DegreeProgram d = DegreeProgramDL.isDegreeExists(splittedRecordForPreference[x]);
                        if(d != null)
                        {
                            if(!(preferences.Contains(d)))
                            {
                                preferences.Add(d);
                            }
                        }
                    }
                    Student s = new Student(name, age, fscMarks, ecatMarks, preferences);
                    studentList.Add(s);
                }
                f.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}
