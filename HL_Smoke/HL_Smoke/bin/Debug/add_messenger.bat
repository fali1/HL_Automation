cd /d "%1"
hipadm.exe -addmessenger -adminusername admin -adminpassword admin  -name abc -type smtp -interval 10s -queuename default
pause
