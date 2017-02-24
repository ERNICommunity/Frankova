var camera = require('./camera');
var cloud = require('./cloud');
var box = require('./box');
 

camera.takeImage(
    function (image) { 
        cloud.evaluate(image);
        box.open();
    },
    function (error) { console.log(error)}
);


console.log("command executed.");