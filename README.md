# WCF Diff Api
WCF Rest API compare differences in two strings based on json request.  

## Technologies 

* Windows Communication Foundation.
* .NET Framework v4.6.1.
* Azure Cloud Services.
* Code Written in C#.

## Requirements

1. You'll need Visual Studio 2015 to compile and run program and test cases.
2. Azure SDK Libraries installed on Visual Studio.
3. REST Api client like: [SOAP UI](https://www.soapui.org/) or [POSTMAN Plugin for Chrome](https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop)


## Main Functionality

WCFDiffApi takes two string values encoded in based64 format into a json object and give some insights about the request, if both string are the same or has differences.  The api offer also the capability to know if both request has different sizes.

## How to use

You can compile the project and set the endpoint into your client and call it:

    Example:
    PUT <ENDPOINT> HTTP/1.1
    Host: <HOST>
    Content-Type: application/json        
    {
		"Data":"dzNsY29NMw=="
    }
    
The api will provide you a response.  After that you can repeat the last step, change the endpoint to the second one (see **Endpoints** Section below) and invoke the api in the same way.

To retreive the insights you can use your Rest client and follow the sample below:

	Example:
	GET <ENDPOINT> HTTP/1.1
	Host: <HOST>
	Content-Type: application/json
	Cache-Control: no-cache
	
Simple as that.	

## What you will receive

There is 4 differents types of response:


1. If your left side (first request) is like: `{ "Data": "testing" }` and your right side (second request) is like: `{"Data":"testing"}` the response will be:

	   Response:
        {
            "diffResultType": "Equals"
        }

2. If your left side (first request) is like: `{ "Data": "testing" }` and your right side (second request) is like: `{"Data":"test"}` the response will be:

        Response:
        {
            "diffResultType": "SizeDoNotMatch"
        }

3. If your left side (first request) is like: `{ "Data": "testing" }` and your right side (second request) is like: `{"Data":"t3st1Ng"}` the response will be:

        Response:
        {
        "diffResultType": "ContentDoNotMatch",
        "diffs": [
            {
                "offset":1,
                "length":1
            },
            {
                "offset":4,
                "length":2
            }
        ]
        }

4. If you request a response without set the left or right side first to a specific ID you'll receive:

        Response:
        {
        "diffResultType": "IDHasNoLeftSide"
        }
Or

        Response:
        {
        "diffResultType": "IDHasNoRightSide"
        }

## Endpoints

For the first request you can use:

#### <HOST>/v1/diff/{id}/left

Responses:

HTTP Status Code | Description
------------ | -------------
201 Created | The registration was succesfully inserted in data file
500 Internal Server Error | Data input is not base64 encoded
501 Not Implemented | Only left and right are accepted at the end of the url.  Example: <HOST>/v1/diff{id}/center is not allowed

For the second request you can use:

#### <HOST>/v1/diff/{id}/right

Responses:

HTTP Status Code | Description
------------ | -------------
201 Created | The registration was succesfully inserted in data file
500 Internal Server Error | Data input is not base64 encoded
501 Not Implemented | Only left and right are accepted at the end of the url.  Example: <HOST>/v1/diff{id}/center is not allowed

For the last request the endpoint is:

#### <HOST>/v1/diff/{id}

You'll retrieve the insights result. The response is described in the ***What you will receive*** section.

## DEMO

There is a pool of endpoint that you could use for ilustration purposes hosted in [Azure Cloud Service](https://portal.azure.com).  You can use the endpoint with any REST Api client.  The Left and Right endpoint are only available by **PUT** method and the third endpoint is available only by **GET**.

    Left Side:
    PUT http://diffed.cloudapp.net/Service.svc/v1/diff/{id}/left

    Right Side:
    PUT http://diffed.cloudapp.net/Service.svc/v1/diff/{id}/right

    Response:
    GET http://diffed.cloudapp.net/Service.svc/v1/diff/{id}

## Notes

* This service API is not using a data base engine like persistent storage.  Instead is using a json file hosted in Microsoft Azure.  Every left and right side request are saving in the json file.

* The Data value in the left/right side request has to be always encoded in base64.

* Input data is case sensitive.  Correct input: `{ "Data":"dzNsY29NMw==" }`. Wrong input: `{ "data":"dzNsY29NMw==" }`

* You can also empty previous request and clear the data file using:

        DELETE http://diffed.cloudapp.net/Service.svc/v1/clear 

## License

MIT License 2016.