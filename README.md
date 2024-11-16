# **Email OTP Module**

## **Functional Requirements**
- Generate and send secure OTPs to email with domain _dso.org.sg_.
- OTP should be valid for only 1 minute.
- Maximum of 10 tries to submit OTP.

## **Non Functional Requirements**
- Handle concurrent user requests effectively.
- Can be easily integrated with applications.

## **Assumptions**
- 'SendEmail' method is implemented and is integrated with a email provider.
- 'IOStream' class contains the email property and the readOTP() is implemented.
- StatusCodes are created as Constants in 'ServiceLayer.Constants' namespace.

## **Testing Approach**
- We can use XUnit and Moq framework to do the automated testing.
- Create a new XUnit test project and create the test classes.
- Using Moq framework create Mock objects and use them in the XUnit test methods.