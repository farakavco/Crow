# Title: Crow API Guideline

# Author: Amin Etesamian

# Last Modified: 03/02/1395

# Version: 0.0.2a



# Introduction:
	This Web API is (preferbly) used to transfer log messages or files from monitoring services to Slack channels.


# Usage:
	Developer is required to provide the api with the access token of the company channel, the intended message and the destination Slack channel preceded by a "@" using crow.farakav.com/api/message/. The message is sent to the destinitaion Slack channel from SlackBot.
	

# Application/Json Example for sending text:
	
	{
		"token": "xxxxx-xxxxxx-xxxxx",	
		"text": "This Is A Test Message",		
		"channel": "@amin"
		
	}

# Multipart-formdata Example:
		"token": "xxxxxx-xxxxx-xxxxx",
		"channel: "@amin",
		"file": [somebytes],
		"filename": "name of the file including its description"

	
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


