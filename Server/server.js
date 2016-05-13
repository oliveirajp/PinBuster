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

var port = process.env.PORT || 3000; // set our port

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


//////////////////////////////////////////////////////UTILIZADORES////////////////////////////////////////////////////////////
router.route('/utilizador')

// create a bear (accessed at POST http://localhost:8080/api/test)
.post(function(req, res) {

    // console.log("nome: " + req.body.nome);
    insertData("INSERT dbo.utilizador (nome,imagem,raio,face_id) OUTPUT INSERTED.utilizador_id VALUES (@nome,@imagem,@raio,@face_id);", ['nome', 'imagem', 'raio','face_id'], [req.body.nome, req.body.imagem, req.body.raio,req.body.face_id], [TYPES.NVarChar, TYPES.NVarChar, TYPES.Int, TYPES.NVarChar], function(err, rows) {
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
    })
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

router.route('/utilizador/:face_id')
    .get(function(req, res) {
        getData("SELECT * FROM dbo.utilizador WHERE face_id = '" + req.params.face_id + "'", function(err, rows) {
            if (err) {
                // Handle the error
                res.json(err);
            } else if (rows) {
                res.json(rows['data'][0]);
                // Process the rows returned from the database
            } else {
                res.json(rows);
                // No rows returns; handle appropriately
            }
        });
    });


app.delete('/utilizador/:face_id', function(req, res) {
    getData("DELETE FROM dbo.utilizador WHERE face_id = '" + req.params.face_id + "'", function(err, rows) {
        if (err) {
            // Handle the error
            res.json(err);
        } else if (rows) {
            res.json(rows['data'][0]);
            // Process the rows returned from the database
        } else {
            res.json(rows);
            // No rows returns; handle appropriately
        }
    });
});


/////////////////////////////////////////////////////////////FOLLOW////////////////////////////////////////////////////////////////
router.route('/follow')

// create a bear (accessed at POST http://localhost:8080/api/test)

.post(function(req, res) {

    // console.log("nome: " + req.body.nome);
    insertData("INSERT dbo.follow (follower,followed) OUTPUT INSERTED.follower VALUES (@follower,@followed);", ['follower', 'followed'], [req.body.follower, req.body.followed], [TYPES.NVarChar, TYPES.NVarChar], function(err, rows) {
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
    })
})

// get all the bears (accessed at GET http://localhost:8080/api/test)
.get(function(req, res) {

    getData("SELECT * FROM dbo.follow;", function(err, rows) {
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


//----------------------------
router.route('/follow/:face_id')
    .get(function(req, res) {
        var followQuery;
        if (req.query.f == 'follower')
            followQuery = "SELECT * FROM dbo.follow WHERE follower = '" + req.params.face_id + "'";
        else
            followQuery = "SELECT * FROM dbo.follow WHERE followed = '" + req.params.face_id + "'";

        getData(followQuery, function(err, rows) {
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
    })

.delete(function(req, res) {
    getData("DELETE FROM dbo.follow WHERE followed = '" + req.query.unfollow + "' and follower = '" + req.params.face_id + "'", function(err, rows) {
        if (err) {
            // Handle the error
            res.json(err);
        } else if (rows) {
            var retorno = {};
            retorno['data'] = 'done';
            res.json(retorno);
            // Process the rows returned from the database
        } else {
            res.json(rows);
            // No rows returns; handle appropriately
        }
    });
});


/////////////////////////////////////////////////////////MENSAGENS//////////////////////////////////////////////////////////////
router.route('/mensagem')

// create a bear (accessed at POST http://localhost:8080/api/test)
.post(function(req, res) {
    // console.log("nome: " + req.body.nome);
    insertData("INSERT dbo.mensagem (latitude,longitude,data,tempo_limite,raio,face_id,conteudo,localizacao,categoria) OUTPUT INSERTED.mensagem_id VALUES (@latitude,@longitude,@data,@tempo_limite,@raio,@face_id,@conteudo,@localizacao,@categoria);", ['longitude', 'latitude', 'data', 'tempo_limite', 'raio', 'face_id', 'conteudo', 'localizacao', 'categoria'], [req.body.latitude, req.body.longitude, req.body.data, req.body.tempo_limite, req.body.raio, req.body.face_id, req.body.conteudo, req.body.localizacao, req.body.categoria], [TYPES.Float, TYPES.Float, TYPES.NVarChar, TYPES.Int, TYPES.Int, TYPES.NVarChar, TYPES.NVarChar, TYPES.NVarChar, TYPES.NVarChar], function(err, rows) {
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
    })
})

// get all the bears (accessed at GET http://localhost:8080/api/test)
.get(function(req, res) {
    var mensagemQuery = "";
    if (Object.keys(req.query).length === 0)
        mensagemQuery = "SELECT data FROM dbo.mensagem;";
    else
        mensagemQuery = "SELECT * FROM dbo.mensagem WHERE categoria = '" + req.query.categoria + "'";


    getData(mensagemQuery, function(err, rows) {
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

router.route('/mensagem/:face_id')
    .get(function(req, res) {
        getData("SELECT * FROM dbo.mensagem WHERE face_id = " + req.params.face_id, function(err, rows) {
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


////////////////////////////////////////////////////////ACHIEVEMENTS//////////////////////////////////////////////////////////////
router.route('/achievement')

// create a bear (accessed at POST http://localhost:8080/api/test)
.post(function(req, res) {

    // console.log("nome: " + req.body.nome);
    insertData("INSERT dbo.achievement (cidade,descricao,nome,mensagem_id,face_id) OUTPUT INSERTED.achievement_id VALUES (@cidade,@descricao,@nome,@mensagem_id,@face_id);", ['cidade', 'descricao', 'nome', 'mensagem_id', 'face_id'], [req.body.cidade, req.body.descricao, req.body.nome, req.body.mensagem_id, req.body.face_id], [TYPES.NVarChar, TYPES.NVarChar, TYPES.NVarChar, TYPES.Int, TYPES.NVarChar], function(err, rows) {
        if (err) {
            // Handle the error
            res.json(err);
        } else if (rows) {
            var retorno = {};
            retorno['data'] = 'done';
            res.json(retorno);
            // Process the rows returned from the database
        } else {
            res.json(rows);
            // No rows returns; handle appropriately
        }
    })
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

///////////////////////////////////d/////////////////////PERFIL//////////////////////////////////////////////////////////////
router.route('/achievement/:face_id')
.get(function(req, res) {

    getData("SELECT ac.face_id, ah.nome, ah.nr_mensagens as MessagesNeeded, ac.nr_mensagens as MessagesFound FROM (SELECT nome, COUNT(mensagem_id) as nr_mensagens FROM dbo.achievement WHERE face_id = 0 group by nome ) ah, (SELECT face_id, nome, COUNT(mensagem_id) as nr_mensagens FROM dbo.achievement group by face_id, nome) ac WHERE ah.nome = ac.nome and ac.face_id =" + req.params.face_id , function(err, rows) {
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

///////////////////////////////////d/////////////////////MESSAGES WITH USER INFO//////////////////////////////////////////////////////////////
router.route('/message_user')
.get(function(req, res) {

    getData("SELECT me.*, ut.nome, ut.imagem, ut.face_id FROM dbo.mensagem me, dbo.utilizador ut WHERE me.face_id = ut.face_id" , function(err, rows) {
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



///////////////////////////////////d/////////////////////PARA PERFIL //////////////////////////////////////////////////////////////
router.route('/perfil_info/:face_id')
.get(function(req, res) {

    getData("SELECT (SELECT COUNT(*) FROM dbo.mensagem WHERE face_id = '" + req.params.face_id + "') AS nr_mensagens, (SELECT COUNT(*) FROM dbo.follow WHERE followed = '" + req.params.face_id + "') AS nr_followers, (SELECT COUNT(*) FROM dbo.follow WHERE follower = '" + req.params.face_id + "') as nr_followed" , function(err, rows) {
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


//======================================================TEDIOUS=============================================================
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
    if (err)
        console.log("Resposta azure: " + err);
    else
        console.log("Azure - Connected");
});

var Request = require('tedious').Request;
var TYPES = require('tedious').TYPES;


function getData(Query, callback) {
    var connection = new Connection(config);
    var newdata = [];
    var dataset = {};
    connection.on('connect', function(err) {
        var Request = require('tedious').Request;
        var request = new Request(Query, function(err, rowCount) {
            if (err) {
                callback(err);
            } else {
                if (rowCount < 1) {
                    dataset['data'] = false;
                    callback(null, dataset);
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



function insertData(Query, paramName, paramValue, types, callback) {
    request = new Request(Query, function(err) {
        if (err) {
            callback(err);
        }
    });

    for (i = 0; i < paramName.length; i++) {
        //console.log(paramName[i]+"...."+ types[i]+"...."+paramValue[i]);
        request.addParameter(paramName[i], types[i], paramValue[i]);
    }
    //request.addParameter('Number', TYPES.NVarChar , 'SQLEXPRESS2014');
    //request.addParameter('Cost', TYPES.Int, 11);

    request.on('row', function(columns) {
        columns.forEach(function(column) {
            if (column.value === null) {
                callback(null, 'not done');
            } else {
                callback(null, 'done');
            }
        });
    });
    connection.execSql(request);
}