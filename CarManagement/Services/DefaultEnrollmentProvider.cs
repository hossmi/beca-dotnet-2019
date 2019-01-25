namespace CarManagement.Services
{
    public class DefaultEnrollmentProvider : IEnrollmentProvider
    {
        private string letter;
        private int number;

        DefaultEnrollmentProvider()
        {
            this.letter = "";
            this.number = 0;
        }
        string IEnrollmentProvider.NewEnrollment
        {
            get
            {

            }
        }
    }
}