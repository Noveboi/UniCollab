PROJ="Saas.Api"
API_PATH="../api/$PROJ/"

DB_PASS="YourStrong(!)Password"
DB_IMG="mcr.microsoft.com/mssql/server:2019-latest"

WEB_PORT=5173
API_PORT=4000

echo "🚪Checking if port $WEB_PORT is free for the React Application..."
if netstat -anp | grep -q ":$WEB_PORT"; then
    echo "⚠️ Port $WEB_PORT is already in use. Exiting..."
    exit 1
fi

echo "🤔 Checking if SQL Server container is running"
if docker ps --filter "ancestor=$DB_IMG" --format "{{.ID}}" | grep -q .; then
    echo "✅ Found running SQL Server container!"
else 
    echo "🐳 No running container found, firing up a new SQL Server container."
    docker run --rm -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$DB_PASS" -p 5433:1433 -d $DB_IMG
fi

echo "🛠️ Building .NET API"
dotnet build "$API_PATH/$PROJ.sln"

echo "🚀 Launching .NET API"
dotnet run --launch-profile "http" --project "$API_PATH/src/$PROJ/$PROJ.csproj" --no-build -v n &

sleep 1

echo "⚛️ Launch React Application"
npm run dev