// ----------------------------------------------------------------------
// <copyright file="IsAliveSteps.cs" company="Eurofins">
//  Copyright 2024 Eurofins Scientific Ltd, Ireland
//  Usage reserved to Eurofins Global Franchise Model subscribers.
// </copyright>
// ----------------------------------------------------------------------

namespace QuoteAcceptanceTests.StepDefinitions
{
    using System.Net;
    using FluentAssertions;
    using Reqnroll;

    /// <summary>
    /// The is alive steps.
    /// </summary>
    [Binding, Scope(Tag = "IsAlive")]
    internal class IsAliveSteps
    {
        /// <summary>
        /// The response.
        /// </summary>
        private HttpResponseMessage? response;

        /// <summary>
        /// The client.
        /// </summary>
        private HttpClient client = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });


        #region Given
        [Given(@"the Quote Service Running")]
        public void GivenTheQuoteServiceRunning()
        {

        }

        #endregion

        #region When

        /// <summary>
        /// The when i call is alive.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [When(@"I check if it is alive")]
        public async Task WhenICallIsAlive()
        {
            //Uri uri = new Uri("http://localhost:59252/api/Quotes/isalive");

            this.response = await Hooks.SharedClient.GetAsync("/api/Quotes/isalive");
        }

        #endregion

        #region Then

        /// <summary>
        /// The then it returns ok.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        [Then(@"it returns OK \(http (\d+)\)")]
        public void ThenItReturnsOk(int a)
        {
            var httpResponseMessage = this.response;
            if (httpResponseMessage != null)
            {
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
                ((int)httpResponseMessage.StatusCode).Should().Be(a);
            }
        }

        #endregion
    }
}
