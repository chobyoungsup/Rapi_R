@echo OFF

ECHO Loading Rapidas App...
  PING -n 5 127.0.0.1>nul    

path %UserProfile%\Desktop\Release\;
start QuickChargeApp.exe


