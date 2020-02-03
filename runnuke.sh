#!/bin/bash
# Script to run cake on AzureDevops

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1   
dotnet tool install --global Nuke.GlobalTool --version 0.24.0
PATH="${PATH}:/$HOME/.dotnet/tools"
if [ "$#" -eq  "0" ]
   then
     nuke PushDockerImageToAzureContainerRegistry
 else
     nuke $1
 fi
