const fs = require('fs'); // instead of: import fs from 'fs';

function execute() {
    let subfolderOrPath = '/'; // subpath, depends on where and how you install the application
	let createIISUrlRewrite = false;

    // get args
    if (process && process.argv && process.argv.length > 2) {
        subfolderOrPath = process.argv[2];
		// ensure it starts and ends with '/'
        if (!subfolderOrPath.startsWith('/')) {
			subfolderOrPath = '/' + subfolderOrPath;
        }
        if (!subfolderOrPath.endsWith('/')) {
			subfolderOrPath = subfolderOrPath + '/';
        }
		if (process.argv.length > 3) {
			let p3 = process.argv[3];
			if(p3 == true || ((p3) && p3.toLowerCase() == 'true')){
				createIISUrlRewrite = true;
			}
		}
    }
	
	// execute actions
		
	// set base tag
	setBaseTagInIndexFile(subfolderOrPath);
		
	// IIS config
	if(createIISUrlRewrite){
		setIISRewriteWebConfig(subfolderOrPath);
	}
	
}

function setBaseTagInIndexFile(subfolderOrPath){
	
	const filePath = 'build/index.html';
	let devBaseTag = '<base href="/" data-href="/">';
    let prodTag = '<base href="' + subfolderOrPath + '" data-href="' + subfolderOrPath + '">';
    if (subfolderOrPath == '') {
		prodTag = ''; // completly remove it
    }

	console.log('HTML base tag in html file is adapated so it works with RouterBrowser if SPA is not in root');
	replaceStringInFile(filePath, devBaseTag, prodTag);
}

function setIISRewriteWebConfig(subfolderOrPath){
	const sourceWebConfig = 'scripts/web.config';
	const targetWebConfig = 'build/web.config';
	console.log('web.config is set for IIS Url Rewrite (ATTENTION: requires IIS extension');
	
	// step 1: copy
	fs.copyFile(sourceWebConfig, targetWebConfig, (err) => {
		if (err){
			console.log('error coping file web.config! Error: ' + err.message);
			throw err;
		}
		let placeholder = '{webapp}';
		replaceStringInFile(targetWebConfig, placeholder, subfolderOrPath);
	});
}
	

function replaceStringInFile(filePath, placeholder, replaceValue){
	console.log('File ' + filePath + ' will be modified. Value "' + placeholder + '" will be replaced with "' + replaceValue + '"');
	
	fs.readFile(filePath, 'utf8', function (err, data) {
        if (err) {
            console.log('error reading file' + filePath + ' Error: ' + err.message);
			throw err;
        }

		let result = data.replace(placeholder, replaceValue);
        fs.writeFile(filePath, result, 'utf8', function (err) {
            if (err){ 
				console.log('error writing file' + filePath + ' Error: ' + err.message);
				throw err;
			}
        });
	});
}

// run it
execute();
