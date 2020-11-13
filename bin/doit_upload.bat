rem cd Release
C:
cd C:\renato\GIS4Dv4\binMacOS\
del GIS4Dv4.app.zip
pause


rem "C:\Program Files\WinRAR\rar" a -r GIS4Dv4.app.zip GIS4Dv4.app
"C:\Program Files\WinRAR\winrar" a -afzip -r GIS4Dv4.app.zip GIS4Dv4.app

dir
rem copy GIS4Dv4.rar "G:\Meu Drive\Unity3D Projects\ProjetosRenato\ScanPix4Dprojects\bin\"

rem copy GIS4Dv4.app.zip "G:\Meu Drive\compartilhado\GIS4D\GIS4D_lastest"
rem dir "G:\Meu Drive\compartilhado\GIS4D\GIS4D_lastest"
copy GIS4Dv4.app.zip "G:\Meu Drive\compartilhado\GIS4D\GIS4D_test"
dir "G:\Meu Drive\compartilhado\GIS4D\GIS4D_test"

pause