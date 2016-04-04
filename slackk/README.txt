# Title: Crow API Guideline

# Author: Amin Etesamian

# Last Modified: 04/02/2016

# Version: 0.0.1



# Introduction:
	This Web API is (preferbly) used to transfer log messages from monitoring services to Slack channels.


# Usage:
	Developer is required to provide the api with the intended message and the destination Slack channel using /api/message/:


# Example:
	
{
		
"text": intended message,
		
"channel": destination Slack channel
	
}

	
In case of successful delivery to Slack, a response is received in the following format indicating the success of the sending operation:
	
{
		"status": "ok"
	}


	Else, a failure message is received including the details of the failure:
	
{
		"status": "failure",
		"details": failure details
	}
  
