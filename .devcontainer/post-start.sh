#!/bin/sh

# pull and start the HMS database
docker run -d -p 1433:1433 marcelv/project_hms_db

