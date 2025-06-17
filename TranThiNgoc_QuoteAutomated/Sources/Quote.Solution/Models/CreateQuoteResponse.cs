// ----------------------------------------------------------------------
// <copyright file="CreateQuoteResponse.cs" company="Eurofins">
//  Copyright 2024 Eurofins Scientific Ltd, Ireland
//  Usage reserved to Eurofins Global Franchise Model subscribers.
// </copyright>
// ----------------------------------------------------------------------

namespace Models
{
    /// <summary>
    /// The create quote response.
    /// </summary>
public class CreateQuoteResponse
{
    public Quote? Quote { get; set; }

    public Confirmation? Confirmation { get; set; }

    /// <summary>
    /// Gets or sets the total value of the quote.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Gets or sets the response message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
}
