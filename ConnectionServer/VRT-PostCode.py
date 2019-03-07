
import boto3
import json
import random
import datetime

print('Loading function')
dynamo = boto3.client('dynamodb')

def respond(code, message):
    return {
        'statusCode': code,
        'body': message,
        'headers': {
            'Content-Type': 'application/json',
        },
    }
    
def randChar():
    alphabet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'
    return random.choice(alphabet)
    
def genRandomCode():
    #generates a random code for client
    return randChar() + randChar() + randChar() + randChar()
    
def tryPush(event):
    localIP = event["queryStringParameters"]['local']
    publicIP = event['requestContext']['identity']['sourceIp']
    code = genRandomCode()
    currentDate = datetime.datetime.now()
    formatDate = currentDate.strftime('%m/%d/%Y %H:%M')
    
    #push IP to database
    dynamo.put_item(
        TableName='VRT-IP-table',
        ConditionExpression="attribute_not_exists(code)",
        Item={
            'code': {
                'S': code,
            },
            'localIP': {
                'S': localIP,
            },
            'publicIP': {
                'S': publicIP,
            },
            'session-ended': {
                'BOOL': False,
            },
            'session-length': {
                'N': '5',
            },
            'session-start': {
                'S': formatDate,
            },
        }
    )
    
    return code
    
def addToDatabase(event):
    result = None
    loopCount = 0
    while result is None and loopCount < 15:
        try:
            result = tryPush(event)
            return result
        except:
            loopCount = loopCount + 1
            pass
    #if too many loops, fail
    return None

def assertQuery(event):
    #check if required queries exist
    try:
        data = event["queryStringParameters"]['local']
    except:
        return False
    return True
    
def assertIP(event):
    localIP = event["queryStringParameters"]['local']
    a = localIP.split('.')
    if len(a) != 4:
        return False
    for x in a:
        if not x.isdigit():
            return False
        i = int(x)
        if i < 0 or i > 255:
            return False
    return True

def lambda_handler(event, context):
    #check if required queries exist
    if not assertQuery(event):
        return respond(400, "A Local IP was not given.")
    
    #ensure IP given is valid
    if not assertIP(event):
        return respond(400, "The given local IP is invalid.")
    
    #inserts a new entry when given an item
    connectCode = addToDatabase(event)
    if connectCode == None:
        return respond(502, "Server Timed out.")
    return respond(200, connectCode)