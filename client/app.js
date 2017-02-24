var camera = require('./camera');
var cloud = require('./cloud');
var box = require('./box');
var path = require('path');
var file = path.join(__dirname, 'result.jpg')
 

cloud.evaluate(file, 
    function(str) {
        if (!!str && str.length !== 0) {
            box.open();
        } 
    }
);


console.log("command executed.");