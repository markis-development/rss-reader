#!/bin/bash

dotnet restore;
dotnet ef database update;
dotnet run;
