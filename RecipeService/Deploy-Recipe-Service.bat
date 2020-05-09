echo Step 1) Build the Docker container for Recipe Service/Postgres/etc.
docker-compose --project-name dirty_paws_cookbook build

echo Step 2) Run the Docker container we just created 
start cmd /k docker-compose --project-name dirty_paws_cookbook up

pause