thisFile = mfilename('fullpath');
thisFile;

parentDir = fileparts(thisFile);

dataDir = [parentDir filesep 'Data'];
test = [dataDir filesep '*.csv'];
cd(dataDir);

dirData = dir(test);

[m, n] = size(dirData);

for i = 1:m
   file = dirData(i).name;
   [p, name, ext] = fileparts(file);

   fullFile = strcat(dataDir, filesep, file);
   name
   T = readtable(fullFile, 'Delimiter', ',')
end