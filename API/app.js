const express = require("express");
const bodyParser = require("body-parser");
const cors = require("cors");
const jwt = require('jsonwebtoken');
const MongoClient = require("mongodb").MongoClient;
const bcrypt = require("bcrypt");

const app = express();

app.use(cors());
app.use(bodyParser());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());


const MONGO_ADDR = process.env.MONGO_ADDR;
const MONGO_PORT = process.env.MONGO_PORT;
const MONGO_DATABASE = process.env.MONGO_DATABASE;
let db;

const getConnectionString = () => {return `mongodb://${MONGO_ADDR}:${MONGO_PORT}`};

const register = (username, mail, password, role) => {

	  return bcrypt.hash(password, 10).then((hash) => {
		const user = {
			username,
			mail,
			password: hash,
			role,
			createdAt: new Date()
		}
		return db.collection("users").insert(user).then((result) => {
			return true;
		});
	});
}


const login = (username, password) => {
	return db.collection("users").findOne({username}).then((result) => {
		return bcrypt.compare(password, result.password).then((res) => {
			if (res) {
				const token = jwt.sign({
					exp: 60,
					data: {
					id: result._id,
					username: result.username,
					mail: result.mail,
					role: result.role
					}   
			}, result.password);

			return token;
		} else {
			return "";
		}

		});
	}).catch((err) => {
		return "";
	});
	
}


MongoClient.connect(getConnectionString()).then((client) => {
	db = client.db(MONGO_DATABASE);
	console.log("Connection success");
});


app.post("/api/login", (req, res) => {
   login(req.body.username, req.body.password).then((result) => {
	res.json({token: result});
   });
});


const port = process.env.port || 3000;

app.listen(port, () => {
	console.log(`App listen on ${port}`);
});


