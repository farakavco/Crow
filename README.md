Title: Pigeon API Guideline
Author: Amin Etesamian
Last Modified: 18/02/2016
Version: 2.0

Introduction:
This Web API is (preferbly) used to transfer log messages from monitoring services to Slack channels.

<<<<<<< HEAD
# Last Modified: 03/02/1395

# Version: 0.0.2a
=======
Usage:
Developer is required to add a header to the request to include the token decoded using the required 
algorithm. In order to deliver text to slack channels, submit your request in an application/Json 
format containing the text and the receiving channel. You could also add another parameter named
"TelegramChannel" as a backup, thus in case slack delivery fails, the message is sent to this
telegramchannel.

>>>>>>> Pigeon

Example:
{
    "text": "This Is A Test Message",       
    "channel": "@amin"
    (optional)"TelegramChannel": "amin"

}
In order to deliver file to slack channels, add these parameters: channel, file(as file byte[]) and the filename.
In case of successful delivery to Slack, a response is received in the following format indicating the success of the sending operation.

<<<<<<< HEAD
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


=======
{
    "ok": "true",
    "error": null
}
Else, a failure message is received including the details of the failure:

{
    "ok": "false",
    "error": failure details
}


Working Example in Python:
​{
from datetime import datetime
import requests
import sys
import argparse
import jwt
​
__version__ = '0.1.0'
​
​
​
parser = argparse.ArgumentParser(description='*** help for slack message sender ***',
                                 epilog='*** and that`s how you communicate with slack. ***',
                                 add_help=True,
                                 prog=sys.argv[0])
​
parser.add_argument(metavar='CHANNEL',
                    dest='channel',
                    help='Specify the channel name. it should be string and is required.')
​
parser.add_argument('-u', '--slack-url',
                    help='the api url to send message. defualt value is http://crow.farakav.com/api/message/ .',
                    default='http://crow.farakav.com/api/message/')
​
parser.add_argument('-f', '--time-format',
                    help='format the message time.',
                    default='%Y-%m-%d %H:%M:%S')
​
parser.add_argument('-t', '--token',
                    default='xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx',
                    help='the specific token to communicate with slack bot. it should be string. required.')
​
parser.add_argument('-s', '--secret-key',
                    help='The secret key to generate the JWT Token.',
                    default='xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx')
​
​
args = parser.parse_args()
​
​
def create_jwt_token():
    return jwt.encode(dict(token=args.token), args.secret_key, algorithm='xxxxxxxx')
​
​
def create_headers():
    return {
        'X-JWT-Token': create_jwt_token()
    }
​
​
def send_to_slack(message):
​
    now = datetime.now().strftime(args.time_format)
    message.format(now)
    slack_message = {
        'text': message,
        'channel': args.channel
    }
​
    response = requests.post(
        args.slack_url,
        slack_message,
        headers=create_headers()).json()
    print(str(response))
​
​
def main():
    try:
        while True:
            line = sys.stdin.readline()
            if not line.strip():
                return 0
            send_to_slack(line)
    except KeyboardInterrupt:
        print('CTRL+C pressed')
        return 1
​
​
if __name__ == '__main__':
    sys.exit(main())
}
