## School Backend APIs

### Project Prerequisites

<ol>
    <li>Dot Net Core 3.1 runtime or SDK</li>
    <li>Visual Studio 2019 or Later</li>
</ol>

### Steps to get the project up and running

<ol>
    <li>Clone the project from this repository</li>
    <li>Copy appsetting.json.example to appsettings.json and add a random string (16 chars preferred) at the <strong>JWTSettings:SecretKey</strong> value section. (THIS IS A MUST)</li>
    <li>Open the project on visual studio</li>
    <li>Search for package manager console and launch the window</li>
    <li>Run the <strong>update-database</strong> command to start database migrations</li>
    <li>Build and run the application and navigate to http://localhost:5000 for the API documentation</li>
</ol>
