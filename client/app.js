var camera = require('./camera');
var cloud = require('./cloud');
var box = require('./box');
 
camera.takeImage(function (image) { 
    cloud.evaluate(image, function() { 
        box.open();
    });
});


console.log("command executed.");