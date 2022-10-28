#!/bin/bash

pm2 delete TuetrisServer
npm install
pm2 start TuetrisServer.js