# Title: Crow API Guideline

# Author: Amin Etesamian

# Last Modified: 04/02/2016

# Version: 0.0.1



# Introduction:
	This Web API is (preferbly) used to transfer log messages from monitoring services to Slack channels.


# Usage:
	Developer is required to provide the api with the access token of the company channel and the intended message and the destination Slack channel preceded by a "@" in an application/json format using 
crow.farakav.com/api/message/. The message is sent to the destinitaion Slack channel from SlackBot.
	

# Example:
	
	{
		"token": "xxxxx-xxxxxx-xxxxx",	
		"text": "This Is A Test Message",		
		"channel": "@amin"
		
	}

	
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
