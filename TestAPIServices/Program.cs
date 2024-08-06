namespace TestAPIServices
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            VCheck.Interface.API.ezyVet.RequestMessage.GetAppointmentRequest sReq = new VCheck.Interface.API.ezyVet.RequestMessage.GetAppointmentRequest();
            sReq.animal_id = "5555";

            VCheck.Interface.API.ezyVetAPI sAPI = new VCheck.Interface.API.ezyVetAPI();
            var sResult = sAPI.GetAppointmentList(sReq, "Token ANCAADADSADADSDD");
            if (sResult != null)
            {
                String abc = "abc";
            }
        }
    }
}
