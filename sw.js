importScripts("sw-toolbox.js");
toolbox.precache(["index.html", "assets/css/semantic.css"]);
toolbox.router.get("/*", toolbox.networkFirst, {
	"networkTimeoutSeconds": 5
});