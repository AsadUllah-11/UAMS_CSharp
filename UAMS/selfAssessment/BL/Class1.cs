using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using selfAssessment.BL;
using selfAssessment.DL;
using selfAssessment.UI;

namespace selfAssessment.BL
{
    class Subject
    {
        public string code;
        public string type;
        public int creditHours;
        public int subjectFees;
        public Subject(string code, string type, int creditHours, int subjectFees)
        {
            this.code = code;
            this.type = type;
            this.creditHours = creditHours;
            this.subjectFees = subjectFees;
        }
    }

    class DegreeProgram
    {
        public string degreeName;
        public float degreeDuration;
        public List<Subject> subjects;
        public int seats;
        public DegreeProgram(string degreeName, float degreeDuration, int seats)
        {
            this.degreeName = degreeName;
            this.degreeDuration = degreeDuration;
            this.seats = seats;
            subjects = new List<Subject>();
        }
        public bool isSubjectExists (Subject sub)
        {
            foreach(Subject s in subjects)
            {
                if(s.code == sub.code)
                {
                    return true;
                }
            }
            return false;
        }
        public bool AddSubject(Subject s)
        {
            int creditHours = calculateCreditHours();
            if (creditHours + s.creditHours <= 20)
            {
                subjects.Add(s);
                return true;
            }
            else
            {
                return false;
            }
        }
        public int calculateCreditHours()
        {
            int count = 0;
            for (int x =0; x < subjects.Count; x++)
            {
                count = count + subjects[x].creditHours;
            }
            return count;
        }
    }
    class Student
    {
        public string name;
        public int age;
        public double fscMarks;
        public double ecatMarks;
        public double merit;
        public List<DegreeProgram> preferences;
        public List<Subject> regSubject;
        public DegreeProgram regDegree;

        public Student(string name, int age, double fscMarks, double ecatMarks, List<DegreeProgram> preferences)
        {
            this.name = name;
            this.age = age;
            this.fscMarks = fscMarks;
            this.ecatMarks = ecatMarks;
            this.preferences = preferences;
            regSubject = new List<Subject>();
        }
        public void calculateMerit()
        {
            this.merit = (((fscMarks/1100) * 0.45F) + ((ecatMarks/ 400)* 0.55F))*100;
        }
        public bool regStudentSubject (Subject s)
        {
            int stCH = getCreditHours();
            if (regDegree != null && regDegree.isSubjectExists(s) && stCH +s.creditHours <=9)
            {
                regSubject.Add(s);
                return true;
            }
            else
            {
                return false;
            }
        }
        public int getCreditHours()
        {
            int count = 0;
            foreach(Subject sub in regSubject)
            {
                count = count + sub.creditHours;
            }
            return count;
        }
        public float calculateFee()
        {
            float fee = 0;
            if(regDegree != null)
            {
                foreach(Subject sub in regSubject)
                {
                    fee = fee + sub.subjectFees;
                }    
            }
            return fee;
        }




    }

}
