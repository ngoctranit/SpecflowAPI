Feature: Quote Creation
  The quote creation API should return a success message and correct total for various item and discount scenarios.

  Scenario: AC1 - Create quote with one item
    Given a new quote for a customer with the following items:
      | Name   | Quantity | UnitaryPrice | DiscountPercentage |
      | Widget | 1        | 100.00       | 0.0                |
    When I send the quote creation request 
    Then the response status code should be 200
    And the response message should be "Quote created successfully."
    And the total value should be 100.00

  Scenario: AC2 - Create quote with one item and discount
    Given a new quote for a customer with the following items:
      | Name   | Quantity | UnitaryPrice | DiscountPercentage |
      | Gadget | 2        | 50.00        | 0.1                |
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."
    And the total value should be 90.00

  Scenario: AC3 - Create quote with two items
    Given a new quote for a customer with the following items:
      | Name   | Quantity | UnitaryPrice | DiscountPercentage |
      | ItemA  | 1        | 30.00        | 0.0                |
      | ItemB  | 2        | 15.00        | 0.0                |
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."
    And the total value should be 60.00

  Scenario: AC4 - Create quote with three items
    Given a new quote for a customer with the following items:
      | Name   | Quantity | UnitaryPrice | DiscountPercentage |
      | Item1  | 3        | 10.00        | 0.0                |
      | Item2  | 1        | 100.00       | 0.5                |
      | Item3  | 2        | 25.00        | 0.1                |
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."
    And the total value should be 125.00

  Scenario: AC5 - Create quote with item price = 0
    Given a new quote for a customer with the following items:
      | Name        | Quantity | UnitaryPrice | DiscountPercentage |
      | FreeSupport | 2        | 0.00         | 0.0                |
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."
    And the total value should be 0.00

  Scenario: AC6 - Create quote with discountPercentage = 0
    Given a new quote for a customer with the following items:
      | Name           | Quantity | UnitaryPrice | DiscountPercentage |
      | ItemNoDiscount | 5        | 10.00        | 0.0                |
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."
    And the total value should be 50.00

  Scenario: AC7 - Create quote with discountPercentage = 1 (100%)
    Given a new quote for a customer with the following items:
      | Name     | Quantity | UnitaryPrice | DiscountPercentage |
      | FreeItem | 3        | 20.00        | 1.0                |
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."
    And the total value should be 0.00

  Scenario: AC8 - Create quote with missing customer field
    Given a new quote request without the customer field
    When I send the quote creation request
    Then the response status code should be 400
    And the response message should contain "Customer or Items cannot be null or empty"

  Scenario: AC9 - Create quote with missing items array
    Given a new quote request without the items field
    When I send the quote creation request
    Then the response status code should be 400
    And the response message should contain "Customer or Items cannot be null or empty"

  Scenario: AC10 - Create quote with quantity zero
    Given a new quote for a customer with the following items:
      | Name       | Quantity | UnitaryPrice | DiscountPercentage |
      | InvalidQty | 0        | 100.00       | 0.0                |
    When I send the quote creation request
    Then the response status code should be 400
    And the response message should contain "quantity must be greater than 0"

  Scenario: AC11 - Create quote with negative price
    Given a new quote for a customer with the following items:
      | Name        | Quantity | UnitaryPrice | DiscountPercentage |
      | InvalidPrice| 2        | -10.00       | 0.0                |
    When I send the quote creation request
    Then the response status code should be 400
    And the response message should contain "price must be greater than 0"

  Scenario: AC12 - Create quote with invalid discount percentage
    Given a new quote for a customer with the following items:
      | Name          | Quantity | UnitaryPrice | DiscountPercentage |
      | InvalidDiscHi | 1        | 50.00        | 1.5                |
    When I send the quote creation request
    Then the response status code should be 400
    And the response message should contain "discount must be between 0 and 1"

  Scenario: AC13 - Create quote with large quantity and price
    Given a new quote for a customer with the following items:
      | Name       | Quantity | UnitaryPrice | DiscountPercentage |
      | Expensive  | 100000   | 99999.99     | 0.0                |
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."

  Scenario: AC14 - Create quote with decimal precision
    Given a new quote for a customer with the following items:
      | Name       | Quantity | UnitaryPrice | DiscountPercentage |
      | PreciseOne | 1.25     | 9.99         | 0.0                |
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."
    And the total value should be 12.4875

  Scenario: AC15 - Performance test with many items
    Given a new quote for a customer with 1000 valid items
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."

  Scenario: AC16 - Security test with script injection
    Given a new quote for a customer with name "<script>alert(1)</script>" and a valid item
    When I send the quote creation request
    Then the response status code should be 200
    And the response message should be "Quote created successfully."
    And the response should not contain "<script>"

  Scenario: AC17 - Concurrent quote submissions
    Given multiple users sending quote requests at the same time
    When all requests are processed
    Then all responses should return status code 200
    And each response should contain correct total and confirmation message
