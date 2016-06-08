    // server.js
// BASE SETUP
// =============================================================================
// call the packages we need
var express = require('express'); // call express
var app = express(); // define our app using express
var bodyParser = require('body-parser');
var basicAuth = require('basic-auth-connect');
var session = require('express-session');
var cookieParser = require('cookie-parser');
var flash = require('connect-flash');

// configure app to use bodyParser()
// this will let us get the data from a POST
app.use(bodyParser.urlencoded({
    extended: true
}));
app.use(bodyParser.json());

app.set('views', './views');
app.set('view engine', 'jade');

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



function updateTimeLimits() {
  getData("DELETE FROM dbo.mensagem WHERE tempo_limite != 0 and DATEADD(minute,tempo_limite,data) < DATEADD(hour,1,CURRENT_TIMESTAMP)", function(err, rows) {
        if (err) {
            // Handle the error
            console.log("Err: "+err);
        } else if (rows) {
            console.log("Sucess: " +rows);
            // Process the rows returned from the database
        } else {
            console.log("Meh" + rows);
            // No rows returns; handle appropriately
        }
    });
}

setInterval(updateTimeLimits, 45 * 1000);

//////////////////////////////////////////////////////UTILIZADORES////////////////////////////////////////////////////////////
router.route('/utilizador')

