
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
    
def releaseCode(event):
    code = event["queryStringParameters"]['code']
    
    #remove from database
    dynamo.get_item(
        TableName='VRT-IP-table',
        Key={
            'code': {
                'S': code,
            },
        },
        ProjectionExpression='localIP, publicIP'
    )

def assertQuery(event):
    #check if required queries exist
    try:
        data = event["queryStringParameters"]['code']
    except:
        return False
    return True


def lambda_handler(event, context):
    #check if required queries exist
    if not assertQuery(event):
        return respond(400, "A Code was not given.")
    
    #inserts a new entry when given an item
    releaseCode(event)

    return respond(200, "")