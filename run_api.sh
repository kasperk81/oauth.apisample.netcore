#!/bin/bash

#############################################
# A script to run the API in a child terminal
#############################################

cd "$(dirname "${BASH_SOURCE[0]}")"

#
# Build the API's code
#
dotnet build
if [ $? -ne 0 ]; then
    echo 'Problem encountered building the API'
    exit
fi

#
# Run the API
#
dotnet run
if [ $? -ne 0 ]; then
    echo 'Problem encountered running the API'
    exit
fi