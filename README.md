# Social Media Portal

SMP is a recreation of fundamental social networking features, done with .NET Core + Angular.

## Local setup

### Dependencies

SMP uses and depends on several technologies which are needed to run the application locally. SMP's dependencies are:
- .NET Core 3.1.100 - building and running the migrations, API, and tests
- Node.js - npm
- SQL Server - data storage

As well as these dependencies, SMP also uses various configuration values in `appsettings.json`. An example of a complete `appsettings.json` file is as follows:
```
{
	"DatabaseConnectionString": "Data Source=.;Initial Catalog=SMP;Integrated Security=True;MultipleActiveResultSets=True",
	"Tokens": {
		"Issuer": "localhost:5001",
		"SigningKey": "base64 encoded signing key"
	},
	"Mail": {
		"Host": "127.0.0.1",
		"Port": 25
	},
	"WebApp": {
		"Url":  "https://localhost:5001/" 
  } 
}
```

##### 1. ~/src/Smp.Web/ClientApp:
```
$ npm install
```

##### 2. ~/src/Smp.Web:
```
$ dotnet run
```

##### 3. In the web browser, navigate to localhost:5001

**Note:**
If you use Visual Studio Code, there are various tasks that can be used to help speed development up in `.vscode/tasks.json`. There is also a `launch.json` file which when used will allow you to debug the application in Visual Studio Code.

## Maintainers

[amrwc](https://github.com/amrwc)

[faibz](https://github.com/faibz)