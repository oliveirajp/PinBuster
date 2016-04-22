// server.js
// BASE SETUP
// =============================================================================
// call the packages we need
var express = require('express'); // call express
var app = express(); // define our app using express
var bodyParser = require('body-parser');

// configure app to use bodyParser()
// this will let us get the data from a POST
app.use(bodyParser.urlencoded({
    extended: true
}));
app.use(bodyParser.json());

var port = process.env.PORT || 8080; // set our port

// ROUTES FOR OUR API
// =============================================================================
var router = express.Router(); // get an instance of the express Router

// middleware to use for all requests
router.use(function(req, res, next) {
    // do logging
    console.log('Something is happening.');
    next(); // make sure we go to the next routes and don't stop here
});

// test route to make sure everything is working (accessed at GET http://localhost:8080/api)
router.get('/', function(req, res) {
    res.json({
        message: 'hooray! welcome to our api!'
    });
});

// more routes for our API will happen here


//UTILIZADORES
router.route('/utilizador')

// create a bear (accessed at POST http://localhost:8080/api/test)
.post(function(req, res) {

})

// get all the bears (accessed at GET http://localhost:8080/api/test)
.get(function(req, res) {

    getData("SELECT * FROM dbo.utilizador;", function(err, rows) {
    if (err) {
        // Handle the error
        res.json(err);
    } else if (rows) {
        res.json(rows);
        // Process the rows returned from the database
    } else {
        res.json(rows);
        // No rows returns; handle appropriately
    }
});
});



//MENSAGENS
router.route('/mensagem')

// create a bear (accessed at POST http://localhost:8080/api/test)
.post(function(req, res) {

})

// get all the bears (accessed at GET http://localhost:8080/api/test)
.get(function(req, res) {
    getData("SELECT * FROM dbo.mensagem;", function(err, rows) {
    if (err) {
        // Handle the error
        res.json(err);
    } else if (rows) {
        res.json(rows);
        // Process the rows returned from the database
    } else {
        res.json(rows);
        // No rows returns; handle appropriately
    }
});
});



//ACHIEVEMENTS
router.route('/achievement')

// create a bear (accessed at POST http://localhost:8080/api/test)
.post(function(req, res) {

})

// get all the bears (accessed at GET http://localhost:8080/api/test)
.get(function(req, res) {

    getData("SELECT * FROM dbo.achievement;", function(err, rows) {
    if (err) {
        // Handle the error
        res.json(err);
    } else if (rows) {
        res.json(rows);
        // Process the rows returned from the database
    } else {
        res.json(rows);
        // No rows returns; handle appropriately
    }
});
});



//FOLLOW
router.route('/follow')

// create a bear (accessed at POST http://localhost:8080/api/test)
.post(function(req, res) {

})

// get all the bears (accessed at GET http://localhost:8080/api/test)
.get(function(req, res) {

    getData("SELECT * FROM dbo.achievement;", function(err, rows) {
    if (err) {
        // Handle the error
        res.json(err);
    } else if (rows) {
        res.json(rows);
        // Process the rows returned from the database
    } else {
        res.json(rows);
        // No rows returns; handle appropriately
    }
});
});


// REGISTER OUR ROUTES -------------------------------
// all of our routes will be prefixed with /api
app.use('/api', router);

// START THE SERVER
// =============================================================================
app.listen(port);
console.log('Magic happens on port ' + port);


//connect to sql database
var Connection = require('tedious').Connection;
var config = {
    userName: 'pinbusteradmin@pinbuster.database.windows.net',
    password: 'PBadmin2016',
    server: 'pinbuster.database.windows.net',
    // If you are on Microsoft Azure, you need this:
    options: {
        encrypt: true,
        database: 'pinbuster'
    }
};
var connection = new Connection(config);
connection.on('connect', function(err) {
    // If no error, then good to proceed.
    console.log("Connected");
    console.log("resposta azure: " + err);
});

var Request = require('tedious').Request;
var TYPES = require('tedious').TYPES;


//original query
function original(Query) {
    request = new Request(Query, function(err) {
        if (err) {
            console.log(err);
        }
    });
    var result = "";
    request.on('row', function(columns) {
        columns.forEach(function(column) {
            if (column.value === null) {
                console.log('NULL');
            } else {
                result += column.value + " ";
            }
        });
        console.log("resultado: " + result);
        result = "";
    });

    request.on('done', function(rowCount, more) {
        console.log(rowCount + ' rows returned');
    });
    connection.execSql(request);
}

function getData(Query, callback){
    var connection = new Connection(config);
    var newdata = [];
    var dataset = {};
    connection.on('connect', function(err) {

        var Request = require('tedious').Request;
        var request = new Request(Query, function (err, rowCount) {
            if (err) {
                callback(err);
            } else {
                if (rowCount < 1) {
                    dataset['data'] = false;
                    callback(null,  dataset);
                } else {
                    dataset = {};
                    dataset['data'] = newdata;
                    callback(null, dataset);
                }
            }
        });

        request.on('row', function(columns) {
            dataset = {};
            columns.forEach(function(column) {
            dataset[column.metadata.colName] = String(column.value).trim();

         });

           newdata.push(dataset);

        });

        connection.execSql(request);

    });

}

