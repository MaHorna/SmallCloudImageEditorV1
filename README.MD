/* 2024 intro */

DONT REPRODUCE !

One of my first non console projects, actually had to reverse engineer this version from build cause i didnt use any versioning system at the time and deleted last version...

Its very crude , lots of bugs, security issues(connection keys in code), poor practices(no OOP), its a blasphemy to even post it here, please dont reproduce. Lot of things were rushed cause ive set myself a deadline of i think 3 days ?.

Servers no longer work - 30 day free server. But it worked at the time and sparked interest in online, multiplayer experiences for me. You could basically use this image editor as image social network because it offered quick posting and viewing of other images.  Had a early 2000s vibe with the clunkiness of the system. 

/*************/


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
