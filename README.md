# emailHttpFunction
This is a very simple function showing how to relay an email through a function to Azure Communication Services.

The body of the request should look something like:
```json
{
    "toAddress" : "recipient@domain.com",
    "toDisplayName" : "Chris Smith",
    "subject" : "Test of Azure Communication Services",
    "body" : "This is a test of email through Azure Communication Services"
}
```

The configuration settings will need the following values:
* "DONOTREPLY_ADDRESS" : "DoNotReply@domain.com",
* "COMMUNICATION_SERVICES_CONNECTION_STRING":"connection string from Communication Service"

> **_NOTE:_** The "DONOTREPLY_ADDRESS" address value MUST match the "From" address displayed in the Email Communication Services Domain configuration, e.g. DoNotReply@domain.com, else the call will fail.