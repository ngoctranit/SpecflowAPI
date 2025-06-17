// ----------------------------------------------------------------------
// <copyright file="CreateQuoteService.cs" company="Eurofins">
//  Copyright 2024 Eurofins Scientific Ltd, Ireland
//  Usage reserved to Eurofins Global Franchise Model subscribers.
// </copyright>
// ----------------------------------------------------------------------

namespace QuoteService
{
    using Models;

    /// <summary>
    /// The create quote service.
    /// </summary>
    public class CreateQuoteService
    {
        /// <summary>
        /// The successful message.
        /// </summary>
        private const string SuccessfulMessage = "Quote created successfully.";

        /// <summary>
        /// The error message.
        /// </summary>
        private const string ErrorMessage = "Cannot create the quote for a null item.";

        /// <summary>
        /// The create quote.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="CreateQuoteResponse"/>.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// Application Error
        /// </exception>
        public CreateQuoteResponse CreateQuote(CreateQuoteRequest request)
        {
            try
            {
                CreateQuoteResponse response;
                if (request.Items == null || request.Items.Any(i => string.IsNullOrWhiteSpace(i.Item)))
                {
                    response = new CreateQuoteResponse
                                   {
                                       Confirmation = new Confirmation
                                                          {
                                                              Level = ConfirmationLevel.Error,
                                                              Message = ErrorMessage
                                                          }
                                   };
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Customer))
                {
                    return new CreateQuoteResponse
                    {
                        Confirmation = new Confirmation
                        {
                            Level = ConfirmationLevel.Error,
                            Message = "Customer or Items cannot be null or empty"
                        }
                    };
                }

                foreach (var item in request.Items)
                {
                    if (item.Quantity <= 0)
                    {
                        return new CreateQuoteResponse
                        {
                            Confirmation = new Confirmation
                            {
                                Level = ConfirmationLevel.Error,
                                Message = "quantity must be greater than 0"
                            }
                        };
                    }

                    if (item.UnitaryPrice < 0)
                    {
                        return new CreateQuoteResponse
                        {
                            Confirmation = new Confirmation
                            {
                                Level = ConfirmationLevel.Error,
                                Message = "price must be greater than 0"
                            }
                        };
                    }

                    if (item.DiscountPercentage < 0 || item.DiscountPercentage > 1)
                    {
                        return new CreateQuoteResponse
                        {
                            Confirmation = new Confirmation
                            {
                                Level = ConfirmationLevel.Error,
                                Message = "discount must be between 0 and 1"
                            }
                        };
                    }
                }


                // XSS protection
                request.Customer = System.Net.WebUtility.HtmlEncode(request.Customer);

                // Initialize a new Quote
                var quote = new Quote
                                {
                                    ID = Guid.NewGuid(),
                                    Customer = request.Customer,
                                    Revision = 1,
                                    Status = QuoteStatus.Active,
                                    Lines = new List<QuoteLine>(),
                                    TotalPrice = 0m
                                };

                // Process each item in the request
                foreach (var item in request.Items)
                {
                    var line = new QuoteLine
                                   {
                                       Item = item.Item,
                                       Quantity = item.Quantity,
                                       UnitaryPrice = item.UnitaryPrice,
                                       DiscountPercentage = item.DiscountPercentage,
                                       DiscountAmount = (decimal)item.Quantity * item.UnitaryPrice
                                                                               * (decimal)item.DiscountPercentage
                                   };

                    line.LinePrice = ((decimal)line.Quantity * line.UnitaryPrice) - line.DiscountAmount;

                    quote.TotalPrice += line.LinePrice;
                    quote.Lines.Add(line);
                }

                // Save in database should be done here. But not necessary for the exercise.
                response = new CreateQuoteResponse
                               {
                                   Quote = quote,
                                   Confirmation = new Confirmation
                                                      {
                                                          Level = ConfirmationLevel.Success,
                                                          Message = SuccessfulMessage
                                   }
                               };
                return response;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }
    }
}
