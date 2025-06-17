using Reqnroll;

namespace QuoteAcceptanceTests.StepDefinitions
{
    [Binding]
    public class Hooks
    {
        public static HttpClient SharedClient;
        
        [BeforeScenario]
        public void PrintScenarioTitle(ScenarioContext scenarioContext)
        {
            Console.WriteLine($"[INFO] Running Scenario: {scenarioContext.ScenarioInfo.Title}");
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            SharedClient = new HttpClient(handler);
            SharedClient.BaseAddress = new Uri("http://localhost:54958");
            Console.WriteLine("[INFO] SharedClient.BaseAddress = " + SharedClient.BaseAddress);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            SharedClient.Dispose();
        }
    }
}
