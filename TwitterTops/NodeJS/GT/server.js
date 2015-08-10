var app   = require('express')();
var http = require('http').Server(app);
var mysql = require('mysql');
var bodyParser = require("body-parser");
var connection = mysql.createConnection({
		host     : 'media.zeboba.com/',
		user     : 'root',
		password : '0909@BLUEzeboba',
		database : 'geotracer',
	});
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

	
app.get('/',function(req,res){
	var data = {
		
	};
	data = "Welcome to Book Store DEMO...";
	res.json(data);
});

app.get('/stream',function(req,res){
		var Bookname = req.query.devicename;
	var data = {"error":1,"devicename":""};
	//data["devicename"] = req.query.devicename;
	
	connection.query("select tracking.* from tracking, devices where latitude!=0 and longitude!=0 and tracking.deviceId = devices.id ORDER BY dateTime DESC LIMIT 1",function(err, rows, fields){
		if(rows.length != 0){
			data["error"] = 0;
			data["devicename"] = rows;
			res.json(data);
		}else{
			data["devicename"] = 'No devices Found..';
			res.json(data);
		}
	});
});

http.listen(8080,function(){
	console.log("Connected & Listen to port 8020");
});