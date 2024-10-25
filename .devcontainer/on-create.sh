#!/bin/sh

## install and trust dev cert
dotnet dev-certs https --clean && dotnet dev-certs https --trust
sudo dotnet workload install wasm-tools
sudo dotnet workload update
