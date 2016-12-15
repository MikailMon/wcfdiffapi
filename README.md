# WCF Diff Api
WCF Rest API that compare diff in two strings based on json request.  

## Technologies 

* Windows Communication Foundation
* .NET Framework v4.6.1
* Code Written in C#

## Requirements

1. You need Visual Studio 2015 to compile and run program and test cases.
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
    
The api will provide you and response.  After that you can repeat the last step, change the endpoint to the second one (see **Endpoints** Section below) and invoke the request in the same way.

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
`
	Response:
    {
    "diffResultType": "Equals"
    }
`

 