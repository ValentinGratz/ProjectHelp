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


const {MONGO_ADDR, MONGO_PORT, MONGO_DATABASE, PORT, JWT_SECRET} = process.env;

const register = (db, username, mail, password, role) => {

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


const login = (db, username, password) => {
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
				}, JWT_SECRET);

				return token;

			} else return "";
		});
	}).catch((err) => {return ""});
}


MongoClient.connect(`mongodb://${MONGO_ADDR}:${MONGO_PORT}`).then((client) => {

	db = client.db(MONGO_DATABASE);
	console.log("CONNECTION SUCCESS");

	app.post("/api/login", (req, res) => {

		login(db, req.body.username, req.body.password).then((result) => {
			res.json({token: result});
		});
	});
});


app.listen(PORT, () => {
	console.log(`App listen on ${PORT}`);
});


