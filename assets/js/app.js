let client = new Vue({
	el: "#sign-up",
	data: {
		username: "",
		mail: "",
		password: "",
		role: "",
		message: "",
		token: ""
	},
	methods: {
		submit () {

				fetch("http://localhost:3000/api/login",
				{
					method: "POST",
					headers: {
						'Accept': 'application/json, text/plain, */*',
						'Content-Type': 'application/json'
					},
					body: JSON.stringify({
						username: this.username,
						password: this.password,
					})
				})
				.then((res) => { return res.json(); })
				.then((res) => {

					try {
						const data = JSON.parse(atob(res.token.split('.')[1]));
						const {username, mail, role} = data.data;
						this.username = username;
						this.mail = mail;
						this.role = role;
						this.token = res.token;
						console.log(this.username, this.mail, this.role, this.token);
						document.getElementById("result").className = "ui message success";
						this.message = "Authentification réussie";
					} catch (e) {
						document.getElementById("result").className = "ui message error";
						this.message = "Authentification refusée"
					}
				});

			
		}
		
	}
});