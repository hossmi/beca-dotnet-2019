using System;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        //Random enrollmentGenerator = new Random();
        //private const int ENROLLMENT_CHAR_QUANTITY = 8;
        //private List<String> usedEnrollments = new List<String>();

        private double enrollmentCount = 0;

        string IEnrollmentProvider.getNewEnrollment()
        {

            String enrollmentPlate = (this.enrollmentCount++).ToString();

            /*do
            {
                enrollmentPlate = "";

                for (int enrollmentChars = 0; enrollmentChars < ENROLLMENT_CHAR_QUANTITY; enrollmentChars++)
                {

                    enrollmentPlate += enrollmentGenerator.Next();
                }

            } while (usedEnrollments.Contains(enrollmentPlate));

            usedEnrollments.Add(enrollmentPlate);*/

            return enrollmentPlate;
        }
    }
}