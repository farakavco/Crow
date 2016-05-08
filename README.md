Title: Pigeon API Guideline
Author: Amin Etesamian
Last Modified: 18/02/2016
Version: 2.0

Introduction:
This Web API is (preferbly) used to transfer log messages from monitoring services to Slack channels.

Usage:
Developer is required to add a header to the request to include the token decoded using the required 
algorithm. In order to deliver text to slack channels, submit your request in an application/Json 
format containing the text and the receiving channel. You could also add another parameter named
"TelegramChannel" as a backup, thus in case slack delivery fails, the message is sent to this
telegramchannel.


Example:
{
    "text": "This Is A Test Message",       
    "channel": "@amin"
    (optional)"TelegramChannel": "amin"

}
In order to deliver file to slack channels, add these parameters: channel, file(as file byte[]) and the filename.
In case of successful delivery to Slack, a response is received in the following format indicating the success of the sending operation.

{
    "ok": "true",
    "error": null
}
Else, a failure message is received including the details of the failure:

{
    "ok": "false",
    "error": failure details
}