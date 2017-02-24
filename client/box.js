var gpio = require("pi-gpio");

module.exports.open = function() {
    var pin = 12;
    var time = 10000;           // 10 seconds

    gpio.open(pin, "output", function(err) {
        gpio.write(pin, 1, function() {
            setTimeout(function() {
                gpio.close(pin);
            } , time);				
        });
    });
};