// create a bear (accessed at POST http://localhost:8080/api/test)
.post(function(req, res) {

    // console.log("nome: " + req.body.nome);
    insertData("INSERT dbo.utilizador (nome,imagem,raio,face_id) OUTPUT INSERTED.utilizador_id VALUES (@nome,@imagem,@raio,@face_id)", ['nome', 'imagem', 'raio','face_id'], [req.body.nome, req.body.imagem, req.body.raio,req.body.face_id], [TYPES.NVarChar, TYPES.NVarChar, TYPES.Int, TYPES.NVarChar], function(err, rows) {
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

// get all the bears (accessed at GET http://localhost:8080/api/test)
.get(function(req, res) {
    var queryUtilizador = "";
    if(req.query.searchName)
    queryUtilizador = "SELECT * FROM dbo.utilizador WHERE nome LIKE '%" + req.query.searchName+ "%'";
else
    queryUtilizador = "SELECT * FROM dbo.utilizador;";

    getData(queryUtilizador, function(err, rows) {
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
            followQuery = "SELECT nome, followed as face_id, imagem FROM [dbo].[follow],[dbo].[utilizador] WHERE face_id = follower AND face_id ='" + req.params.face_id + "'";
        else
            followQuery = "SELECT nome, follower as face_id, imagem FROM [dbo].[follow],[dbo].[utilizador] WHERE face_id = followed AND face_id ='" + req.params.face_id + "'";


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
    
    //console.log("INSERT dbo.mensagem (latitude,longitude,data,tempo_limite,raio,face_id,conteudo,localizacao,categoria) OUTPUT INSERTED.mensagem_id VALUES ('"+req.body.latitude+"','"+req.body.longitude+"','convert(datetime,'" + req.body.data + "',5)','"+req.body.tempo_limite+"','"+req.body.raio+"','"+req.body.latitude+"','"+req.body.face_id+"','"+req.body.conteudo+"','"+req.body.localizacao+"','"+req.body.categoria+"');");
    getData("INSERT dbo.mensagem (latitude,longitude,data,tempo_limite,raio,face_id,conteudo,localizacao,categoria) OUTPUT INSERTED.mensagem_id VALUES ('"+req.body.latitude+"','"+req.body.longitude+"',convert(datetime,'" + req.body.data + "',5),'"+req.body.tempo_limite+"','"+req.body.raio+"','"+req.body.face_id+"','"+req.body.conteudo+"','"+req.body.localizacao+"','"+req.body.categoria+"');", function(err, rows) {
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
.put(function(req, res) {
   // if(req.body.data && req.body.tempo_limite && req.body.raio && req.body.conteudo && req.body.categoria && req.body.mensagem_id)
    //{
        getData("UPDATE dbo.mensagem SET data = convert(datetime,'" + req.body.data + "',5) , tempo_limite = '" + req.body.tempo_limite + "' , raio = '" + req.body.raio + "' , conteudo = '" + req.body.conteudo  + "' , categoria = '" + req.body.categoria + "' WHERE mensagem_id = " + req.body.mensagem_id, function(err, rows) {
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

/*    }
    else
    {
        console.log(req.body.data +" " + req.body.tempo_limite+" " + req.body.raio +" " +req.body.conteudo +" " + req.body.categoria+" " + req.body.mensagem_id);
        res.json("Falta argumentos");
    } */
})

// get all the bears (accessed at GET http://localhost:8080/api/test)
.get(function(req, res) {
    var mensagemQuery = "";

    if(req.query.latitude && req.query.longitude && req.query.raio)
    {
        mensagemQuery = "SELECT * FROM dbo.mensagem;";
        getDataRaio(mensagemQuery,req.query.latitude,req.query.longitude,req.query.raio, function(err, rows) {
            if (err) {
            // Handle the error
            //console.log("Err :" + err);
            res.json(err);
        } else if (rows) {
            //console.log("Normal :" + rows);
            res.json(rows);
            // Process the rows returned from the database
        } else {
            //console.log("Else :" + rows);
            res.json(rows);
            // No rows returns; handle appropriately
        }
    });
    }
    else{
        if(req.query.categoria)
            mensagemQuery = "SELECT * FROM dbo.mensagem WHERE categoria = '" + req.query.categoria + "'";
        else if (Object.keys(req.query).length === 0)
            mensagemQuery = "SELECT * FROM dbo.mensagem;";

        getData(mensagemQuery, function(err, rows) {
        if (err) {
            // Handle the error
            //console.log("Err :" + err);
            res.json(err);
        } else if (rows) {
            //console.log("Normal :" + rows);
            res.json(rows);
            // Process the rows returned from the database
        } else {
            //console.log("Else :" + rows);
            res.json(rows);
            // No rows returns; handle appropriately
        }
    });
    }

});

router.route('/mensagem/:face_id')
.get(function(req, res) {
    getData("SELECT * FROM dbo.mensagem WHERE face_id = '" + req.params.face_id + "'", function(err, rows) {
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

    getData("SELECT ac.face_id, ah.nome, ah.nr_mensagens as MessagesNeeded, coalesce(ac.nr_mensagens, 0) as MessagesFound FROM (SELECT nome, COUNT(mensagem_id) as nr_mensagens FROM dbo.achievement WHERE face_id = 0 group by nome ) ah left join (SELECT face_id, nome, COUNT(mensagem_id) as nr_mensagens FROM dbo.achievement group by face_id, nome) ac on ah.nome = ac.nome and face_id = '" + req.params.face_id +"'" , function(err, rows) {
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
    if(req.query.latitude && req.query.longitude && req.query.raio)
    {
        getDataRaio("SELECT me.*, ut.nome, ut.imagem, ut.face_id FROM dbo.mensagem me, dbo.utilizador ut WHERE me.face_id = ut.face_id",req.query.latitude,req.query.longitude,req.query.raio, function(err, rows) {
            if (err) {
            // Handle the error
            //console.log("Err :" + err);
            res.json(err);
        } else if (rows) {
            //console.log("Normal :" + rows);
            res.json(rows);
            // Process the rows returned from the database
        } else {
            //console.log("Else :" + rows);
            res.json(rows);
            // No rows returns; handle appropriately
        }
    });
    }
    else{
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
    }
});

var auth = require('http-auth');
var basic = auth.basic({
        realm: "PinBuster"
    }, function (username, password, callback) { // Custom authentication
        callback(username === "lgp" && password === "lgp");
    }
);


app.use(cookieParser('secret'));
app.use(session({cookie: { maxAge: 60000 }}));
app.use(flash());
app.use(function(req, res, next){
    res.locals.message = req.flash('message');
    next();
});

router.route('/admin')
.get(auth.connect(basic), function(req, res)  {
  res.render('home', {
    title: 'Welcome'
  });
})
.post(function(req, res) {

  insertData("INSERT dbo.mensagem (latitude,longitude,data,tempo_limite,raio,face_id,conteudo,localizacao,categoria) OUTPUT INSERTED.mensagem_id VALUES (@latitude,@longitude,@data,@tempo_limite,@raio,@face_id,@conteudo,@localizacao,@categoria);", ['longitude', 'latitude', 'data', 'tempo_limite', 'raio', 'face_id', 'conteudo', 'localizacao', 'categoria'], [req.body.latitude, req.body.longitude, (new Date()).toISOString(), 0, req.body.raio, 0, req.body.name, req.body.city, "Exploration"], [TYPES.Float, TYPES.Float, TYPES.NVarChar, TYPES.Int, TYPES.Int, TYPES.NVarChar, TYPES.NVarChar, TYPES.NVarChar, TYPES.NVarChar], function(err, rows) {
      if (err) {
          // Handle the error
      } else if (rows) {
          req.flash('message', 'Exploration pin created successfully.');
          res.redirect('admin');
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
            res.json(rows['data'][0]);
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


// getData Normal
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



//Fazer posts
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




// getData Com distancia
function getDataRaio(Query,lat,lon,raio,callback) {
    var connection = new Connection(config);
    var newdata = [];
    var dataset = {};
    connection.on('connect', function(err) {
        var Request = require('tedious').Request;
        var request = new Request(Query, function(err, rowCount) {
            if (err) {
                console.log("Erro :" + err);
                callback(err);
            } else {
                if (rowCount < 1) {
                    dataset['data'] = false;
                    callback(null,dataset);
                } else {
                    dataset = {};
                    dataset['data'] = newdata;
                    callback(null,dataset);
                }
            }
        });
        request.on('row', function(columns) {
            dataset = {};
            columns.forEach(function(column) {

                dataset[column.metadata.colName] = String(column.value).trim();
            });

            var dist = findDistance(dataset['latitude'],dataset['longitude'],lat,lon);
            console.log("Distancia: " + dist + " Loc: " + dataset['localizacao'] + " Lat: " + dataset['latitude'] + " Lon: " + dataset['longitude']);

            if(dist <= raio){
                dataset['visivel'] = 0;
                if(dist <= dataset['raio'] /1000)
                dataset['visivel'] = 1;
                 newdata.push(dataset);
            }
        });
        connection.execSql(request);
    });
}


/* main function */
function findDistance(t1, n1, t2, n2) {

var Rm = 3961; // mean radius of the earth (miles) at 39 degrees from the equator
var Rk = 6373; // mean radius of the earth (km) at 39 degrees from the equator
var  lat1, lon1, lat2, lon2, dlat, dlon, a, c, dm, dk, mi, km;

        // get values for lat1, lon1, lat2, and lon2

        // convert coordinates to radians
        lat1 = deg2rad(t1);
        lon1 = deg2rad(n1);
        lat2 = deg2rad(t2);
        lon2 = deg2rad(n2);

        // find the differences between the coordinates
        dlat = lat2 - lat1;
        dlon = lon2 - lon1;

        // here's the heavy lifting
        a  = Math.pow(Math.sin(dlat/2),2) + Math.cos(lat1) * Math.cos(lat2) * Math.pow(Math.sin(dlon/2),2);
        c  = 2 * Math.atan2(Math.sqrt(a),Math.sqrt(1-a)); // great circle distance in radians

        dk = c * Rk; // great circle distance in km

        // round the results down to the nearest 1/1000
        km = round(dk);

        // display the result
        return km;
    }


    // convert degrees to radians
    function deg2rad(deg) {
        rad = deg * Math.PI/180; // radians = degrees * pi/180
        return rad;
    }


    // round to the nearest 1/1000
    function round(x) {
        return Math.round( x * 1000) / 1000;
    }
