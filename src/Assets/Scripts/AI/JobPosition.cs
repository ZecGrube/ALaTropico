using UnityEngine;

namespace CaudilloBay.AI
{
    [System.Serializable]
    public class JobPosition
    {
        public string title;
        public EducationLevel requiredEducation;
        public float baseSalary;
        public World.Building workplace;
        public Citizen currentEmployee;
        public bool isFilled => currentEmployee != null;

        public JobPosition(string t, EducationLevel edu, float salary, World.Building b)
        {
            title = t;
            requiredEducation = edu;
            baseSalary = salary;
            workplace = b;
        }
    }

    [System.Serializable]
    public class LaborContract
    {
        public int employeeId;
        public float salary;
        public int monthsRemaining;

        public LaborContract(int id, float s, int duration = 12)
        {
            employeeId = id;
            salary = s;
            monthsRemaining = duration;
        }
    }
}
