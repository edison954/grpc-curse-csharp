
Protocol Buffer:
- deinfe Messages (data, request and response)
- define service (service name and rpc endpoints)

Efficiencia de protocol buffer over json

- grpc uses protocol buffers for communications
- lets measure the payload size vs json:

json: 55 bytes:

{
	"age": 35,
	"firstName": "Edison",
	"lastName": "Plaza"
}

same in protocol buffer: 20 bytes

message Person {
	int32 age = 35;
	string firsName = 2;
	string lastName = 3;
}

- parsin Json is actually cpu intensive (becouse the format is human readable)
- parsin protocol buffers (binary format) is less CPU intensive becouse its closer to
  how a machine represent data
  
- by using fRPC, the use of Protocol Buffer mean faster and efficient communications
  friendly with mobile devices that have a slower cpu
  
https://grpc.io

grpc can be used by andy language
- Becouse the cde can be generated for any language, it makes it super simple
to create micro-services in any language that interact with each other

porque protocol buffers?
- easy to write message definition
- the definition of the api is independent from the implementation
- a huge amount o code can be generated, in any language, from a simple .proto file
- the payload is binary, therefore very efficient to send / recive on a network and serialize /
  deserializer on a CPU
- Protocol buffers defines rules to make an api evolve without breaking
  existing clients, which is helpful for micro-services
 
HTTP2
- grpc leverages http/2 as backbone for communications
https://imagekit.io/demo/http2-vs-http1

- http2 is the newer standard for internet communications that address common
  pitfall of http/1.1 on modern web pages
  
- before we go into http/2 lets look at some http/1.1 request

How http/1.1 works:

- http/1.1 was released in 1997, it has worked great for many years
- opens a new tcp connection to a server at each request
- it does not compress headers (wich are plaintext)
- it only works with request/response mechanism (no server push)
- was originally composed of two commands: get and post
- nowadays a web page loads 80 assets on average
- headers are sent at every request and are plainttesxt (heavy size)
- each request opens a tcp connection
- these inefficiencies add latency and increase network packet size

How http/2 works;

- http2 was released in 2015. it has been battled tested for many years 
  (and was before that tested by google under the name SPDY)
- http2 supports multiplexing:
	- the client & server can push messages in parallel over the same tcp connection
	- this greatly reduces latency
- http2 supports server push
	- server can push streams (multiple messages) for one request frm the client
- supports header compression
  (remember the average http request may have over 20 headers, due to cookies,
  content cache, and application headers)
- http/2 is binary
	while http/1 text makes it easy for debugging, it's not efficient over the network
	(protocol buffers is a binary protocol and makes it a great match for http2)
- http/2 is secure (ssl is not required but recommended by default)
	
HTTP2: bottom line

- less chatter
- more efficent protocol (less bandwidth)
- reduce latency
- increased security

4 types of api in grpc
------------------------
*Unary:
	- is what a tradational api looks like (http rest)
htt2 as we've seen, enables apis to now have streaming capabilities
the server and the client can push multiple messages as part one reques.	

*Server Streaming

*Client Streaming

*Bi Directional Streaming

Scalability in grpc
------------------------
- grpc Servers are asynchronous by default
- this means they do not block threads on request
- therefore each grpc server can serve millions of request in parallel

- grpc clients can be asynchronous or synchronous (blocking)
- the client decides which model works best for the performance needs
- grpc clients can perform client side load balancing
- as a proof of scalabilty:
  google has 10 billon grpc request being made per second iternally
  
Security in grpc 
-------------------------
- By default grpc strongly advocates for you to use SSL (encryption over the wire)
  in your api
- this means that grpc has security as a first class citizen
- each language will provide an api to load grpc with the required
  certificated and provide encryption capability out of the box
- additionally usin interceptors, we can also provide authentication 

grpc vs Rest Api 
--------------------------

GRPC										|REST
----------------------------------------	|---------------------------------------
Protocol buffers - smaller, faster 			|Json - text based, slower, bigger
http/2 (lower latency) - from 2015			|http1.1 (higher latency) from 1997
Bidirectional & Async						|Client => Server requests only
Steam supports								|Request / Response support only
Api oriented - "what"						|CRUD oriented (created-retrieve-update-delete/
 (no constraints - free design)				| post, get, put, delete)
Code generation through protocol buffer		|Code generation through OpenAPi /swagger
 in anny language							| (add-on)
RPC based - grpc does the plumbing for use	|HTTP verbs based - we have to write the porque or use a third party library


https;//husebee.github.io/golang/rest/grpc/2016/05/28/golang-rest-v-grpc.html
grpc is 25 times more performance than rest api. (time to have the response for an api)

Summary: why use grpc
- Easy code definition in over 11 languages
- uses a modern, low latency http2 transport mechanism
- ssl security is built in
- support for streaming apis for maximum performance
- grpc is api oriented, instead of resource oriented like rest

demos:
http://www.http2demo.io/
https://imagekit.io/demo/http2-vs-http1


----------------------------------
----------------------------------

Unary Api
----------------------------------

- are the basic Request/Response
- The client will send one message to the server and will receive one response from the server
- Unary RPC calls will be the most common for your apis
	- are very well suited when your data is small
	. start with unary when writing apis and use streaming api if performance is an issue
- are defined using protocol buffers
- for each rpc call we have to define a "Request" message and a "Response" message



