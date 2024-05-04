

/* original 2018-ish text*/
Small-Cloud-Image-Editor

Small image editor where you can upload you images and see other people images too :)

Small image editor with upload feature via ftp to sever and uploiad of image information to database (mysql)

Usage (buttons)

Import image - selects image for editing

Save image - saves image to selected folder

Upload online - uploads your image to server via ftp connection Author and image name is required.

Show other’s images - loads images stored on the server, sorted by newest and shows info about them stored in database. Changes UI and adds ability to see more images or return to editor. Loading can take quite a while because all images are being downloaded(slow ftp). By clicking on a loaded image you can save it to your selected filder (I recommend saving with .bmp/.jpg/... extension)

Editor functions: (generaly its better to use smaller values 3-15) 

Scale - scales image, "x" pixels changes to 1 pixel

Blur/compress - blurs compresses image 

Posterize - reduces color pallet.value 3 on slider = 8 colors, 4 = 27 colors ... 

Reduce islands - removes single pixels, pixel color is replaced by color most present yround it

Undo - reverts change. (There is 10 previous versions saved)

/****************/
